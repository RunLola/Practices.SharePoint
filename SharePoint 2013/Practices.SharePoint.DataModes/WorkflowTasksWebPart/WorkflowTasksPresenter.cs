namespace Practices.SharePoint.WebParts {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Script.Serialization;
    using Utilities;
    using Models;

    class WorkflowTasksPresenter {
                
        readonly string taskStatus = "Completed";
        IWorkflowTasksView view;

        SPWeb web;
        protected SPWeb Web {
            get {
                return web;
            }
        }

        public WorkflowTasksPresenter(IWorkflowTasksView view, SPWeb web) {
            this.view = view;
            this.web = web;
        }

        public void LoadTasks() {
            if (view.List != null && view.ViewFields.Count() > 0 && view.TaskContentTypes.Count() > 0) {
                view.RelatedItems = GetRelatedItems(0, 100);
            } else {
                view.RelatedItems = GetTable();
            }            
        }

        protected DataTable GetRelatedItems(uint startRow, uint maxRows) {
            var table = GetTable();            
            var tasks = GetTasks(startRow, maxRows);
            foreach (DataRow task in tasks.Rows) {
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
                    row["Identity"] = new JavaScriptSerializer().Serialize(relatedItem);
                    row["Title"] = item[view.ViewFields.FirstOrDefault().InternalName];
                    foreach (var field in view.ViewFields.Skip(1)) {
                        if (item.Fields.ContainsField(field.InternalName)) {
                            row[field.InternalName] = item[field.InternalName];
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        
        protected DataTable GetTasks(uint startRow, uint maxRows) {
            var repository = new WorkflowTaskRepository();
            var queryString = new CAMLQueryBuilder()
                .AddCurrentUser(BuiltInFieldName.AssignedTo)
                .OrCurrentUserGroups(BuiltInFieldName.AssignedTo)
                .AddNotEqual(BuiltInFieldName.TaskStatus, taskStatus)                
                .AddIsNotNull(BuiltInFieldName.RelatedItems)
                .AddBeginsWith(BuiltInFieldName.ContentTypeId, BuiltInContentTypeId.WorkflowTask2013.ToString())
                .AddIn(BuiltInFieldName.ContentType, view.TaskContentTypes.Select(c => c.Name)).Build();
            return repository.Get(queryString, startRow, maxRows);
        }

        public DataTable GetTable() {
            var table = new DataTable();
            table.Columns.Add("NavigateUrl");
            table.Columns.Add("Title");
            foreach (var field in view.ViewFields.Skip(1)) {
                table.Columns.Add(field.InternalName);
            }
            return table;
        }
    }
}
