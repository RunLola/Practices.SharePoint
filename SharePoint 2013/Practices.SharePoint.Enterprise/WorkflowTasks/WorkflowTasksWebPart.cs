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
    using System.Linq;

    [ToolboxItemAttribute(false)]
    public class WorkflowTasksWebPart : WebPart {
        [WebBrowsable(true),
         WebDisplayName("工作流任务内容类型"),
         WebDescription("请用分号隔开"),
         Personalizable(PersonalizationScope.Shared),
         Category("配置选项")]
        public string TaskContentTypeNames { get; set; }

        [WebBrowsable(true),
         WebDisplayName("内容类型"),
         WebDescription(""),
         Personalizable(PersonalizationScope.Shared),
         Category("配置选项")]
        public string ContentTypeName { get; set; }

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
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WebParts/WorkflowTasksControl.ascx";

        protected override void CreateChildControls() {
            if (!string.IsNullOrEmpty(TaskContentTypeNames) &&
                !string.IsNullOrEmpty(ContentTypeName) &&
                !string.IsNullOrEmpty(ViewFieldNames)) {
                var control = Page.LoadControl(_ascxPath) as WorkflowTasksControl;
                control.TaskContentTypeNames = TaskContentTypeNames.Trim(';').Split(';').AsEnumerable();
                control.ContentTypeName = ContentTypeName;
                control.ViewFieldNames = ViewFieldNames.Trim(';').Split(';').AsEnumerable();
                Controls.Add(control);
            }            
        }
    }
}
