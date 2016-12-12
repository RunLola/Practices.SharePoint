namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    interface IIssueTasksView {
        IEnumerable<SPContentType> TaskContentTypes {
            get;
        }

        IEnumerable<SPField> ViewFields {
            get;
        }

        SPList List {
            get;
        }

        DataTable IssueTasks {
            set;
        }
    }
}
