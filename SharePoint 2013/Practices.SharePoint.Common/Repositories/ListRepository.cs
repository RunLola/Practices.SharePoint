namespace Practices.SharePoint.Repositories {
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    public abstract class ListRepository : IListRepository<SPListItem> {
        public abstract SPList List {
            get;
        }

        public abstract IEnumerable<SPListItem> Get(string queryString);

        public abstract IEnumerable<SPListItem> Get(string queryString, uint startRow, uint maxRows);

        protected IEnumerable<SPListItem> Get(SPQuery query) {
            return List.GetItems(query).Cast<SPListItem>();
        }

        protected IEnumerable<SPListItem> Get(SPQuery query, uint startRow, uint maxRows) {
            if (startRow > 0) {
                query.RowLimit = startRow;
                query.ListItemCollectionPosition = List.GetItems(query).ListItemCollectionPosition;
            }
            query.RowLimit = maxRows;
            return List.GetItems(query).Cast<SPListItem>();
        }
    }
}
