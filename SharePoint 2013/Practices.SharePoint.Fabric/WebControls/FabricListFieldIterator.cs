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
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

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

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        protected override void CreateChildControls() {
            this.Controls.Clear();
            if (ControlTemplate == null)
                throw new ArgumentException("Could not find FabricListFieldIterator control template.");
            FabricPageLayout layout = new FabricPageLayout() {
                Rows = new List<List<FabricPageLayout.Field>>() { 
                    new List<FabricPageLayout.Field>(){
                        new FabricPageLayout.Field(){
                            ClassName="col-sm-12 col-md-12",
                            Title="Title",
                            InternalName ="Title"
                        }
                    },
                    new List<FabricPageLayout.Field>(){
                        new FabricPageLayout.Field(){
                            ClassName="col-sm-12 col-md-6",
                            Title="组织者",
                            InternalName ="组织者"
                        },
                        new FabricPageLayout.Field(){
                            ClassName="col-sm-12 col-md-6",
                            Title="分分",
                            InternalName ="分分"
                        }
                    },
                    new List<FabricPageLayout.Field>(){
                    },
                    new List<FabricPageLayout.Field>(){
                        new FabricPageLayout.Field(){
                            ClassName="col-sm-12 col-md-12",
                            Title="久久",
                            InternalName ="HeiHei"
                        }
                    }
                }
            };
            if (layout != null && layout.Rows.Count() > 0) {
                foreach (var row in layout.Rows) {
                    var gridRow = new HtmlGenericControl("div");
                    gridRow.Attributes.Add("class", "row");
                    this.Controls.Add(gridRow);
                    if (row.Count() > 0) {
                        foreach (var col in row) {
                            var field = Fields.Cast<SPField>().Where(f => f.Title == col.Title || f.InternalName == col.InternalName).FirstOrDefault();
                            if (field != null && !IsFieldExcluded(field)) {
                                AddTemplateContainer(gridRow, field, col.ClassName);
                            }
                        }
                    } else {
                        var hr = new HtmlGenericControl("hr");
                        gridRow.Controls.Add(hr);
                    }
                }
            } else {
                foreach (SPField field in Fields) {
                    AddTemplateContainer(this, field);
                }
            }
        }

        protected virtual void AddTemplateContainer(Control container, SPField field, string className = null) {
            TemplateContainer templateContainer = CreateTemplateContainer(field);
            container.Controls.Add(templateContainer);
            ControlTemplate.InstantiateIn(templateContainer);
            var gridCol = templateContainer.FindControl("GridCol") as HtmlGenericControl;
            if (gridCol == null)
                throw new ArgumentException("Could not find 'GridCol' in FabricListFieldIterator control template.");
            gridCol.Attributes.Add("class", className ?? "col-md-12");
        }

        protected virtual TemplateContainer CreateTemplateContainer(SPField field) {
            TemplateContainer templateContainer = new TemplateContainer();
            //templateContainer.ControlMode = base.ControlMode;                    
            PropertyInfo ControlMode = typeof(TemplateContainer).GetProperty("ControlMode", BindingFlags.NonPublic | BindingFlags.Instance);
            ControlMode.SetValue(templateContainer, base.ControlMode);
            //templateContainer.FieldName = field.InternalName;
            PropertyInfo FieldName = typeof(TemplateContainer).GetProperty("FieldName", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldName.SetValue(templateContainer, field.InternalName);
            return templateContainer;
        }
    }
}
