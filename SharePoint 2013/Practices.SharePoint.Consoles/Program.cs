namespace Practices.SharePoint.Consoles {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.Office.Server.UserProfiles;
    using Utilities;
    using System.Data;
    using Microsoft.SharePoint.Administration.Claims;

    class Program {
        static void Main(string[] args) {
            var array = "隐患下达任务;隐患整改任务;隐患复查任务;隐患验收任务;隐患签字任务;隐患签字任务;".Trim(';').Split(';').AsEnumerable();

            var siteUrl = "http://58.132.202.99/sites/yanglijun";
            using (SPSite site = new SPSite(siteUrl)) {
                using (SPWeb web = site.OpenWeb()) {
                    var queryString = new CAMLQueryBuilder()
                        .AddCurrentUser(SPBuiltInFieldName.AssignedTo)
                        .OrCurrentUserGroups(SPBuiltInFieldName.AssignedTo)
                        .AddNotEqual(SPBuiltInFieldName.TaskStatus, "Completed")
                        .AddIsNotNull(SPBuiltInFieldName.RelatedItems).Build();
                    var query = new SPSiteDataQuery() {
                        Webs = "<Webs Scope='SiteCollection' />",
                        Lists = "<Lists ServerTemplate='171' BaseType='0' />",
                        ViewFields = "<FieldRef Name='Title' /><FieldRef Name='ContentType' />",
                        Query = queryString,
                        QueryThrottleMode = SPQueryThrottleOption.Override,
                        RowLimit = 100
                    };
                    var data = web.GetSiteData(query);

                    Console.WriteLine(data.Rows.Count);
                    Console.ReadKey();
                    //foreach (SPGroup group in web.SiteGroups) {
                    //    var roleDefinitions = web.RoleDefinitions;
                    //    var roleAssignments = web.RoleAssignments;
                    //    var assignment = new SPRoleAssignment(group);
                    //    var bindings = assignment.RoleDefinitionBindings;
                    //    bindings.Add(roleDefinitions["参与讨论"]);
                    //    roleAssignments.Add(assignment);
                    //    Console.WriteLine(group.Name);
                    //}
//                    SPList list = web.Lists["Workflow Tasks"];
//                    SPView view = list.Views["所有任务"];
//                    view.Query = @"<Where>
//                                    <Or>
//                                        <Membership Type=""CurrentUserGroups"" >
//                                         <FieldRef Name=""AssignedTo"" />
//                                        </Membership>
//                                        <In>
//                                          <FieldRef Name=""AssignedTo"" LookupId=""TRUE"" />
//                                          <Values>
//                                            <Value Type=""Integer"" >
//                                              <UserID />
//                                            </Value>
//                                          </Values>
//                                        </In>
//                                    </Or>
//                                  </Where>";
//                    view.Update();
                }
            }
        }

        public static DataTable GetTable() {
            var table = new DataTable();
            table.Columns.Add("WebId");
            table.Columns.Add("ListId");
            table.Columns.Add("ID");
            table.Columns.Add("Title");
            table.Columns.Add("AssignedTo");            
            table.Columns.Add("NavigateUrl");
            return table;
        }

        static void InitUsers(SPServiceContext context) {
            UserProfileManager manager = new UserProfileManager(context);
            using (var db = new CMSMIPEntities()) {
                var users = db.CMS_SA_USER_INFO_V;
                foreach (var user in users) {
                    if (!manager.UserExists(user.SP账号)) {
                        UserProfile userProfile = manager.CreateUserProfile(user.SP账号);
                    }
                }
            }
        }

        static void InitOrganizations(SPServiceContext context, SPSite site) {
            ProfileSubtypeManager subtypeManager = ProfileSubtypeManager.Get(context);
            string subtypeName = ProfileSubtypeManager.GetDefaultProfileName(ProfileType.Organization);
            ProfileSubtype subtype = subtypeManager.GetProfileSubtype(subtypeName);
            OrganizationProfileManager manager = new OrganizationProfileManager(context);
            OrganizationProfile rootProfile = manager.RootOrganization;
            using (var db = new CMSMIPEntities()) {
                var root = db.CMS_BA_IN_DEPT_INFO_V.Where(o => o.组织机构上级ID == 0).FirstOrDefault();
                CreateOrganization(root.组织机构名称, root.机构ID, rootProfile, subtype, manager, site);
            }
        }

        static OrganizationProfile CreateOrganization(string displayName, long id, OrganizationProfile parentProfile, ProfileSubtype subtype, OrganizationProfileManager manager, SPSite site) {
            OrganizationProfile profile = manager.CreateOrganizationProfile(subtype, parentProfile);
            profile.DisplayName = displayName;
            using (var db = new CMSMIPEntities()) {
                IEnumerable<string> accountNames = db.CMS_SA_USER_INFO_V.Where(u => u.所属部门ID == id).Select(u => u.SP账号);
                foreach (var accountName in accountNames) {
                    profile.AddMember(accountName, OrganizationMembershipType.Member);
                }
                profile.Commit();
                CreateSiteGroup(profile, accountNames, site);
                var subOrganizations = db.CMS_BA_IN_DEPT_INFO_V.Where(o => o.组织机构上级ID == id);
                foreach (var subOrganization in subOrganizations) {
                    CreateOrganization(subOrganization.组织机构名称, subOrganization.机构ID, profile, subtype, manager, site);
                }
                return profile;
            }
            Console.WriteLine(displayName + id);
        }

        static void CreateSiteGroup(OrganizationProfile parentProfile, IEnumerable<string> accountNames, SPSite site) {
            var groupName = parentProfile.DisplayName + parentProfile.RecordId;
            site.RootWeb.SiteGroups.Add(groupName, site.Owner, null, null);
            var group = site.RootWeb.SiteGroups.GetByName(groupName);
            foreach (var accountName in accountNames) {
                group.AddUser(accountName, null, null, null);
            }
        }
    }
}
