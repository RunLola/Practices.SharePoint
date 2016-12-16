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
                return "<Lists ServerTemplate='107' BaseType='0' />";
            }
        }

        protected override string WebScope {
            get {
                return "<Webs Scope='SiteCollection' />";
            }
        }

        protected override string ViewFields {
            get {
                return string.Format("<FieldRef Name='{0}' />", BuiltInFieldName.RelatedItems);
            }
        }
    }
}
