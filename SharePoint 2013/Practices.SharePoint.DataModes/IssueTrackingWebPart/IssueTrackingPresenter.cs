namespace Practices.SharePoint.WebParts {
    using Microsoft.SharePoint;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Utilities;

    class IssueTrackingPresenter {

        public IssueTrackingPresenter(IIssueTrackingView view, SPWeb web) {

        }

        public IEnumerable<DataRow> LoadIssues() {
            var repository = new IssueTrackingRepository();
            var queryString = new CAMLQueryBuilder().Build();
            return repository.Get(null, 0, 100);
        }
    }
}