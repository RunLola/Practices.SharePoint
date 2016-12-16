namespace Practices.SharePoint.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.Data;

    public abstract class SiteRepository : IListRepository<DataRow> {
        private SPWeb web;
        protected SPWeb Web {
            get {
                return web;
            }
        }

        protected abstract string ListTemplate { get; }

        protected abstract string WebScope { get; }

        protected abstract string ViewFields { get; }

        public SiteRepository() 
            : this(SPContext.Current != null ? SPContext.Current.Web : null) {
        }

        public SiteRepository(SPWeb clientWeb) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            this.web = clientWeb;
        }

        public virtual IEnumerable<DataRow> Get(string queryString) {
            var query = new SPSiteDataQuery() {
                Lists = ListTemplate,
                Webs = WebScope,
                ViewFields = ViewFields,
                Query = queryString,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
            return Get(query);
        }

        public virtual IEnumerable<DataRow> Get(string queryString, uint startRow, uint maxRows) {
            var query = new SPSiteDataQuery() {
                Lists = ListTemplate,
                Webs = WebScope,
                ViewFields = ViewFields,
                Query = queryString,
                RowLimit = startRow + maxRows,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
            return Get(query, startRow, maxRows);
        }

        protected IEnumerable<DataRow> Get(SPSiteDataQuery query) {
            return Web.GetSiteData(query).AsEnumerable();
        }

        protected IEnumerable<DataRow> Get(SPSiteDataQuery query, uint startRow, uint maxRows) {
            query.RowLimit = startRow + maxRows;
            var rows = Web.GetSiteData(query).AsEnumerable().AsEnumerable();
            return rows.Skip(int.Parse(startRow.ToString())).Take(int.Parse(maxRows.ToString()));
        }
    }
}
