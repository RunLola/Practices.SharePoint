namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true),
        AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
        SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class FabricListFieldIterator : ListFieldIterator {
        protected override string DefaultTemplateName {
            get {
                return "FabricListFieldIterator";
            }
        }

        protected override bool IsFieldExcluded(SPField field) {
            return base.IsFieldExcluded(field);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void CreateChildControls() {
            this.Controls.Clear();
            if (this.ControlTemplate == null) {
                throw new System.ArgumentException("Could not find FabricListFieldIterator control template.");
            }
            for (int i = 0; i < base.Fields.Count; i++) {
                SPField field = base.Fields[i];
                if (!this.IsFieldExcluded(field)) {
                    TemplateContainer templateContainer = new TemplateContainer();
                    this.Controls.Add(templateContainer);
                    //templateContainer.ControlMode = base.ControlMode;
                    //templateContainer.FieldName = sPField.InternalName;
                    this.ControlTemplate.InstantiateIn(templateContainer);
                }
            }
        }
    }
}
