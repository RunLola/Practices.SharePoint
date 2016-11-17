namespace Practices.SharePoint {
    using Microsoft.SharePoint;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Activation;
    using Apps;
    using Microsoft.SharePoint.Linq;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class AppsDataService : ListDataService {
        private SPWeb clientWeb;

        private AppCatalogAccessor catalogAccessor;
        protected AppCatalogAccessor CatalogAccessor {
            get {
                if (catalogAccessor == null) {
                    catalogAccessor = new AppCatalogAccessor(clientWeb, true);
                }
                return catalogAccessor;
            }
        }

        //public AppsService() {
        //    if (clientWeb == null && SPContext.Current != null) {
        //        this.clientWeb = SPContext.Current.Web;
        //    }
        //}

        //public AppsService(SPWeb clientWeb) {
        //    this.clientWeb = clientWeb;
        //}

        public string Create(string title, string launchUrl) {
            Guid identifier = Guid.NewGuid();
            using (Stream stream = AppPackageFactory.CreatePackage(identifier, title, launchUrl)) {
                CatalogAccessor.List.ParentWeb.AllowUnsafeUpdates = true;
                string name = Guid.NewGuid().ToString("D") + ".app";
                SPFile file = CatalogAccessor.List.RootFolder.Files.Add(name, stream);
                CatalogAccessor.List.ParentWeb.AllowUnsafeUpdates = false;
                return file.Item[AppBuiltInFields.ProductID] as string;
            }
        }

        public string Upgrade(string title, string launchUrl, string productId) {
            SPListItem item = CatalogAccessor.GetAppByProductId(new Guid(productId));
            using (Stream stream = AppPackageFactory.UpgradePackage(item.File.OpenBinaryStream(), title, launchUrl)) {
                item.Web.AllowUnsafeUpdates = true;
                item.File.SaveBinary(stream);
                item.Web.AllowUnsafeUpdates = false;
                return item[AppBuiltInFields.Version] as string;
            }
        }

        public int Update(Dictionary<string, string> fields, string productId) {
            SPListItem item = CatalogAccessor.GetAppByProductId(new Guid(productId));
            item.Web.AllowUnsafeUpdates = true;

            foreach (var field in fields) {
                if (item.Fields.ContainsField(field.Key)) {
                    //item[field.Key] = field.Value;
                    item.Fields.GetField(field.Key).ParseAndSetValue(item, field.Value);
                }
            }
            item.Update();
            item.Web.AllowUnsafeUpdates = false;
            return fields.Count;
        }

        public bool Delete(string productId) {
            SPListItem item = CatalogAccessor.GetAppByProductId(new Guid(productId));
            CatalogAccessor.List.ParentWeb.AllowUnsafeUpdates = true;
            item.Delete();
            CatalogAccessor.List.ParentWeb.AllowUnsafeUpdates = false;
            return true;
        }

        public string Install(string productId) {
            SPListItem item = CatalogAccessor.GetAppByProductId(new Guid(productId));
            var appInstance = clientWeb.GetAppInstancesByProductId(new Guid(productId)).FirstOrDefault();
            clientWeb.AllowUnsafeUpdates = true;
            if (appInstance == null) {
                AppPackageFactory.RegisterPackage(item.File.OpenBinaryStream(), clientWeb);
                appInstance = clientWeb.LoadAndInstallApp(item.File.OpenBinaryStream());
            } else {
                if (new Version(item[AppBuiltInFields.Version].ToString()) > new Version(appInstance.App.VersionString)) {
                    appInstance.Upgrade(item.File.OpenBinaryStream());
                }
            }
            clientWeb.AllowUnsafeUpdates = false;
            return appInstance.Id.ToString();
        }

        public bool Uninstall(string productId) {
            var appInstances = clientWeb.GetAppInstancesByProductId(new Guid(productId));

            clientWeb.AllowUnsafeUpdates = true;
            foreach (var appInstance in appInstances) {
                appInstance.Uninstall();
            }
            clientWeb.AllowUnsafeUpdates = false;

            return true;
        }

        public string Generate(string title, string launchUrl, Dictionary<string, string> fields, string productId) {
            if (new Guid(productId).Equals(Guid.Empty)) {
                productId = Create(title, launchUrl);
            } else {
                Upgrade(title, launchUrl, productId);
            }
            Update(fields, productId);
            string appInstanceId = Install(productId);
            return appInstanceId;
        }

        public bool Destroy(string productId) {
            Uninstall(productId);
            Delete(productId);
            return true;
        }

        public string Invoke(string title, string launchUrl) {
            string productId = Create(title, launchUrl);
            string instanceId = "";
            SPSecurity.RunWithElevatedPrivileges(delegate() {
                string siteUrl = SPContext.Current.Site.Url;
                SPUser user = SPContext.Current.Site.RootWeb.SiteAdministrators[0];
                SPUserToken userToken = user.UserToken;
                using (SPSite site = new SPSite(siteUrl, userToken)) {
                    //instanceId = Install(productId);
                }
            });
            return instanceId;

            //using (SPSite site = new SPSite(siteUrl, user.UserToken)) {
            //    SPListItem item = CatalogAccessor.GetAppByProductId(new Guid(productId));
            //    AppPackageFactory.RegisterPackage(item.File.OpenBinaryStream(), clientWeb);

            //    var instance = clientWeb.GetAppInstancesByProductId(new Guid(productId)).FirstOrDefault();

            //    clientWeb.AllowUnsafeUpdates = true;
            //    if (instance == null) {
            //        instance = clientWeb.LoadAndInstallApp(item.File.OpenBinaryStream());
            //    } else {
            //        if (new Version(item[AppBuiltInFields.Version].ToString()) > new Version(instance.App.VersionString)) {
            //            instance.Upgrade(item.File.OpenBinaryStream());
            //        }
            //    }
            //    clientWeb.AllowUnsafeUpdates = false;

            //    return instance.Id.ToString();
            //}
        }
    }
}