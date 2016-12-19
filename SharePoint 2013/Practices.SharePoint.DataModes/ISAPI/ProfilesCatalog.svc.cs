namespace Practices.SharePoint.Services {
    using Microsoft.SharePoint;
    using Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Activation;
    using Utilities;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ProfilesCatalogService : IProfilesCatalogService {

        private SPWeb clientWeb;
        private OrganizationRepository repository;

        #region Constructors

        public ProfilesCatalogService()
            : this(SPContext.Current != null ? SPContext.Current.Web : null) {
        }

        public ProfilesCatalogService(SPWeb clientWeb) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            this.clientWeb = clientWeb;
            this.repository = new OrganizationRepository(clientWeb);
        }

        #endregion

        public IEnumerable<Organization> GetOrgs(int orgId) {
            var queryString = new CAMLQueryBuilder()
                .AddEqual(SPBuiltInFieldName.ParentID, orgId).Build();
            var items = repository.Get(queryString);
            foreach (var item in items) {
                yield return OrganizationRepository.Deserialize(item);
            }
        }

        public IEnumerable<SiteUser> GetUsers(int orgId) {
            var org = repository.Get(orgId);
            SPGroup group = clientWeb.SiteGroups.GetByID(org.GroupId);
            foreach (SPUser user in group.Users) {
                yield return new SiteUser() {
                    LoginName = user.LoginName,
                    DisplayName = user.Name
                };
            }
        }
    }
}