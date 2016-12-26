namespace Practices.SharePoint.WebControls {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint;
    using System.Web.UI;

    public class FabricTaskListFieldIterator : TaskListFieldIterator {
        public string BottomFields {
            get;
            set;
        }
        
        protected override bool IsTopField(SPField field) {
            if (base.IsTopField(field)) {
                return true;
            }
            string text = ";#" + this.BottomFields + ";#";
            string value = ";#" + field.InternalName + ";#";
            if (text.IndexOf(value) != -1) {
                return false;
            }
            value = ";#" + field.Title + ";#";
            if (text.IndexOf(value) != -1) {
                return false;
            }
            return true;
        }
    }
}
