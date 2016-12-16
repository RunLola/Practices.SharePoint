namespace Practices.SharePoint.Models {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RelatedItem {
        public int ItemId {
            get;
            set;
        }

        public string WebId {
            get;
            set;
        }

        public string ListId {
            get;
            set;
        }
    }
}
