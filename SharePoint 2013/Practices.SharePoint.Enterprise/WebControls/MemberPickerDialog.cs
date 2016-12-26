namespace Practices.SharePoint.WebControls {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration.Claims;
    using Microsoft.SharePoint.Portal.WebControls;
    using Microsoft.SharePoint.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    public class MemberPickerDialog : PickerDialog {
        public MemberPickerDialog() :
            base(new MemberQueryControl(), new HierarchyResultControl(), new PeopleEditor(), true) {
            (base.ResultControl as HierarchyResultControl).PickerInitialData = this.GetInitialData();
        }

        private SPProviderInitialData GetInitialData() {
            SPProviderInitialData sPProviderInitialData = new SPProviderInitialData();
            sPProviderInitialData.Description = SPResource.GetString("PikcerEmptyResultText", new object[0]);
            sPProviderInitialData.DefaultImageUrl = "Images/PICKERDEFAULT.GIF";
            sPProviderInitialData.DefaultSipUrl = "images/STSICON.GIF";
            sPProviderInitialData.PickerEnabled = true;
            SPWeb contextWeb = SPControl.GetContextWeb(this.Context);
            Uri uri = new Uri(contextWeb.Url);
            sPProviderInitialData.ContextUrl = ((null == uri) ? string.Empty : uri.ToString());
            sPProviderInitialData.HierarchialDisplayEnabled = true;
            sPProviderInitialData.DetailedViewEnabled = true;
            sPProviderInitialData.ResultTypeIsJson = true;
            sPProviderInitialData.ContextUrl = SPControl.GetContextWeb(HttpContext.Current).Url;
            sPProviderInitialData.ResultDisplay = SPDefaultResultDisplayType.DetailedView;
            SPProviderSchema profileSchema = GetProfileSchema();
            sPProviderInitialData.InitialSchema = new SPProviderSchema[]
			{
				profileSchema
			};
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            SPSchemaElement[] providerSchema = profileSchema.ProviderSchema;
            for (int i = 0; i < providerSchema.Length; i++) {
                SPSchemaElement sPSchemaElement = providerSchema[i];
                dictionary.Add(sPSchemaElement.Name, sPSchemaElement.DisplayName);
            }
            sPProviderInitialData.TableViewColumns = dictionary;
            var queryControl = base.QueryControl as MemberQueryControl;
            if (queryControl != null && queryControl.HierarchyProvider != null) {
                SPProviderHierarchyTree hierarchy = queryControl.HierarchyProvider.GetHierarchy(new Uri(sPProviderInitialData.ContextUrl), null, 2);
                if (hierarchy != null) {
                    sPProviderInitialData.InitialHierarchy = new SPProviderHierarchyTree[]
					{
						hierarchy
					};
                }
            }
            sPProviderInitialData.ResultTypes = new string[]
			{
				SPClaimTypes.UserIdentifier
			};
            return sPProviderInitialData;
        }

        private static SPProviderSchema GetProfileSchema() {
            SPProviderSchema sPProviderSchema = new SPProviderSchema();
            sPProviderSchema.SupportsHierarchy = true;
            //sPProviderSchema.AddSchemaElement(new SPSchemaElement(EntityDataKeys.Picture, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_Picture), SPSchemaElementType.DetailViewOnly));
            //if ((searchFlags & ProfileSearchFlags.Organization) != (ProfileSearchFlags)0) {
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(EntityDataKeys.Leaders, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_Leaders), SPSchemaElementType.Both));
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(EntityDataKeys.MemberCount, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_MemberCount), SPSchemaElementType.Both));
            //}
            //if ((searchFlags & ProfileSearchFlags.User) != (ProfileSearchFlags)0) {
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(PeopleEditorEntityDataKeys.Email, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_Email), SPSchemaElementType.TableViewOnly));
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(PeopleEditorEntityDataKeys.JobTitle, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_JobTitle), SPSchemaElementType.Both));
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(EntityDataKeys.Location, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_Location), SPSchemaElementType.Both));
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(PeopleEditorEntityDataKeys.SIPAddress, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_SIPAddress), SPSchemaElementType.DetailViewOnly));
            //    sPProviderSchema.AddSchemaElement(new SPSchemaElement(PeopleEditorEntityDataKeys.WorkPhone, StringResourceManager.GetString(LocStringId.ClaimProvider_SchemaElementNames_WorkPhone), SPSchemaElementType.TableViewOnly));
            //}
            return sPProviderSchema;
        }
    }
}
