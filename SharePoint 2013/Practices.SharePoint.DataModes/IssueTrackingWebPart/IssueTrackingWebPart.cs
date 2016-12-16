namespace Practices.SharePoint.WebParts {
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Collections.Generic;

    [ToolboxItemAttribute(false)]
    public class IssueTrackingWebPart : WebPart {

        [WebBrowsable(true),
         WebDisplayName("隐患流程任务类型"),
         WebDescription("请用分号隔开"),
         Personalizable(PersonalizationScope.Shared),
         Category("配置选项")]
        public string TaskContentTypeNames { get; set; }

        [WebBrowsable(true),
         WebDisplayName("列表名称"),
         WebDescription("请用分号隔开"),
         Personalizable(PersonalizationScope.Shared),
         Category("配置选项")]
        public string ListName { get; set; }

        [WebBrowsable(true),
         WebDisplayName("视图字段"),
         WebDescription("请用分号隔开"),
         Personalizable(PersonalizationScope.Shared),
         Category("配置选项")]
        public string ViewFieldNames { get; set; }

        protected SPWeb Web {
            get {
                return SPContext.Current.Web;
            }
        }

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WebParts/IssueTrackingControl.ascx";

        protected override void CreateChildControls() {
            SPContentType ContentType = null;
            SPList List = null;
            IEnumerable<SPField> ViewFields = null;

            var control = (IssueTrackingControl)Page.LoadControl(_ascxPath);

            Controls.Add(control);
        }
    }
}
