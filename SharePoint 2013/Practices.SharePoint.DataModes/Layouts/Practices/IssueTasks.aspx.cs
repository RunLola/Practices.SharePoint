namespace Practices.SharePoint.ApplicationPages {
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Collections.Generic;
    using System.Reflection;
    using Repositories;
    using System.Linq;
    using Utilities;
    using Microsoft.SharePoint.Utilities;
    using System.Web;
    using System.Data;
    using System.Web.Script.Serialization;
    using System.Collections;
    using System.Web.UI.WebControls;

    public partial class IssueTasksPage : LayoutsPageBase, IIssueTasksView {
        readonly string taskStatus = "Completed";
        readonly string ribbonTabId = "Practices.IssueTracking.Actions";
        
        #region IMyTasksView

        protected void GetBoundFields(SPGridView grid) {
            var fields = GetViewFields();
            foreach (var field in fields) {
                var boundField = new SPBoundField() {
                    HeaderText = field,
                    DataField = field,
                };
                boundField.HeaderStyle.CssClass = "ms-vh2";
                boundField.ItemStyle.CssClass = "ms-cellstyle ms-vb2";
                grid.Columns.Add(boundField);
            }
        }

        public DataTable GetRelatedItems(uint startRow, uint maxRows) {
            var table = new DataTable();
            var fields = GetViewFields();
            table.Columns.Add("NavigateUrl");
            foreach (var field in fields) {
                table.Columns.Add(field);
            }
            var tasks = GetTasks(startRow, maxRows);
            foreach (var task in tasks) {
                var row = table.NewRow();
                using (SPWeb web = Web.Site.OpenWeb(new Guid(task["WebId"].ToString()))) {
                    var url = string.Format("{0}listform.aspx?ListId={1}&PageType=6&ID={2}&Source={3}",
                        SPUtility.GetWebLayoutsFolder(web), task["ListId"], task["ID"], HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery));
                    row["NavigateUrl"] = url;

                    var relatedItemsJson = task[BuiltInFieldName.RelatedItems].ToString();
                    var relatedItem = new JavaScriptSerializer().Deserialize<List<RelatedItem>>(relatedItemsJson).AsEnumerable().FirstOrDefault();
                    //using (SPWeb web = Web.Site.OpenWeb(new Guid(relatedItem.WebId))) {
                    //}
                    var list = web.Lists[new Guid(relatedItem.ListId)];
                    var item = list.GetItemById(relatedItem.ItemId);
                    foreach (var field in fields) {
                        if (item.Fields.ContainsField(field)) {
                            row[field] = item[field];
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        protected IEnumerable<DataRow> GetTasks(uint startRow, uint maxRows) {
            var repository = new WorkflowTaskRepository();
            var queryString = new CAMLQueryBuilder()
                .AddNotEqual(BuiltInFieldName.TaskStatus, taskStatus)
                .AddCurrentUser(BuiltInFieldName.AssignedTo)
                .OrCurrentUserGroups(BuiltInFieldName.AssignedTo)
                .AddIsNotNull(BuiltInFieldName.RelatedItems)
                .AddBeginsWith(BuiltInFieldName.ContentTypeId, BuiltInContentTypeId.WorkflowTask2013.ToString())
                .AddIn(BuiltInFieldName.ContentType, new List<string>() { "Workflow Task (SharePoint 2013)" }).Build();
            var tasks = repository.Get(queryString, startRow, maxRows);
            return tasks;
        }

        protected IEnumerable<string> GetViewFields() {
            List<string> fields = new List<string>();
            fields.Add("Title");
            return fields;
        }

        #endregion        

        protected override void OnPreRender(EventArgs e) {
            LoadAndActivateRibbonContextualTab();
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            var items = GetRelatedItems(0, 100);
            GetBoundFields(DataGrid);
            DataGrid.DataSource = items;
            DataGrid.DataBind();
        }

        protected void LoadAndActivateRibbonContextualTab() {
            SPRibbon current = SPRibbon.GetCurrent(this.Page);
            // Ensure ribbon exists.
            if (current != null) {
                current.Minimized = false;
                current.CommandUIVisible = true;
                var tabId = ribbonTabId;
                if (!current.IsTabAvailable(tabId)) {
                    current.MakeTabAvailable(tabId);
                }
            }
        }
    }
}
