namespace Practices.SharePoint.Repositories {
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    public abstract class ListRepository : IListRepository<SPListItem> {
        public abstract SPList List {
            get;
        }

        public ListRepository() {
            
        }

        public IEnumerable<SPListItem> Get(string queryString) {
            SPQuery query = new SPQuery() {
                Query = queryString
            };
            return List.GetItems(query).Cast<SPListItem>();
        }

        public IEnumerable<SPListItem> Get(uint startRow, uint maxRows, string queryString) {
            SPQuery query = new SPQuery() {
                Query = queryString
            };
            if (startRow > 0) {
                query.RowLimit = startRow;
                query.ListItemCollectionPosition = List.GetItems(query).ListItemCollectionPosition;
            }
            query.RowLimit = maxRows;
            return List.GetItems(query).Cast<SPListItem>();
        }
    }
}
