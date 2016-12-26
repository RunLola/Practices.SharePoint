namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.Data;

    interface ISiteContentByQueryView {
        string ContentTypeBeginsWithId {
            get;
        }

        IList<string> ViewFieldIds {
            get;
        }

        string ListScope {
            get;
        }

        string WebScope {
            get;
        }

        DataTable DataSource {
            set;
        }

        string QueryScope {
            get;
        }

        SPContentType ContentType {
            get;
        }
    }
}
