namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Linq;

    public partial class IssueTrackingControl : UserControl, IIssueTrackingView {

        readonly string ribbonTabId = "Practices.IssueTracking.Actions";

        IssueTrackingPresenter presenter;

        public string ContentTypeName;
        public IEnumerable<string> ViewFieldNames;

        #region IIssueTrackingView

        private SPContentType contentType;
        public SPContentType ContentType {
            get {
                if (contentType == null) {
                    contentType = Web.ContentTypes[ContentTypeName];
                }
                return contentType;
            }
        }

        private List<SPField> viewFields = new List<SPField>();
        public IList<SPField> ViewFields {
            get {
                if (!viewFields.Any()) {
                    if (ContentType != null) {
                        foreach (var fieldName in ViewFieldNames) {
                            if (ContentType.Fields.ContainsField(fieldName)) {
                                viewFields.Add(ContentType.Fields[fieldName]);
                            }
                        }
                    }
                }
                return viewFields;
            }
        }

        public DataTable Items {
            get;
            set;
        }

        #endregion

        protected SPWeb Web {
            get {
                return SPContext.Current.Web;
            }
        }

        public IssueTrackingControl() {
            presenter = new IssueTrackingPresenter(this, Web);
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (ContentType != null && ViewFields.Any()) {
                GenerateBoundFields(GridView);
                presenter.loadData(0, 100);
                if (Items != null && Items.Rows.Count > 0) {
                    GridView.DataSource = Items;
                    GridView.DataBind();
                }
            }
        }

        protected void LoadAndActivateRibbonContextualTab(string tabId) {
            SPRibbon current = SPRibbon.GetCurrent(this.Page);
            if (current != null) {
                current.Minimized = false;
                current.CommandUIVisible = true;
                if (!current.IsTabAvailable(tabId)) {
                    current.MakeTabAvailable(tabId);
                }
            }
        }

        protected void GenerateBoundFields(SPGridView grid) {
            var titleField = grid.Columns[1] as TemplateField;
            titleField.HeaderText = ViewFields.FirstOrDefault().Title;
            foreach (var field in ViewFields.Skip(1)) {
                var boundField = new SPBoundField() {
                    HeaderText = field.Title,
                    DataField = field.InternalName,
                };
                boundField.HeaderStyle.CssClass = "ms-vh2";
                boundField.ItemStyle.CssClass = "ms-cellstyle ms-vb2";
                grid.Columns.Add(boundField);
            }
        }
    }
}
