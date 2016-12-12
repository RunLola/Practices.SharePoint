namespace Practices.SharePoint.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.Data;
    using Utilities;
    using System.Web.Script.Serialization;

    public class WorkflowTaskRepository : SiteRepository {
        private SPWeb web;
        public override SPWeb Web {
            get {
                return web;
            }
        }

        public WorkflowTaskRepository() 
            : this(SPContext.Current != null ? SPContext.Current.Web : null) {
        }

        public WorkflowTaskRepository(SPWeb web) {
            this.web = web;
        }

        public override IEnumerable<DataRow> Get(string queryString) {
            var query = new SPSiteDataQuery() {
                Lists = "<Lists ServerTemplate='107' BaseType='0' />",
                Webs = "<Webs Scope='SiteCollection' />",
                ViewFields = string.Format("<FieldRef Name='{0}' />", BuiltInFieldName.RelatedItems),
                Query = queryString,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
            return Get(query);
        }

        public override IEnumerable<DataRow> Get(string queryString, uint startRow, uint maxRows) {
            var query = new SPSiteDataQuery() {
                Lists = "<Lists ServerTemplate='107' BaseType='0' />",
                Webs = "<Webs Scope='SiteCollection' />",
                ViewFields = string.Format("<FieldRef Name='{0}' />", BuiltInFieldName.RelatedItems),
                Query = queryString,
                QueryThrottleMode = SPQueryThrottleOption.Override
            };
            return Get(query, startRow, maxRows);
        }
    }
}
