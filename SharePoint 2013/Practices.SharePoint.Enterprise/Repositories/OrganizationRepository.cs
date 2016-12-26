namespace Practices.SharePoint.Repositories {
    using Microsoft.SharePoint;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    
    public class OrganizationRepository: ListRepository {
        private SPWeb clientWeb;

        private SPList list;
        public override SPList List {
            get {
                return clientWeb.Lists[""];
            }
        }

        public OrganizationRepository() 
            : this(SPContext.Current != null ? SPContext.Current.Site.RootWeb : null) {

        }

        public OrganizationRepository(SPWeb clientWeb) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            this.clientWeb = clientWeb.Site.RootWeb;
        }

        //public override IEnumerable<Organization> Get(string queryString) {
        //    throw new NotImplementedException();
        //}

        //public override IEnumerable<Organization> Get(string queryString, uint startRow, uint maxRows) {
        //    throw new NotImplementedException();
        //}

        public Organization Get(int id) {
            var item = List.GetItemById(id);
            return Deserialize(item);
        }
        
        public override IEnumerable<SPListItem> Get(string queryString) {
            var query = new SPQuery() {
                Query = queryString
            };
            return Get(query);
        }

        public override IEnumerable<SPListItem> Get(string queryString, uint startRow, uint maxRows) {
            var query = new SPQuery() {
                Query = queryString
            };
            return Get(query, startRow, maxRows);
        }

        public static Organization Deserialize(SPListItem item) {
            return null;
        }
    }
}
