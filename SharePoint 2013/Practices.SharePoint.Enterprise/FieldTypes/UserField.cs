namespace Practices.SharePoint.FieldTypes {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;

    public class UserField : SPFieldUser {
        private static string webId;
        public string WebId {
            get;
            set;
        }

        private static string listId;
        public string ListId {
            get;
            set;
        }

        public override BaseFieldControl FieldRenderingControl {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get {
                BaseFieldControl fieldControl = new UserFieldControl() {
                    FieldName = InternalName
                };
                return fieldControl;
            }
        }

        public UserField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName) {
            Initialize();
        }

        public UserField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName) {
            Initialize();
        }

        void Initialize() {
            WebId = GetCustomProperty("WebId") == null ? null : GetCustomProperty("WebId").ToString();
            ListId = GetCustomProperty("ListId") == null ? null : GetCustomProperty("ListId").ToString();
        }

        public override void OnAdded(SPAddFieldOptions op) {
            base.OnAdded(op);
            this.Update();
        }

        public override void Update() {
            SetCustomProperty("WebId", string.IsNullOrEmpty(webId) ? WebId : webId);
            SetCustomProperty("ListId", string.IsNullOrEmpty(listId) ? ListId : listId);
            base.Update();
            CleanCustomProperty();
        }

        public void UpdateCustomProperty(string key, string value) {
            switch (key) {
                case "WebId":
                    webId = value;
                    break;
                case "ListId":
                    listId = value;
                    break;
                default:
                    break;
            }
        }

        public void CleanCustomProperty() {
            webId = null;
            listId = null;
        }
    }
}
