namespace Practices.SharePoint.WebParts {
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System.Linq;
    using System.Reflection;

    [ToolboxItemAttribute(false)]
    public class SiteContentByQueryWebPart : WebPart {

        #region Serialize Properties

        [Category("CategoryConfigureQuery"),
           DefaultValue(""),
           Personalizable(PersonalizationScope.Shared),
           WebBrowsable(true)]
        public string ListName {
            get;
            set;
        }

        [Category("CategoryConfigureQuery"),
           DefaultValue(""),
           Personalizable(PersonalizationScope.Shared),
           WebBrowsable(true)]
        public string ContentTypeName {
            get;
            set;
        }

        [Category("CategoryConfigureQuery"),
            DefaultValue(""),
            Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true)]
        public string ViewFieldNames {
            get;
            set;
        }

        [Category("CategoryConfigureQuery"),
            DefaultValue(""),
            Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true)]
        public string ListTemplateName {
            get;
            set;
        }

        [Category("CategoryConfigureQuery"),
            DefaultValue(""),
            Personalizable(PersonalizationScope.Shared),
            WebDescription("List, WebAndBelow, SiteCollection"),
            WebBrowsable(true)]
        public string QueryScope {
            get;
            set;
        }

        #endregion
        
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WebParts/SiteContentByQueryControl.ascx";

        protected override void CreateChildControls() {
            var control = (SiteContentByQueryControl)Page.LoadControl(_ascxPath);
            control.ContentTypeName = ContentTypeName;
            control.ViewFieldNames = ViewFieldNames;
            control.ListTemplateName = ListTemplateName;
            control.QueryScope = QueryScope;
            Controls.Add(control);
        }
    }
}