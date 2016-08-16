﻿namespace Practices.SharePoint.Repositories {
    using System.Collections.Generic;

    public interface IListRepository<TEntity>
        where TEntity : class {

        IEnumerable<TEntity> Get(string queryString);

        IEnumerable<TEntity> Get(uint startRow, uint maxRows, string queryString);
    }
}