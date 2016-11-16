namespace Practices.SharePoint.ApplicationPages {
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.UI.WebControls.WebParts;
    using Utilities;

    public partial class ListFabricSettingsPage : LayoutsPageBase {
        protected Guid ListId {
            get {
                var listId = Guid.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["List"]) && Guid.TryParse(Request.QueryString["List"], out listId)) {
                    return listId;
                } else {
                    throw new ArgumentOutOfRangeException("List");
                }
            }
        }

        protected SPList List {
            get {
                if (SPContext.Current != null) {
                    return SPContext.Current.Web.Lists[ListId];
                } else {
                    throw new ArgumentNullException("SPContext.Current");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            List.UpateFormTempalte(PAGETYPE.PAGE_DISPLAYFORM, "FabricListForm");
            List.UpateFormTempalte(PAGETYPE.PAGE_NEWFORM, "FabricListForm");
            List.UpateFormTempalte(PAGETYPE.PAGE_EDITFORM, "FabricListForm");
        }
    }
}
