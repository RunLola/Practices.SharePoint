namespace Practices.SharePoint.Providers {
    using Microsoft.SharePoint.Administration.Claims;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint.WebControls;

    public class SiteClaimProvider : SPClaimProvider {
        public static string ProviderDisplayName {
            get { return "员工编号"; }
        }

        public static string ProviderDescription {
            get { return "基础平台的安全令牌服务"; }
        }

        protected static string SiteClaimType {
            //The type of claim that we will return. Our provider only returns the
            //email address, which is the user identifier claim.
            get { return "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn"; }
        }

        protected static string SiteClaimValueType {
            //The type of value that we will return. Our provider only returns email address
            //as a string.
            get { return Microsoft.IdentityModel.Claims.ClaimValueTypes.String; }
        }

        public override string Name {
            get {
                return "SiteClaimProvider";
            }
        }
        
        public override bool SupportsEntityInformation {
            get {
                return false;
            }
        }

        public override bool SupportsHierarchy {
            get {
                return false;
            }
        }

        public override bool SupportsResolve {
            get {
                return false;
            }
        }

        public override bool SupportsSearch {
            get {
                return false;
            }
        }

        public SiteClaimProvider(string displayName) : base(displayName) {
        }


        protected override void FillClaimsForEntity(Uri context, SPClaim entity, List<SPClaim> claims) {
            throw new NotImplementedException();
        }

        protected override void FillClaimTypes(List<string> claimTypes) {
            if (claimTypes == null)
                throw new ArgumentNullException("claimTypes");

            // Add our claim type.
            claimTypes.Add(SiteClaimType);
        }

        protected override void FillClaimValueTypes(List<string> claimValueTypes) {
            if (claimValueTypes == null)
                throw new ArgumentNullException("claimValueTypes");

            // Add our claim value type.
            claimValueTypes.Add(SiteClaimValueType);
        }

        protected override void FillEntityTypes(List<string> entityTypes) {
            if (null == entityTypes) {
                throw new ArgumentNullException("entityTypes");
            }
            entityTypes.Add(SPClaimEntityTypes.User);
        }

        protected override void FillHierarchy(Uri context, string[] entityTypes, string hierarchyNodeID, int numberOfLevels, SPProviderHierarchyTree hierarchy) {
            throw new NotImplementedException();
        }

        protected override void FillResolve(Uri context, string[] entityTypes, SPClaim resolveInput, List<PickerEntity> resolved) {
            FillResolve(context, entityTypes, resolveInput.Value, resolved);
        }

        protected override void FillResolve(Uri context, string[] entityTypes, string resolveInput, List<PickerEntity> resolved) {
            var user = new SiteUser() {
                LoginName = resolveInput,
                DisplayName = resolveInput
            };
            if (null != user) {
                PickerEntity entity = GetPickerEntity(user);
                resolved.Add(entity);
            }
        }

        protected override void FillSchema(SPProviderSchema schema) {
            throw new NotImplementedException();
        }

        protected override void FillSearch(Uri context, string[] entityTypes, string searchPattern, string hierarchyNodeID, int maxCount, SPProviderHierarchyTree searchTree) {
            if (!EntityTypesContain(entityTypes, SPClaimEntityTypes.FormsRole)) {
                return;
            }
            var user = new SiteUser() {
                LoginName = searchPattern,
                DisplayName = searchPattern
            };
            PickerEntity entity = GetPickerEntity(user);
            searchTree.AddEntity(entity);
        }

        private PickerEntity GetPickerEntity(SiteUser user) {
            PickerEntity entity = CreatePickerEntity();
            entity.Claim = new SPClaim(SiteClaimType, user.LoginName, SiteClaimValueType,
                    SPOriginalIssuers.Format(SPOriginalIssuerType.TrustedProvider, ProviderDescription));
            entity.Description = user.DisplayName;
            entity.DisplayText = user.DisplayName;
            entity.EntityData[PeopleEditorEntityDataKeys.AccountName] = user.LoginName;
            entity.EntityData[PeopleEditorEntityDataKeys.DisplayName] = user.DisplayName;            
            entity.EntityType = SPClaimEntityTypes.User;
            entity.IsResolved = true;
            return entity;
        }

        public class SiteUser {
            public string LoginName { get; set; }

            public string DisplayName { get; set; }
        }
    }
}
