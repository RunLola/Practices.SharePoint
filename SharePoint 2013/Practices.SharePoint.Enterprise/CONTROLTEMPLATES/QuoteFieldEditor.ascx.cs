namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using FieldTypes;

    public partial class QuoteFieldEditor : UserControl, IFieldEditor {
        public bool DisplayAsNewSection {
            get {
                return true;
            }
        }

        private string webId;
        protected string WebId {
            get {
                if (!string.IsNullOrEmpty(WebListIds.Value)) {
                    webId = WebListIds.Value.Split('?')[1].Split(':')[1];
                }
                return webId;
            }
            set {
                webId = value;
            }
        }

        private string listId;
        protected string ListId {
            get {
                if (!string.IsNullOrEmpty(WebListIds.Value)) {
                    listId = WebListIds.Value.Split('?')[0].Split(':')[1];
                }
                return listId;
            }
            set {
                listId = value;
            }
        }

        public void InitializeWithField(SPField field) {
            var quoteTextField = field as QuoteTextField;
            var quoteNoteField = field as QuoteNoteField;
            if (!Page.IsPostBack) {
                if (quoteTextField != null) {
                    WebId = quoteTextField.WebId;
                    ListId = quoteTextField.ListId;
                } else if (quoteNoteField != null) {
                    WebId = quoteNoteField.WebId;
                    ListId = quoteNoteField.ListId;
                }
                if (!string.IsNullOrEmpty(WebId) && !string.IsNullOrEmpty(ListId)) {
                    WebListIds.Value = string.Format("SPList:{0}?SPWeb:{1}", ListId, WebId);
                    using (var web = SPContext.Current.Site.OpenWeb(new Guid(WebId))) {
                        var list = web.Lists[new Guid(ListId)];
                        ListUrl.Text = list.RootFolder.ServerRelativeUrl.ToString();
                    }
                }
            }
        }

        public void OnSaveChange(SPField field, bool isNewField) {
            var quoteTextField = field as QuoteTextField;
            var quoteNoteField = field as QuoteNoteField;
            if (quoteTextField != null) {
                if (isNewField) {
                    quoteTextField.UpdateCustomProperty("WebId", WebId);
                    quoteTextField.UpdateCustomProperty("ListId", ListId);
                } else {
                    quoteTextField.WebId = WebId;
                    quoteTextField.ListId = ListId;
                }
            } else if (quoteNoteField != null) {
                if (isNewField) {
                    quoteNoteField.UpdateCustomProperty("WebId", WebId);
                    quoteNoteField.UpdateCustomProperty("ListId", ListId);
                } else {
                    quoteNoteField.WebId = WebId;
                    quoteNoteField.ListId = ListId;
                }
            }            
        }
    }
}
