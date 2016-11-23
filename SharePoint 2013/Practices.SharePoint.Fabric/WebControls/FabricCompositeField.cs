namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Represents a field control along with its label and description with Office UI Fabric.
    /// </summary>
    [ParseChildren(true), PersistChildren(false)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true),
        SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true),
        AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
        AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class FabricCompositeField : CompositeField {
        /// <summary>
        /// Gets the name of the default rendering template for CompositeField controls.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that names a rendering template in an .ascx file.
        /// </returns>
        protected override string DefaultTemplateName {
            get {
                if (base.ControlMode == SPControlMode.Display) {
                    return "DisplayFabricCompositeField";
                }
                return "FabricCompositeField";
            }
        }
    }
}
