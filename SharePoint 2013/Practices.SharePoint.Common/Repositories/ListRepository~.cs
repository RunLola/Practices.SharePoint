namespace Practices.SharePoint.Repositories {
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using Models;

    public abstract class ListRepository<TEntity> : IListRepository<TEntity>
        where TEntity : BaseEntity {
        public abstract SPList List {
            get;
        }

        public virtual int Create(IDictionary<string, string> fields) {
            var item = List.Items.Add();
            foreach (var field in fields) {
                if (item.Fields.ContainsField(field.Key)) {
                    item.Fields.GetField(field.Key).ParseAndSetValue(item, field.Value);
                }
            }
            List.Update();
            return item.ID;
        }

        public virtual int Update(int id, IDictionary<string, string> fields) {
            var item = List.GetItemById(id);
            foreach (var field in fields) {
                if (item.Fields.ContainsField(field.Key)) {
                    item.Fields.GetField(field.Key).ParseAndSetValue(item, field.Value);
                }
            }
            item.Update();
            return item.ID;
        }

        public virtual void Delete(int id) {
            var item = List.GetItemById(id);
            item.Recycle();
        }

        public virtual IDictionary<string, string> Get(int id, IEnumerable<string> fieldNames) {
            var fields = new Dictionary<string, string>();
            var item = List.GetItemById(id);
            foreach (var fieldName in fieldNames) {
                if (item.Fields.ContainsField(fieldName)) {
                    fields.Add(fieldName, item[fieldName] as string);
                }
            }
            return fields;
        }

        public abstract IEnumerable<TEntity> Get(string queryString);

        public abstract IEnumerable<TEntity> Get(string queryString, uint startRow, uint maxRows);
    }
}
