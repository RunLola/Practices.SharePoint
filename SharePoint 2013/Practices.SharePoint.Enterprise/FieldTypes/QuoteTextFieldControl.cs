namespace Practices.SharePoint.FieldTypes {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls;

    public class QuoteTextFieldControl : TextField {
        protected override string DefaultTemplateName {
            get {
                return "QuoteTextField";
            }
        }

        public override string CssClass {
            get {
                return base.CssClass;
            }
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            if (base.ControlMode == SPControlMode.Display) {
                return;
            }
            var field = (QuoteTextField)Field;
            if (!string.IsNullOrEmpty(field.WebId) && !string.IsNullOrEmpty(field.ListId)) {
                using (var web = SPContext.Current.Site.OpenWeb(new Guid(field.WebId))) {
                    var list = web.Lists[new Guid(field.ListId)];
                    //var queryString = new CAMLQueryBuilder().AddContains("Title", "");
                    //var listTitle = (Literal)this.TemplateContainer.FindControl("ListTitle");
                    //listTitle.Text = list.Title;

                    var RestUrl = (Literal)this.TemplateContainer.FindControl("RestUrl");
                    RestUrl.Text = string.Format("{0}/_api/web/lists('{1}')/items?$select=Title",
                        web.ServerRelativeUrl != "/" ? web.ServerRelativeUrl : string.Empty, field.ListId);
                }
            }
        }
    }
}