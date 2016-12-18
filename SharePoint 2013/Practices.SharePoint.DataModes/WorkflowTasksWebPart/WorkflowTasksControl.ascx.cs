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

        #region IWorkflowTasksView

        public IEnumerable<string> TaskContentTypeNames;
        public IEnumerable<SPContentType> TaskContentTypes {
            get {
                foreach (var contentTypeName in TaskContentTypeNames) {
                    var contentType = Web.ContentTypes[contentTypeName];
                    if (contentType != null && contentType.Parent.Id == SPBuiltInContentTypeId.WorkflowTask) {
                        yield return contentType;
                    }
                }
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

        public DataTable RelatedItems {
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

        public WorkflowTasksControl() {
            presenter = new WorkflowTasksPresenter(this, Web);
        }

        protected override void OnPreRender(EventArgs e) {
            //LoadAndActivateRibbonContextualTab(ribbonTabId);
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);            
            presenter.LoadTasks();
            GenerateBoundFields(GridView);
            GridView.DataSource = RelatedItems;
            GridView.DataBind();
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
