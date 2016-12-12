namespace Practices.SharePoint.Services {
    using Microsoft.SharePoint;
    using Practices.SharePoint.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Activation;
    using Apps;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CorporateCatalogService : ICorporateCatalogService {
        private SPWeb clientWeb;

        CorporateCatalogAccessor CatalogAccessor {
            get {
                return new CorporateCatalogAccessor(clientWeb, true);
            }
        }

        #region Constructors

        public CorporateCatalogService()
            : this(SPContext.Current != null ? SPContext.Current.Web : null) {
        }

        public CorporateCatalogService(SPWeb clientWeb) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            this.clientWeb = clientWeb;
        }

        #endregion

        #region CRUD of App

        public string Create(string title, string launchUrl) {
            return CatalogAccessor.Create(title, launchUrl);
        }

        public string Upgrade(string title, string launchUrl, string productId) {
            return CatalogAccessor.Upgrade(title, launchUrl, new Guid(productId));
        }

        public int Update(Dictionary<string, string> fields, string productId) {
            return CatalogAccessor.Update(fields, new Guid(productId));
        }

        public bool Delete(string productId) {
            return CatalogAccessor.Delete(new Guid(productId));
        }

        #endregion

        #region Operations of AppInstance

        public string Push(string productId) {
            SPListItem item = CatalogAccessor.Get(new Guid(productId));
            var instance = clientWeb.GetAppInstancesByProductId(new Guid(productId)).FirstOrDefault();
            clientWeb.AllowUnsafeUpdates = true;
            if (instance == null) {
                AppPackageFactory.RegisterPackage(item.File.OpenBinaryStream(), clientWeb);
                instance = clientWeb.LoadAndInstallApp(item.File.OpenBinaryStream());
            } else {
                if (new Version(item[CorporateCatalogBuiltInFields.Version].ToString()) > new Version(instance.App.VersionString)) {
                    instance.Upgrade(item.File.OpenBinaryStream());
                }
            }
            clientWeb.AllowUnsafeUpdates = false;
            return instance.Id.ToString();
        }

        public bool Pull(string productId) {
            var appInstances = clientWeb.GetAppInstancesByProductId(new Guid(productId));
            clientWeb.AllowUnsafeUpdates = true;
            foreach (var appInstance in appInstances) {
                appInstance.Uninstall();
            }
            clientWeb.AllowUnsafeUpdates = false;
            return true;
        }

        #endregion
    }
}