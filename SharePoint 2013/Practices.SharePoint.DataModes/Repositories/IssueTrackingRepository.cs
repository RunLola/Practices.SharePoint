namespace Practices.SharePoint.Repositories {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;

    public class IssueTrackingRepository : SiteRepository {
        protected override string ListTemplate {
            get {
                return "<Lists ServerTemplate='1100' BaseType='5' />";
            }
        }

        protected override string WebScope {
            get {
                return "<Webs Scope='SiteCollection' />";
            }
        }
        private IEnumerable<string> fieldNames;
        protected override string ViewFields {
            get {
                string viewFields = string.Empty;
                foreach (var fieldName in fieldNames) {
                    viewFields += string.Format("<FieldRef Name='{0}' />", fieldName);
                }
                return viewFields;
            }
        }

        public IssueTrackingRepository(IEnumerable<string> fieldNames) {
            this.fieldNames = fieldNames;
        }
    }
}
