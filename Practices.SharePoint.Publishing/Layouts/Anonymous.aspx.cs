namespace Practices.SharePoint.Publishing {
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;

    public partial class AnonymousPage : UnsecuredLayoutsPageBase {
        protected override bool AllowAnonymousAccess {
            get {
                return true;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {

        }
    }
}