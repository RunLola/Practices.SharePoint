namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    interface IWorkflowTasksView {
        IList<SPContentType> TaskContentTypes {
            get;
        }

        SPContentType ContentType {
            get;
        }

        IList<SPField> ViewFields {
            get;
        }
        
        DataTable RelatedItems {
            set;
        }
    }
}
