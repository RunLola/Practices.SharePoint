namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Renders each field in a list item, with some possible exceptions. 
    /// </summary>
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true),
     SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true),
     AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
     AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
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
            if (this.ControlTemplate == null) 
                throw new ArgumentException("Could not find FabricListFieldIterator control template.");
            
            for (int i = 0; i < base.Fields.Count; i++) {
                SPField field = base.Fields[i];
                if (!this.IsFieldExcluded(field)) {
                    TemplateContainer templateContainer = new TemplateContainer();
                    this.Controls.Add(templateContainer);
                    //templateContainer.ControlMode = base.ControlMode;
                    PropertyInfo controlMode = typeof(TemplateContainer).GetProperty("ControlMode", BindingFlags.NonPublic | BindingFlags.Instance);
                    controlMode.SetValue(templateContainer, base.ControlMode);
                    //templateContainer.FieldName = field.InternalName;
                    PropertyInfo fieldName = typeof(TemplateContainer).GetProperty("FieldName", BindingFlags.NonPublic | BindingFlags.Instance);
                    fieldName.SetValue(templateContainer, field.InternalName);
                    ControlTemplate.InstantiateIn(templateContainer);

                    var gridCol = templateContainer.FindControl("GridCol") as HtmlGenericControl;
                    if (gridCol == null)
                        throw new ArgumentException("Could not find GridCol in FabricListFieldIterator control template.");
                    gridCol.Attributes.Add("class", "col-md-12");
                }
            }
        }
    }
}
