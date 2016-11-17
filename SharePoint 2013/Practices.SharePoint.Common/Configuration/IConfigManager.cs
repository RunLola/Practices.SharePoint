namespace Practices.SharePoint.Configuration {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IConfigManager {
        IPropertyBag GetPropertyBag(ConfigScope scope);

        TValue GetPropertyBag<TValue>(string key, IPropertyBag propertyBag);

        void SetPropertyBag(string key, object value, IPropertyBag propertyBag);
    }
}
