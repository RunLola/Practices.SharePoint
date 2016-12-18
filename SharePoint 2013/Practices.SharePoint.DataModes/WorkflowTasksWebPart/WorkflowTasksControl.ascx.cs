namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Collections;
    using System.Linq;

    public partial class WorkflowTasksControl : UserControl, IWorkflowTasksView {

        WorkflowTasksPresenter presenter;
        readonly SPContentTypeId WorkflowTask2013 = new SPContentTypeId("0x0108003365C4474CAE8C42BCE396314E88E51F");

        public IEnumerable<string> TaskContentTypeNames = new List<string>();
        public string ContentTypeName;
        public IEnumerable<string> ViewFieldNames= new List<string>();

        #region IWorkflowTasksView

        private List<SPContentType> taskContentTypes= new List<SPContentType>();
        public IList<SPContentType> TaskContentTypes {
            get {
                if (!taskContentTypes.Any()) {
                    foreach (var contentTypeName in TaskContentTypeNames) {
                        var contentType = Web.ContentTypes[contentTypeName];
                        if (contentType != null && (contentType.Parent.Id == WorkflowTask2013) || contentType.Id == WorkflowTask2013) {
                            taskContentTypes.Add(contentType);
                        }
                    }
                }
                return taskContentTypes;
            }
        }

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
                if (!viewFields.Any() && ContentType != null) {
                    foreach (var fieldName in ViewFieldNames) {
                        if (ContentType.Fields.ContainsField(fieldName)) {
                            viewFields.Add(ContentType.Fields[fieldName]);
                        }
                    }
                }
                return viewFields;
            }
        }

        public DataTable RelatedItems {
            get;
            set;
        }
        
        #endregion

        protected SPWeb Web {
            get {
                return SPContext.Current.Web;
            }
        }

        public WorkflowTasksControl() {
            presenter = new WorkflowTasksPresenter(this, Web);
        }

        protected override void OnPreRender(EventArgs e) {
            //LoadAndActivateRibbonContextualTab(ribbonTabId);
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && ViewFields.Any()) {
                GenerateBoundFields(GridView);
                if (TaskContentTypes.Any()) {
                    presenter.LoadTasks(0, 30);
                    GridView.DataSource = RelatedItems;
                    GridView.DataBind();
                } else {
                    GridView.DataSource = new DataTable();
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
                    DataField = field.Id.ToString(),
                };
                boundField.HeaderStyle.CssClass = "ms-vh2";
                boundField.ItemStyle.CssClass = "ms-cellstyle ms-vb2";
                grid.Columns.Add(boundField);
            }
        }
    }
}
