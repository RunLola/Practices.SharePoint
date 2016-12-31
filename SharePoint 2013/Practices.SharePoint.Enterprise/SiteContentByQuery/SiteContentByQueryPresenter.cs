namespace Practices.SharePoint.WebParts {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Practices.SharePoint.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Script.Serialization;
    using Utilities;
    
    class SiteContentByQueryPresenter {
        ISiteContentByQueryView view;

        protected SPWeb Web {
            get;
            set;
        }

        public SiteContentByQueryPresenter(ISiteContentByQueryView view) :
            this(view, SPContext.Current != null ? SPContext.Current.Web : null) {
        }

        public SiteContentByQueryPresenter(ISiteContentByQueryView view, SPWeb clientWeb) {
            Validation.ArgumentNotNull(view, "ISiteContentByQueryView");
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            this.view = view;
            this.Web = clientWeb;
        }

        public void LoadData(int startRow, int maxRows) {
            var queryString = BuildQueryString();
            var table = Get(queryString, startRow, maxRows);
            table.Columns.Add("Title");
            table.Columns.Add("Identity");
            table.Columns.Add("NavigateUrl");
            foreach (DataRow row in table.Rows) {
                using (SPWeb web = Web.Site.OpenWeb(new Guid(row["WebId"].ToString()))) {
                    row["Title"] = row[view.ViewFieldIds.FirstOrDefault().ToString()];
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
            view.DataSource = table;
        }

        public string BuildQueryString() {
            var queryBuilder = new CAMLQueryBuilder();
            switch (view.QueryScope) {
                case "ByBlaming":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.AssignedTo)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "CanBlaiming":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.Author)
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "保存")
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "关闭")
                        .AddEqual("_x662f__x5426__x8ffd__x8d23_", false)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "HasBlaming":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.Author)
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "保存")
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "关闭")
                        .AddEqual("_x662f__x5426__x8ffd__x8d23_", true)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "ByForfeit":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.AssignedTo)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "CanForfeit":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.Author)
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "保存")
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "关闭")
                        .AddEqual("_x662f__x5426__x7f5a__x6b3e_", false)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "HasForfeit":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.Author)
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "保存")
                        .AddNotEqual("_x9690__x60a3__x72b6__x6001_", "关闭")
                        .AddEqual("_x662f__x5426__x7f5a__x6b3e_", true)
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                case "CanClosed":
                    queryBuilder = queryBuilder
                        .AddCurrentUser(SPBuiltInFieldName.Author)
                        .AddEqual("_x9690__x60a3__x72b6__x6001_", "销号")
                        .AddBeginsWith(SPBuiltInFieldName.ContentTypeId, view.ContentTypeBeginsWithId);
                    break;
                default:
                    break;
            }
            return queryBuilder.Build();
        }

        public DataTable Get(string queryString, int startRow, int maxRows) {
            var query = new SPSiteDataQuery() {
                Lists = view.ListScope,
                Webs = view.WebScope,
                ViewFields = string.Join("", view.ViewFieldIds.Select(fieldId => string.Format("<FieldRef ID='{0}' />", fieldId))),
                Query = queryString,
                RowLimit = (uint)(startRow + maxRows)
            };
            return Get(query, startRow, maxRows);
        }

        protected DataTable Get(SPSiteDataQuery query, int startRow, int maxRows) {
            var data = Web.GetSiteData(query);
            if (data.Rows.Count > 0) {
                return data.Select().Skip(startRow).Take(maxRows).CopyToDataTable();
            } else {
                return data;
            }
        }
    }
}