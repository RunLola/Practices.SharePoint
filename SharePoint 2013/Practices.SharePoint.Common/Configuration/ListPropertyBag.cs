namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;

    public class ListPropertyBag : PropertyBag {
        /// <summary>
        /// The SPList that's wrapped by this property bag. 
        /// </summary>
        private SPList list;

        public override ConfigScope Scope {
            get {
                return ConfigScope.List;
            }
        }

        protected override string KeyPrefix {
            get {
                return "List.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPropertyBag"/> class.
        /// </summary>
        /// <param name="web">The SPWeb associated with this property bag.</param>
        public ListPropertyBag(SPList list) {
            this.list = list;
        }

        public override string this[string key] {
            get {
                key = KeyPrefix + key;
                return this.list.RootFolder.Properties[key] as string;
            }
            set {
                key = KeyPrefix + key;
                if (this.Contains(key)) {
                    this.list.RootFolder.SetProperty(key, value);
                } else {
                    this.list.RootFolder.AddProperty(key, value);
                }
                this.list.Update();
            }
        }

        protected override bool Contains(string key) {
            if (this.list.RootFolder.Properties.Contains(key)) {
                return this.list.RootFolder.Properties[key] != null;
            } else {
                return false;
            }
        }

        public override void Remove(string key) {
            key = KeyPrefix + key;
            if (this.Contains(key)) {
                this.list.RootFolder.Properties.Remove(key);
                this.list.RootFolder.Update();
            }
        }
    }
}