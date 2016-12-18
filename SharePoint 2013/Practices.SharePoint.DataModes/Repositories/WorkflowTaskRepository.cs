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
        protected override string ListTemplate {
            get {
                return "<Lists ServerTemplate='171' BaseType='0' />";
            }
        }

        protected override string WebScope {
            get {
                return "<Webs Scope='SiteCollection' />";
            }
        }
        
        protected override string ViewFields {
            get {
                return string.Format("<FieldRef Name='{0}' />", SPBuiltInFieldName.RelatedItems);
            }
        }
    }
}
