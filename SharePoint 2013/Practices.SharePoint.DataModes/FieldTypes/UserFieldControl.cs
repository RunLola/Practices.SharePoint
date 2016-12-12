namespace Practices.SharePoint.FieldTypes {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserFieldControl : BaseFieldControl {
        protected override string DefaultTemplateName {
            get {
                return "QuoteNote";
            }
        }

        protected override void CreateChildControls() {
            TemplateContainer.FindControl("TextField");
        }
    }
}
