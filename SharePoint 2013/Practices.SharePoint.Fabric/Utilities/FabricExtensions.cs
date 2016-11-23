namespace Practices.SharePoint.Utilities {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.WebPartPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.UI.WebControls.WebParts;
    
    public static class FabricExtensions {
        public static void SetFormTempalteName(this SPList list, SPControlMode controlMode, string templateName) {
            //var form = list.Forms.Cast<SPForm>().Where(f => f.Type == type).FirstOrDefault();
            //if (form == null)
            //    throw new ArgumentOutOfRangeException("PAGETYPE");
            //var file = list.ParentWeb.GetFile(list.ParentWeb.Url + "/" + form.Url);
            //if (file.CheckOutType == SPFile.SPCheckOutType.None) {
            //    list.ParentWeb.AllowUnsafeUpdates = true;
            //    file.CheckOut();
            //    using (SPLimitedWebPartManager webpartManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared)) {
            //        foreach (System.Web.UI.WebControls.WebParts.WebPart webpart in webpartManager.WebParts) {
            //            if (webpart is ListFormWebPart) {
            //                ListFormWebPart listFormWebPart = (ListFormWebPart)webpart;
            //                listFormWebPart.TemplateName = templateName;
            //                webpartManager.SaveChanges(listFormWebPart);
            //                break;
            //            }
            //        }
            //        file.Update();
            //    }
            //    file.CheckIn("Fabric UI");
            //    list.ParentWeb.AllowUnsafeUpdates = false;
            //} else {
            //    throw new SPFileCheckOutException(file, form.Url + " has been CheckOut", 0, SPFileCheckOutExceptionType.OnlineCheckOutExists);
            //}            
        }

        public static void SetFormTempalteName(this SPContentType contentType, SPControlMode controlMode, string templateName) {
            contentType.EditFormTemplateName = "";
        }
    }
}
