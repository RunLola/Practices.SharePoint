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
        IssueTrackingPresenter presenter;

        #region IIssueTrackingView

        public string ContentTypeName;
        public SPContentType ContentType {
            get {
                return Web.ContentTypes[ContentTypeName];
            }
        }

        public IEnumerable<string> ViewFieldNames;
        public IEnumerable<SPField> ViewFields {
            get {
                foreach (var fieldName in ViewFieldNames) {
                    if (List.Fields.ContainsField(fieldName)) {
                        yield return List.Fields[fieldName];
                    }
                }
            }
        }

        public DataTable Items {
            get;
            set;
        }

        public string ListTitle;
        public SPList List {
            get {
                var list = Web.Lists.TryGetList(ListTitle);
                return list;
            }
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

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            base.OnLoad(e);
            presenter.LoadIssues();
            GenerateBoundFields(GridView);
            GridView.DataSource = Items;
            GridView.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e) {
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
