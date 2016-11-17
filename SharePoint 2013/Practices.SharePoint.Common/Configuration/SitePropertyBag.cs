namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;

    public class SitePropertyBag : WebPropertyBag {
        protected override string KeyPrefix {
            get {
                return "Site.";
            }
        }

        public override ConfigScope Scope {
            get {
                return ConfigScope.Site;
            }
        }

        public SitePropertyBag(SPSite site)
            : base(site.RootWeb) {
        }
    }
}
