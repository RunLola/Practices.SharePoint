namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class FabricTemplateContainer : TemplateBasedControl {
        private string fieldName;
        public string FieldName {
            get {
                if (fieldName == null) {
                    Control parentTemplate = TemplateBasedControl.GetParentTemplateBasedControl(this);
                    while (parentTemplate != null && !(parentTemplate is FieldMetadata)) {
                        parentTemplate = TemplateBasedControl.GetParentTemplateBasedControl(parentTemplate);
                    }
                    if (parentTemplate is FieldMetadata) {
                        fieldName = ((FieldMetadata)parentTemplate).FieldName;
                    }
                }
                return fieldName;
            }
            set {
                fieldName = value;
            }
        }

        private SPContext renderContext;
        public SPContext ItemContext {
            get {
                if (renderContext == null) {
                    Control parentFormComponentControl = GetParentFormComponentControl(this);
                    if (parentFormComponentControl != null) {
                        if (parentFormComponentControl is FormComponent) {
                            renderContext = ((FormComponent)parentFormComponentControl).ItemContext;
                        } else {
                            //renderContext = ((TemplateContainer)parentFormComponentControl).ItemContext;
                        }
                    }
                }
                return this.renderContext;
            }
            set {
                renderContext = value;
            }
        }

        static Control GetParentFormComponentControl(Control control) {
            Control parent = control.Parent;
            while (parent != null && !(parent is HtmlForm)) {
                if (parent is FormComponent || parent is TemplateContainer) {
                    return parent;
                }
                parent = parent.Parent;
            }
            return null;
        }
    }
}
