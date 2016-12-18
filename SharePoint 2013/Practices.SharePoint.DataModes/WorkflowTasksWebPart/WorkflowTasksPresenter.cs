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

        public void LoadTasks(uint startRow, uint maxRows) {
            view.RelatedItems = GetRelatedItems(startRow, maxRows);
        }

        protected DataTable GetRelatedItems(uint startRow, uint maxRows) {
            var table = GetTable();            
            var tasks = GetTasks(startRow, maxRows);
            foreach (DataRow task in tasks.Rows) {
                var row = table.NewRow();
                try {
                    using (SPWeb web = Web.Site.OpenWeb(new Guid(task["WebId"].ToString()))) {
                        var relatedItemsJson = task[SPBuiltInFieldName.RelatedItems.ToString()].ToString();
                        var relatedItem = new JavaScriptSerializer().Deserialize<List<RelatedItem>>(relatedItemsJson).AsEnumerable().FirstOrDefault();
                        //using (SPWeb web = Web.Site.OpenWeb(new Guid(relatedItem.WebId))) {
                        //}
                        var list = web.Lists[new Guid(relatedItem.ListId)];
                        var item = list.GetItemById(relatedItem.ItemId);
                        row["Title"] = item[view.ViewFields.FirstOrDefault().InternalName];
                        row["Identity"] = new JavaScriptSerializer().Serialize(relatedItem);
                        var url = string.Format("{0}listform.aspx?ListId={1}&PageType=6&ID={2}&Source={3}",
                            SPUtility.GetWebLayoutsFolder(web), task["ListId"], task["ID"], HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery));
                        row["NavigateUrl"] = url;
                        foreach (var field in view.ViewFields.Skip(1)) {
                            if (item.Fields.ContainsField(field.Title)) {
                                row[field.Id.ToString()] = item[field.Id];
                            }
                        }
                        table.Rows.Add(row);
                    }
                } catch (Exception ex) { 
                    
                }                
            }
            return table;
        }
        
        protected DataTable GetTasks(uint startRow, uint maxRows) {
            var repository = new WorkflowTaskRepository();
            var queryString = new CAMLQueryBuilder()
                .AddCurrentUser(SPBuiltInFieldName.AssignedTo)
                .OrCurrentUserGroups(SPBuiltInFieldName.AssignedTo)
                .AddNotEqual(SPBuiltInFieldName.TaskStatus, taskStatus)                
                .AddIsNotNull(SPBuiltInFieldName.RelatedItems)
                .AddIn(SPBuiltInFieldName.ContentType, view.TaskContentTypes.Select(c => c.Name)).Build();
            return repository.Get(queryString, startRow, maxRows);
        }

        public DataTable GetTable() {
            var table = new DataTable();
            table.Columns.Add("Title");
            table.Columns.Add("Identity");
            table.Columns.Add("NavigateUrl");
            foreach (var field in view.ViewFields.Skip(1)) {
                table.Columns.Add(field.Id.ToString());
            }
            return table;
        }
    }
}
