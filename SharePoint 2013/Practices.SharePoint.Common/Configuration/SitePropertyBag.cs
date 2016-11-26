namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;

    public class SitePropertyBag : WebPropertyBag {
        public override ConfigScope Scope {
            get {
                return ConfigScope.Site;
            }
        }

        protected override string KeyPrefix {
            get {
                return "Site.";
            }
        }
        
        public SitePropertyBag(SPSite site)
            : base(site.RootWeb) {
        }
    }
}
