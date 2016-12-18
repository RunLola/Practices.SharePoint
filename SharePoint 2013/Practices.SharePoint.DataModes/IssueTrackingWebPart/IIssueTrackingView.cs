namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.Data;

    interface IIssueTrackingView {
        SPContentType ContentType {
            get;
        }

        IList<SPField> ViewFields {
            get;
        }
        
        DataTable Items {
            set;
        }
    }
}
