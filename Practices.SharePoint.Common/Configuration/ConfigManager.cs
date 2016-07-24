namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The manager responsible for setting, updating, and deleting configuration values
    /// from property bags, and looking up values from a specific property bag level.
    /// </summary>
    public class ConfigManager : IConfigManager {
        SPWeb web;
        protected SPWeb Web {
            get {
                if (this.web == null && SPContext.Current != null) {
                    web = SPContext.Current.Web;
                }
                return web;
            }
        }

        IConfigSettingSerializer serializer;
        protected virtual IConfigSettingSerializer Serializer {
            get {
                if (serializer == null)
                    serializer = new ConfigSettingSerializer();
                return serializer;
            }
        }

        IPropertyBagHierarchy hierarchy;
        protected virtual IPropertyBagHierarchy Hierarchy {
            get {
                if (hierarchy == null)
                    hierarchy = new PropertyBagHierarchy(Web);
                return hierarchy;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        public ConfigManager() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="web">The web to use as a basis for property bags.</param>
        public ConfigManager(SPWeb web) {
            this.web = web;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        public ConfigManager(IPropertyBagHierarchy hierarchy) {
            this.hierarchy = hierarchy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="serializer">The config setting serializer.</param>
        public ConfigManager(IConfigSettingSerializer serializer) {
            this.serializer = serializer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="hierarchy">instance of <see cref="IPropertyBagHierarchy"/>.</param>
        /// <param name="serializer">The config setting serializer.<see cref="IConfigSettingSerializer"/>.</param>
        public ConfigManager(IPropertyBagHierarchy hierarchy, IConfigSettingSerializer serializer) {
            this.hierarchy = hierarchy;
            this.serializer = serializer;
        }

        public IPropertyBag GetPropertyBag(ConfigScope scope) {
            return Hierarchy.GetPropertyBag(scope);
        }

        public TValue GetPropertyBag<TValue>(string key, IPropertyBag propertyBag) {
            var configValue = propertyBag[key];
            if (configValue != null) {
                object value = Serializer.Deserialize(typeof(TValue), configValue);
                return (TValue)value;
            } else {
                return default(TValue);
            }
        }

        public void SetPropertyBag(string key, object value, IPropertyBag propertyBag) {
            string configValue = Serializer.Serialize(value.GetType(), value);
            propertyBag[key] = configValue;
        }
    }
}
