namespace Practices.SharePoint.ApplicationPages {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.WebPartPages;
    using WebControls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.UI.WebControls.WebParts;
    using Utilities;
    using Configuration;

    public partial class FormTemplateSettingsPage : LayoutsPageBase {
        protected Guid? ListId {
            get {
                if (!string.IsNullOrEmpty(Request.QueryString["List"])) {
                    var listId = Guid.Empty;
                    if (Guid.TryParse(Request.QueryString["List"], out listId)) {
                        return listId;
                    } else {
                        throw new ArgumentOutOfRangeException("ListId");
                    }
                } else {
                    return null;
                }
            }
        }

        protected SPList List {
            get {
                if (ListId.HasValue && SPContext.Current != null) {
                    return SPContext.Current.Web.Lists[ListId.Value];
                } else {
                    return null;
                }
            }
        }

        protected SPContentTypeId? ContentTypeId {
            get {
                if (!string.IsNullOrEmpty(Request.QueryString["ctype"])) {
                    var contentTypeId = new SPContentTypeId(Request.QueryString["ctype"]);
                    return contentTypeId;
                } else {
                    return null;
                }
            }
        }

        new protected SPContentType ContentType {
            get {
                if (ContentTypeId.HasValue) {
                    if (List != null) {
                        return List.ContentTypes[ContentTypeId.Value];
                    } else {
                        return SPContext.Current.Web.ContentTypes[ContentTypeId.Value];
                    }
                } else {
                    return null;
                }
            }
        }

        protected SPFieldCollection Fields {
            get {
                if (ContentType != null) {
                    return ContentType.Fields;
                } else {
                    return List.Fields;
                }
            }
        }

        protected override void OnInit(EventArgs e) {
            //var fields = new List<SPField>();
            //foreach (SPField field in Fields) {
            //    if (!IsFieldExcluded(SPControlMode.New, field)) {
            //        fields.Add(field);
            //    }
            //}
            GridCols.DataSource = Fields.Cast<SPField>()
                .Where(f => !IsFieldExcluded(SPControlMode.New, f));
            GridCols.DataBind();
            IList<FabricPageLayout> pageLayouts = new List<FabricPageLayout>();
            pageLayouts.Add(Get(SPControlMode.New));
            //pageLayouts.Add(Get(SPControlMode.Edit));
            //pageLayouts.Add(Get(SPControlMode.Display));
            json.Value = JsonHelper.JsonSerializer<IEnumerable<FabricPageLayout>>(pageLayouts);
        }

        protected void BtnSave_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "Working on it...";
                longOperation.TrailingHTML = "Working on it...";
                longOperation.Begin();

                if (!string.IsNullOrEmpty(json.Value)) {
                    var pageLayouts = JsonHelper.JsonDeserialize<IEnumerable<FabricPageLayout>>(json.Value);
                    Set(SPControlMode.New, pageLayouts.FirstOrDefault());
                    //Set(SPControlMode.Edit, pageLayouts.FirstOrDefault());
                    //Set(SPControlMode.Display, pageLayouts.FirstOrDefault());
                }

                var redirectURL = HttpContext.Current.Request.Url.AbsoluteUri;
                longOperation.End(redirectURL);
            }
        }

        protected virtual FabricPageLayout Get(SPControlMode controlMode) {
            ConfigManager configManager = new ConfigManager();
            if (List != null && ContentType != null) {
                ContentType.SetFormTempalteName(controlMode, "FabricListForm");
                //IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.List);
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                return configManager.GetPropertyBag<FabricPageLayout>("ListContentType" + List.ID.ToString() + ContentType.Id + controlMode, propertyBag);
            } else if (List != null) {
                List.SetFormTempalteName(controlMode, "FabricListForm");
                //IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.List);
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                return configManager.GetPropertyBag<FabricPageLayout>("List" + List.ID.ToString() + controlMode, propertyBag);
            } else if (ContentType != null) {
                ContentType.SetFormTempalteName(controlMode, "FabricListForm");
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                return configManager.GetPropertyBag<FabricPageLayout>("SiteContentType" + ContentType.Id + controlMode, propertyBag);
            } else {
                return new FabricPageLayout();
            }
        }

        protected virtual void Set(SPControlMode controlMode, FabricPageLayout pageLayout) {
            ConfigManager configManager = new ConfigManager();
            if (List != null && ContentType != null) {
                ContentType.SetFormTempalteName(controlMode, "FabricListForm");
                //IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.List);
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                configManager.SetPropertyBag("ListContentType" + List.ID.ToString() + ContentType.Id + controlMode, pageLayout, propertyBag);
            } else if (List != null) {
                List.SetFormTempalteName(controlMode, "FabricListForm");
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                configManager.SetPropertyBag("List" + List.ID.ToString() + controlMode, pageLayout, propertyBag);
            } else if (ContentType != null) {
                ContentType.SetFormTempalteName(controlMode, "FabricListForm");
                //IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.List);
                IPropertyBag propertyBag = configManager.GetPropertyBag(ConfigScope.Web);
                configManager.SetPropertyBag("SiteContentType" + ContentType.Id + controlMode, pageLayout, propertyBag);
            }
        }

        protected virtual bool IsFieldExcluded(SPControlMode controlMode, SPField field) {
            var fieldIterator = new ListFieldIterator();
            fieldIterator.ControlMode = controlMode;
            MethodInfo IsFieldExcluded = typeof(ListFieldIterator).GetMethod("IsFieldExcluded", BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)IsFieldExcluded.Invoke(fieldIterator, new object[] { field });
        }
    }
}