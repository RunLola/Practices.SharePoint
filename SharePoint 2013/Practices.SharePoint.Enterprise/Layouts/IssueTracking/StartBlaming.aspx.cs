namespace Practices.SharePoint.ApplicationPages {
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using Practices.SharePoint.Repositories;

    public partial class StartBlamingPage : LayoutsPageBase {
        public string WebId {
            get {
                if (!string.IsNullOrEmpty(Request.QueryString["WebId"])) {
                    return Request.QueryString["WebId"];
                } else {
                    throw new ArgumentNullException("WebId");
                }
            }
        }

        public string ListId {
            get {
                if (!string.IsNullOrEmpty(Request.QueryString["ListId"])) {
                    return Request.QueryString["ListId"];
                } else {
                    throw new ArgumentNullException("ListId");
                }
            }
        }

        public string ItemId {
            get {
                if (!string.IsNullOrEmpty(Request.QueryString["ItemId"])) {
                    return Request.QueryString["ItemId"];
                } else {
                    throw new ArgumentNullException("ItemId");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {

            }
        }

        protected void BtnSave_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.TrailingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.Begin();
                var issue = GetIssue();
                issue[SPBuiltInFieldName.Title] = "Blaming";
                issue.Update();
                var redirectURL = string.Concat(SPContext.Current.Web.Url, HttpContext.Current.Request.Url.AbsolutePath);                
                longOperation.End(redirectURL);
            }
        }

        protected SPListItem GetIssue() {
            using (SPWeb web = Web.Site.OpenWeb(new Guid(WebId))) {
                var list = web.Lists[new Guid(ListId)];
                var item = list.GetItemById(int.Parse(ItemId));
                return item;
            }
        }

        protected SPListItem CreateBlaming() {
            using (SPWeb web = Web.Site.OpenWeb(new Guid(WebId))) {
                var list = web.Lists[""];
                var item = list.Items.Add();
                item[SPBuiltInFieldId.RelatedItems] = "";
                item.Update();
                return item;
            }
        }
    }
}
