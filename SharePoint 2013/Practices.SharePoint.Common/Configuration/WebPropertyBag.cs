namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;

    public class WebPropertyBag : PropertyBag {
        /// <summary>
        /// The SPWeb that's wrapped by this property bag. 
        /// </summary>
        private SPWeb web;

        protected override string KeyPrefix {
            get {
                return "Web.";
            }
        }

        public override ConfigScope Scope {
            get {
                return ConfigScope.Web;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebPropertyBag"/> class.
        /// </summary>
        /// <param name="web">The SPWeb associated with this property bag.</param>
        public WebPropertyBag(SPWeb web) {
            this.web = web;
        }

        protected override bool Contains(string key) {
            if (this.web.AllProperties.Contains(key)) {
                return this.web.AllProperties[key] != null;
            } else {
                return false;
            }
        }

        public override string this[string key] {
            get {
                key = KeyPrefix + key;
                return this.web.AllProperties[key] as string;
            }
            set {
                key = KeyPrefix + key;
                if (this.Contains(key)) {
                    this.web.SetProperty(key, value);
                } else {
                    this.web.AddProperty(key, value);
                }
                this.web.Update();
            }
        }
        
        public override void Remove(string key) {
            key = KeyPrefix + key;
            if (this.Contains(key)) {
                this.web.DeleteProperty(key);
                this.web.Update();
            }
        }
    }
}
