namespace Practices.SharePoint.WebParts {
    using System;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;

    [ToolboxItemAttribute(false)]
    public class RegisterPageComponentWebPart : WebPart {
        [Category("Configure"),
            Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true)]
        public string RibbonTabId {
            get;
            set;
        }

        [Category("Configure"),
            Personalizable(PersonalizationScope.Shared),
            WebBrowsable(true)]
        public string InitPageComponentScript {
            get;
            set;
        }

        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);
            InitializeRibbon();
        }

        protected override void CreateChildControls() {
            if (!Page.IsPostBack && !string.IsNullOrEmpty(InitPageComponentScript)) {

                var sp = new ScriptLink() {
                    Language = "javascript",
                    Name = "SP.js",
                    OnDemand = true,
                    Localizable = false,
                };
                var spribbon = new ScriptLink() {
                    Language = "javascript",
                    Name = "SP.Ribbon.js",
                    OnDemand = true,
                    Localizable = false,
                };
                var initialize = new ScriptLink() {
                    Language = "javascript",
                    Name = InitPageComponentScript,
                    LoadAfterUI = true,
                    Localizable = false,
                };
                Controls.Add(sp);
                Controls.Add(spribbon);
                Controls.Add(initialize);
            }
        }

        protected void InitializeRibbon() {
            SPRibbon current = SPRibbon.GetCurrent(this.Page);
            if (current != null &&
                !string.IsNullOrEmpty(RibbonTabId) &&
                !string.IsNullOrEmpty(InitPageComponentScript)) {
                current.Minimized = true;
                current.CommandUIVisible = true;
                if (!current.IsTabAvailable(RibbonTabId)) {
                    current.MakeTabAvailable(RibbonTabId);
                }
                //current.InitialTabId = RibbonTabId;
            }
            //var ribbonScriptManager = new SPRibbonScriptManager();
            //var registerInitializeFunction = typeof(SPRibbonScriptManager).GetMethod("RegisterInitializeFunction", BindingFlags.NonPublic | BindingFlags.Instance);
            //registerInitializeFunction.Invoke(ribbonScriptManager, new object[] { 
            //    this.Page, 
            //    "InitPageComponent", 
            //    "/_layouts/15/Scripts/IssueTracking.Ribbon.Actions.js", 
            //    false, 
            //    "Practices.IssueTracking.ActionsPageComponent.load()" });
        }
    }
}
