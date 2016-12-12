namespace Practices.SharePoint.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.Data;

    public abstract class SiteRepository : IListRepository<DataRow> {
        public abstract SPWeb Web {
            get;
        }
        
        public abstract IEnumerable<DataRow> Get(string queryString);

        public abstract IEnumerable<DataRow> Get(string queryString, uint startRow, uint maxRows);

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
