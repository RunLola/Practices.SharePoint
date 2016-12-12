namespace Practices.SharePoint.WebParts {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;

    public partial class IssueTasksControl : UserControl, IIssueTasksView {
        
        readonly string ribbonTabId = "Practices.IssueTracking.Actions";

        IssueTasksPresenter presenter;

        #region IIssueTasksView

        public IEnumerable<SPContentType> TaskContentTypes {
            get;
            set;
        }

        public SPList List {
            get;
            set;
        }

        public IEnumerable<SPField> ViewFields {
            get;
            set;
        }

        public DataTable IssueTasks {
            get;
            set;
        }

        #endregion

        protected SPWeb Web {
            get {
                return SPContext.Current.Web;
            }
        }

        public IssueTasksControl() {
            presenter = new IssueTasksPresenter(this, Web);
        }

        protected override void OnPreRender(EventArgs e) {
            LoadAndActivateRibbonContextualTab(ribbonTabId);
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            presenter.LoadIssuesTasks();
            TasksGrid.DataSource = IssueTasks;
            TasksGrid.DataBind();
        }

        protected void GenerateBoundFields(SPGridView grid) {
            foreach (var field in ViewFields) {
                var boundField = new SPBoundField() {
                    HeaderText = field.Title,
                    DataField = field.InternalName,
                };
                boundField.HeaderStyle.CssClass = "ms-vh2";
                boundField.ItemStyle.CssClass = "ms-cellstyle ms-vb2";
                grid.Columns.Add(boundField);
            }
        }

        protected void LoadAndActivateRibbonContextualTab(string tabId) {
            SPRibbon current = SPRibbon.GetCurrent(this.Page);
            // Ensure ribbon exists.
            if (current != null) {
                current.Minimized = false;
                current.CommandUIVisible = true;
                if (!current.IsTabAvailable(tabId)) {
                    current.MakeTabAvailable(tabId);
                }
            }
        }
    }
}
