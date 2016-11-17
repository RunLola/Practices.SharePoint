namespace Practices.SharePoint.Publishing {
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    [Guid("cf809497-8a9e-4cc4-889f-b7bbf006a085")]
    public class PracticesEventReceiver : SPFeatureReceiver {
        public override void FeatureActivated(SPFeatureReceiverProperties properties) {
            //RegisterHttpModule(properties);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties) {
            //UnregisterHttpModule(properties);
        }

        private void RegisterHttpModule(SPFeatureReceiverProperties properties) {
            SPWebConfigModification webConfigModification = CreateWebModificationObject();
            SPWebService contentService = SPWebService.ContentService;
            contentService.WebConfigModifications.Add(webConfigModification);
            contentService.Update();
            contentService.ApplyWebConfigModifications();
        }

        private void UnregisterHttpModule(SPFeatureReceiverProperties properties) {
            SPWebConfigModification webConfigModification = CreateWebModificationObject();
            SPWebService contentService = properties.Definition.Farm.Services.GetValue<SPWebService>();
            int numberOfModifications = contentService.WebConfigModifications.Count;
            //Iterate over all WebConfigModification and delete only those we have created
            for (int i = numberOfModifications - 1; i >= 0; i--) {
                SPWebConfigModification currentModifiction = contentService.WebConfigModifications[i];
                if (currentModifiction.Name.Contains("FriendlyUrlHttpModule")) {
                    contentService.WebConfigModifications.Remove(currentModifiction);
                }
                if (currentModifiction.Owner.Equals(webConfigModification.Owner)) {
                    contentService.WebConfigModifications.Remove(currentModifiction);
                }
            }

            //Update only if we have something deleted
            if (numberOfModifications > contentService.WebConfigModifications.Count) {
                contentService.Update();
                contentService.ApplyWebConfigModifications();
            }
        }

        /// <summary>
        /// Create the SPWebConfigModification object for the signalr module
        /// </summary>
        /// <returns>SPWebConfigModification object for the http module to the web.config</returns>
        private SPWebConfigModification CreateWebModificationObject() {
            string name = String.Format("add[@name=\"{0}\"]", typeof(FriendlyUrlHttpModule).Name);
            string xpath = "/configuration/system.webServer/modules";

            SPWebConfigModification webConfigModification = new SPWebConfigModification(name, xpath) {
                Owner = "Practices.Publishing",
                Sequence = 0,
                Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode
            };

            //reflection safe
            webConfigModification.Value = String.Format("<add name=\"{0}\" type=\"{1}\" />",
                                                typeof(FriendlyUrlHttpModule).Name,
                                                typeof(FriendlyUrlHttpModule).AssemblyQualifiedName);
            return webConfigModification;
        }
    }
}
