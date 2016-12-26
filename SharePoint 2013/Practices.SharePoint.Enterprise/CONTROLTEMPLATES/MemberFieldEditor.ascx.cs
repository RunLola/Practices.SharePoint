namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public partial class MemberFieldEditor : UserControl, IFieldEditor {
        public bool DisplayAsNewSection {
            get {
                return true;
            }
        }


        public void InitializeWithField(SPField field) {
            
        }

        public void OnSaveChange(SPField field, bool isNewField) {
                  
        }
    }
}
