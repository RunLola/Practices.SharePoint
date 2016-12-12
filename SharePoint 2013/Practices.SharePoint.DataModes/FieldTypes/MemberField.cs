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

    public class MemberField : SPFieldUser {
        public override BaseFieldControl FieldRenderingControl {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get {
                BaseFieldControl fieldControl = new MemberFieldControl() {
                    FieldName = InternalName
                };
                return fieldControl;
            }
        }

        public MemberField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName) {
            Initialize();
        }

        public MemberField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName) {
            Initialize();
        }

        void Initialize() {
        }

        public override void OnAdded(SPAddFieldOptions op) {
            base.OnAdded(op);
            this.Update();
        }

        public override void Update() {
            base.Update();
            CleanCustomProperty();
        }

        public void UpdateCustomProperty(string key, string value) {
        }

        public void CleanCustomProperty() {
        }
    }
}
