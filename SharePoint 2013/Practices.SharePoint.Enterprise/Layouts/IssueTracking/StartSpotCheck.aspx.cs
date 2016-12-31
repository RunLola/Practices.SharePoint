namespace Practices.SharePoint.ApplicationPages {
    using System;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Web;
    using Practices.SharePoint.Repositories;
    using Models;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Microsoft.SharePoint.Utilities;
    using Utilities;

    public partial class StartSpotCheckPage : LayoutsPageBase {

        #region QueryString

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

        #endregion

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {

            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.TrailingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.Begin();
                var issue = GetIssue();
                issue["是否追责"] = true;
                issue.Update();
                var blaming = CreateBlaming();
                var redirectURL = string.Format("{0}listform.aspx?ListId={1}&PageType=4&ID={2}",
                        SPUtility.GetWebLayoutsFolder(blaming.ParentList.ParentWeb), blaming.ParentList.ID, blaming.ID);
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
                var list = web.Lists["隐患追责"];
                var item = list.Items.Add();
                item[SPBuiltInFieldId.AssignedTo] = new SPFieldUserValue(web, AssignedTo.GetFieldValues());
                item[SPBuiltInFieldId.RelatedItems] = 
                    new JavaScriptSerializer().Serialize(
                        new List<RelatedItem>() {
                            new RelatedItem() {
                            WebId = WebId,
                            ListId = ListId,
                            ItemId = int.Parse(ItemId)
                    }});
                item["原因分析"] = Reasons.Text.Trim();
                item["履责情况"] = Situations.Text.Trim();
                item["追责时间"] = BlameDate.SelectedDate;
                item["追责地点"] = BlameLocation.Text.Trim();
                item.Update();
                return item;
            }
        }
    }
}
