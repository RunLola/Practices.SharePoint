namespace Practices.SharePoint.WebParts {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;

    public partial class SiteContentByQueryControl : UserControl, ISiteContentByQueryView {
        SiteContentByQueryPresenter presenter;

        #region Serialize Properties

        public string ListName {
            get;
            set;
        }

        public string ContentTypeName {
            get;
            set;
        }

        public string ViewFieldNames {
            get;
            set;
        }

        public string ListTemplateName {
            get;
            set;
        }

        public string QueryScope {
            get;
            set;
        }

        #endregion

        private SPContentType contentType;
        public SPContentType ContentType {
            get {
                if (contentType == null && !string.IsNullOrEmpty(ContentTypeName)) {
                    contentType = Web.Site.RootWeb.ContentTypes[ContentTypeName];
                }
                return contentType;
            }
        }

        private List<SPField> viewFields = new List<SPField>();
        protected IEnumerable<SPField> ViewFields {
            get {
                if (!viewFields.Any() && ContentType != null) {
                    var viewFieldNames = ViewFieldNames.Trim(';').Split(';');
                    foreach (var viewFieldName in viewFieldNames) {
                        if (ContentType.Fields.ContainsField(viewFieldName)) {
                            var field = ContentType.Fields[viewFieldName];
                            viewFields.Add(field);
                        }
                    }
                }
                return viewFields;
            }
        }

        #region ISiteContentByQueryView

        private string contentTypeBeginsWithId;
        public string ContentTypeBeginsWithId {
            get {
                if (string.IsNullOrEmpty(contentTypeBeginsWithId) && ContentType != null) {
                    contentTypeBeginsWithId = ContentType.Id.ToString();
                }
                return contentTypeBeginsWithId;
            }
        }

        private IList<string> viewFieldIds = new List<string>();
        public IList<string> ViewFieldIds {
            get {
                if (!viewFieldIds.Any() && ContentType != null && !string.IsNullOrEmpty(ViewFieldNames)) {
                    var stringBuilder = new StringBuilder();
                    var viewFieldNames = ViewFieldNames.Trim(';').Split(';');
                    foreach (var viewFieldName in viewFieldNames) {
                        if (ContentType.Fields.ContainsField(viewFieldName)) {
                            var field = ContentType.Fields[viewFieldName];
                            viewFieldIds.Add(field.Id.ToString());
                        }
                    }
                }
                return viewFieldIds;
            }
        }

        private string listScope;
        public string ListScope {
            get {
                if (string.IsNullOrEmpty(listScope) && !string.IsNullOrEmpty(ListTemplateName)) {
                    foreach (SPWeb web in Web.Site.AllWebs) {
                        var listTemplate = web.ListTemplates.Cast<SPListTemplate>().Where(t => t.Name == ListTemplateName).FirstOrDefault();
                        if (listTemplate != null) {
                            listScope = string.Format("<Lists ServerTemplate='{0}' BaseType='{1}' />", listTemplate.Type_Client, (int)listTemplate.BaseType);
                            break;
                        }
                    }
                }
                return listScope;
            }
        }

        private string webScope;
        public string WebScope {
            get {
                if (string.IsNullOrEmpty(webScope) && !string.IsNullOrEmpty(QueryScope)) {
                    webScope = string.Format("<Webs Scope='{0}' />", QueryScope);
                }
                return webScope;
            }
        }

        public DataTable DataSource {
            get;
            set;
        }

        #endregion

        protected SPWeb Web {
            get {
                return SPContext.Current.Web;
            }
        }

        public SiteContentByQueryControl() {
            presenter = new SiteContentByQueryPresenter(this, Web);
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && ViewFields.Any()) {
                GenerateBoundFields(GridView);
                presenter.LoadData(0, 30);
                GridView.DataSource = DataSource;
                GridView.DataBind();
            }
        }

        protected void GenerateBoundFields(SPGridView grid) {
            ((TemplateField)grid.Columns[1]).HeaderText = ViewFields.FirstOrDefault().Title;
            foreach (var field in ViewFields.Skip(1)) {
                var boundField = new SPBoundField() {
                    HeaderText = field.Title,
                    DataField = field.Id.ToString(),
                };
                boundField.HeaderStyle.CssClass = "ms-vh2";
                boundField.ItemStyle.CssClass = "ms-cellstyle ms-vb2";
                grid.Columns.Add(boundField);
            }
        }
    }
}
