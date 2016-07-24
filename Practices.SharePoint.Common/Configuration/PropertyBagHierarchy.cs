namespace Practices.SharePoint.Configuration {
    using Microsoft.SharePoint;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PropertyBagHierarchy : IPropertyBagHierarchy {
        protected List<IPropertyBag> bags;
        public IEnumerable<IPropertyBag> PropertyBags {
            get {
                return bags;
            }
        }

        public PropertyBagHierarchy(SPWeb web) {
            bags = new List<IPropertyBag>();
            bags.Add(new WebPropertyBag(web));
            bags.Add(new SitePropertyBag(web.Site));
        }
        
        public IPropertyBag GetPropertyBag(ConfigScope scope) {
            foreach (IPropertyBag bag in bags) {
                if (bag.Scope == scope)
                    return bag;
            }
            return null;
        }
    }
}
