namespace Practices.SharePoint.WebParts {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Practices.SharePoint.Models;
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

    class IssueTrackingPresenter {
        IIssueTrackingView view;

        SPWeb web;
        protected SPWeb Web {
            get {
                return web;
            }
        }

        public IssueTrackingPresenter(IIssueTrackingView view, SPWeb web) {
            this.view = view;
            this.web = web;
        }
        
        public void loadData(uint startRow, uint maxRows) {
            var repository = new IssueTrackingRepository(view.ViewFields.Select(f => f.Id));
            var queryBuilder = new CAMLQueryBuilder();
            if (!string.IsNullOrEmpty(view.Status)) {
                queryBuilder.AddEqual(SPBuiltInFieldName.IssueStatus, view.Status);
            }
            var queryString = queryBuilder.Build();
            var table = repository.Get(queryString, startRow, maxRows);
            table.Columns.Add("Title");
            table.Columns.Add("Identity");
            table.Columns.Add("NavigateUrl");
            foreach (DataRow row in table.Rows) {
                using (SPWeb web = Web.Site.OpenWeb(new Guid(row["WebId"].ToString()))) {
                    row["Title"] = row[view.ViewFields.FirstOrDefault().Id.ToString()];
                    row["Identity"] = new JavaScriptSerializer().Serialize(
                        new RelatedItem() {
                            WebId = row["WebId"].ToString(),
                            ListId = row["ListId"].ToString(),
                            ItemId = int.Parse(row["ID"].ToString())
                        });
                    var url = string.Format("{0}listform.aspx?ListId={1}&PageType=6&ID={2}&Source={3}",
                        SPUtility.GetWebLayoutsFolder(web), row["ListId"], row["ID"], HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery));
                    row["NavigateUrl"] = url;
                }
            }
            view.Items = table;
        }
    }
}