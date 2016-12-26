namespace Practices.SharePoint.FieldTypes {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MemberFieldControl : BaseFieldControl {
        protected override string DefaultTemplateName {
            get {
                return "MemberField";
            }
        }

        public override string CssClass {
            get {
                return base.CssClass;
            }
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            if (base.ControlMode == SPControlMode.Display) {
                return;
            }
            var peopleEditor = (PeopleEditor)this.TemplateContainer.FindControl("UserField");
            peopleEditor.PickerDialogType = typeof(MemberPickerDialog);
            peopleEditor.TabIndex = this.TabIndex;
            peopleEditor.CssClass = this.CssClass;            
            peopleEditor.Rows = 1;
            var field = (MemberField)this.Field;
            peopleEditor.MultiSelect = field.AllowMultipleValues;
            peopleEditor.AllowEmpty = !field.Required;
            //if (string.IsNullOrEmpty(this.Field.Description) && field.AllowMultipleValues) {
            //    peopleEditor.InputDescription = SPResource.GetString("MultiUserFieldDefaultDescription", new object[0]);
            //} else {
            //    peopleEditor.InputDescription = this.Field.Description;
            //}
        }
    }
}
