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
            var repository = new IssueTrackingRepository(view.ViewFields.Select(f => f.InternalName));
            var queryString = new CAMLQueryBuilder().Build();
            var table = repository.Get(queryString, startRow, maxRows);
            table.Columns.Add("Identity");
            table.Columns.Add("NavigateUrl");
            foreach (DataRow row in table.Rows) {
                using (SPWeb web = Web.Site.OpenWeb(new Guid(row["WebId"].ToString()))) {
                    var url = string.Format("{0}listform.aspx?ListId={1}&PageType=6&ID={2}&Source={3}",
                        SPUtility.GetWebLayoutsFolder(web), row["ListId"], row["ID"], HttpUtility.UrlDecode(HttpContext.Current.Request.Url.PathAndQuery));
                    row["NavigateUrl"] = url;
                    row["Identity"] = new JavaScriptSerializer().Serialize(
                        new RelatedItem() {
                            WebId = row["WebId"].ToString(),
                            ListId = row["ListId"].ToString(),
                            ItemId = int.Parse(row["ID"].ToString())
                        });
                    row["Title"] = row[view.ViewFields.FirstOrDefault().InternalName];
                }
            }
            view.Items = table;
        }
    }
}