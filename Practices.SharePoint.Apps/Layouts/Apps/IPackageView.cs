namespace Practices.SharePoint.Apps {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Taxonomy;
    using System.Web.UI.WebControls;

    interface IPackageView {
        Guid ProductId {
            get;
        }

        string Title {
            get;
            set;
        }

        string LaunchUrl {
            get;
            set;
        }

        Dictionary<TaxonomyField, string> Categories {
            get;
            set;
        }

        Dictionary<string, string> Fields {
            get;
            set;
        }

        IEnumerable<ListItem> Roles {
            get;
            set;
        }
    }
}
