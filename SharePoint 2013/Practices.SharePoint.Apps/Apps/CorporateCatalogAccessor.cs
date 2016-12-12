namespace Practices.SharePoint.Apps {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebPartPages;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Linq;
    using System.Reflection;
    using System.Web.UI;
    using Utilities;

    /// <summary>
    /// CorporateCatalogAccessor
    /// </summary>
    public class CorporateCatalogAccessor : ListRepository, IDisposable {
        private bool elevatedPrivileges;
        private SPWeb clientWeb;

        private SPList list;
        public override SPList List {
            get {
                if (list == null) {
                    if (elevatedPrivileges) {
                        SPSecurity.RunWithElevatedPrivileges(delegate() {
                            list = GetAppCatalog(clientWeb);
                        });
                    } else {
                        list = GetAppCatalog(clientWeb);
                    }
                }
                return list;
            }
        }

        #region Constructors

        public CorporateCatalogAccessor()
            : this(null, false) {
        }

        public CorporateCatalogAccessor(SPWeb clientWeb)
            : this(clientWeb, false) {
        }

        public CorporateCatalogAccessor(bool elevatedPrivileges)
            : this(SPContext.Current != null ? SPContext.Current.Web : null, elevatedPrivileges) {
        }

        public CorporateCatalogAccessor(SPWeb clientWeb, bool elevatedPrivileges) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");

            this.clientWeb = clientWeb;
            this.elevatedPrivileges = elevatedPrivileges;

            List.ParentWeb.AllowUnsafeUpdates = true;
        }

        #endregion

        #region CRUD Methods

        public string Create(string title, string launchUrl) {
            return Create(Guid.NewGuid(), title, launchUrl);
        }

        public string Create(Guid productId, string title, string launchUrl) {
            var identifier = Guid.NewGuid();
            using (Stream stream = AppPackageFactory.CreatePackage(productId, identifier, title, launchUrl)) {
                string name = Guid.NewGuid().ToString() + ".app";
                SPFile file = List.RootFolder.Files.Add(name, stream);
                if (bool.Parse(file.Item[CorporateCatalogBuiltInFields.IsValid].ToString())) {
                    return new Guid(file.Item[CorporateCatalogBuiltInFields.ProductID].ToString()).ToString();
                } else {
                    return null;
                }
            }
        }

        public string Upgrade(string title, string launchUrl, Guid productId) {
            SPListItem item = Get(productId);            
            using (Stream stream = AppPackageFactory.UpgradePackage(item.File.OpenBinaryStream(), title, launchUrl)) {
                item.File.SaveBinary(stream);
                return item[CorporateCatalogBuiltInFields.Version] as string;
            }
        }

        public int Update(IDictionary<string, string> fields, Guid productId) {
            SPListItem item = Get(productId);
            foreach (var field in fields) {
                if (item.Fields.ContainsField(field.Key)) {
                    //item[field.Key] = field.Value;
                    item.Fields.GetField(field.Key).ParseAndSetValue(item, field.Value);
                }
            }
            item.Update();
            return fields.Count;
        }

        public bool Delete(Guid productId) {
            SPListItem item = Get(productId);
            item.Recycle();
            return true;
        }

        #endregion

        public SPListItem Get(Guid productId) {
            string queryString = new CAMLQueryBuilder()
                .AddEqual(CorporateCatalogBuiltInFields.IsValid, true)
                .AddEqual(CorporateCatalogBuiltInFields.ProductID, productId).Build();
            var item = Get(queryString).FirstOrDefault();
            Validation.ArgumentNotNull(item, "item");
            return item;
        }

        public string GetAppLaunchUrl(Guid productId) {
            var item = Get(productId);
            return AppPackageFactory.ParseAppLaunchUrl(item.File.OpenBinaryStream());
        }

        public IEnumerable<Guid> GetAppProductIds(string queryString) {
            SPQuery query = new SPQuery() {
                Query = queryString,
                ViewFields = string.Concat(string.Format("<FieldRef Name='{0}' />", CorporateCatalogBuiltInFields.ProductID)),
                ViewFieldsOnly = true,
            };
            var items = Get(query);
            return items.Select(item => new Guid(item[CorporateCatalogBuiltInFields.ProductID].ToString()));
        }

        public override IEnumerable<SPListItem> Get(string queryString) {
            var query = new SPQuery() {
                Query = queryString
            };
            return Get(query);
        }

        public override IEnumerable<SPListItem> Get(string queryString, uint startRow, uint maxRows) {
            var query = new SPQuery() {
                Query = queryString
            };
            return Get(query, startRow, maxRows);
        }

        public static SPList GetAppCatalog(SPWeb clientWeb) {
            Validation.ArgumentNotNull(clientWeb, "clientWeb");
            /// <summary>
            /// http://social.technet.microsoft.com/wiki/contents/articles/14423.sharepoint-2013-existing-features-guid.aspx
            /// </summary>
            Guid CorporateCuratedGallerySettingsFeatureID = new Guid("F8BEA737-255E-4758-AB82-E34BB46F5828");
            SPFeature feature = clientWeb.Site.WebApplication.Features[CorporateCuratedGallerySettingsFeatureID];
            Guid siteId = new Guid(feature.Properties["__AppCatSiteId"].Value);
            Guid listId = new Guid(feature.Properties["__AppCatListId"].Value);
            using (SPSite site = new SPSite(siteId))
            using (SPWeb web = site.RootWeb) {
                SPList list = web.Lists[listId];
                return list;
            }
        }

        public void Dispose() {
            List.ParentWeb.AllowUnsafeUpdates = false;
        }

        public static WebPartGalleryItemBase[] GetAppPartItems(Page page, SPWeb web) {
            Type t = typeof(WebPartAdder);
            Type type = t.GetNestedType("SPAppsWebPartGalleryProvider", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo methodInfo = type.GetMethod("GetItemsCore", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { page, web };
            object obj = Activator.CreateInstance(type, parameters);
            object result = methodInfo.Invoke(obj, null);
            WebPartGalleryItemBase[] items = result as WebPartGalleryItemBase[];
            return items;
        }
    }
}