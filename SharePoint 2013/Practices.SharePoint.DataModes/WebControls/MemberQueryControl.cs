namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration.Claims;
    using Microsoft.SharePoint.Portal.ClaimProviders;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MemberQueryControl : SimpleQueryControl {

        private OrganizationClaimHierarchyProvider hiearchyProvider;
        public SPClaimHierarchyProvider HierarchyProvider {
            get {
                if (this.hiearchyProvider == null) {
                    this.hiearchyProvider = new OrganizationClaimHierarchyProvider();
                }
                return this.hiearchyProvider;
            }
        }

        protected override int IssueQuery(string search, string groupName, int pageIndex, int pageSize) {
            return this.IssueQuery(search, groupName, string.Empty, string.Empty, pageIndex, pageSize);
        }

        protected override int IssueQuery(string search, string groupName, string providerID, string hierarchynodeID, int pageIndex, int pageSize) {
            if (search == null || search.Trim().Length == 0) {
                PickerDialog.ErrorMessage = SPResource.GetString("PeoplePickerNoQueryTextError", new object[0]);
                return 0;
            }
            search = search.Trim();
            return base.IssueQuery(search, groupName, providerID, hierarchynodeID, pageIndex, pageSize);
        }
    }
}
