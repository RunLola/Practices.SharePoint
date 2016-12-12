namespace Practices.SharePoint.Repositories {
    using System.Collections.Generic;
    using Microsoft.SharePoint;

    public interface IListRepository<TEntity>
        where TEntity : class {

        IEnumerable<TEntity> Get(string queryString);

        IEnumerable<TEntity> Get(string queryString, uint startRow, uint maxRows);
    }
}