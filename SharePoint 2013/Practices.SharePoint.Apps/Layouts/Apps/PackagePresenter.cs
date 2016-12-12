namespace Practices.SharePoint.ApplicationPages {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using System.IO.Packaging;
    using System.IO;
    using Microsoft.SharePoint.Taxonomy;
    using System.Web.UI.WebControls;
    using Services;
    using Apps;

    public class PackagePresenter {
        private IPackageView view;
        private SPWeb clientWeb;
        private ICorporateCatalogService service;

        protected bool RunWithElevatedPrivileges {
            get {
                return true;
            }
        }

        private CorporateCatalogAccessor catalogAccessor;
        protected CorporateCatalogAccessor CatalogAccessor {
            get {
                if (catalogAccessor == null) {
                    catalogAccessor = new CorporateCatalogAccessor(clientWeb, RunWithElevatedPrivileges);
                }
                return catalogAccessor;
            }
        }

        public PackagePresenter(IPackageView view, SPWeb clientWeb) {
            this.view = view;
            this.clientWeb = clientWeb;
            this.service = new CorporateCatalogService(clientWeb);
        }
        
        public void Init() {
            var categories = new Dictionary<TaxonomyField, string>();
            var fields = new Dictionary<string,string>();
            var roles = new Dictionary<string, string>();
            if (!view.ProductId.Equals(Guid.Empty)) {
                string identifier;
                string title;
                string launchUrl;
                var item = CatalogAccessor.Get(view.ProductId);
                using (Package package = Package.Open(item.File.OpenBinaryStream(), FileMode.Open)) {
                    package.ParseManifest(out identifier, out title, out launchUrl);
                }
                view.Title = title;
                view.LaunchUrl = launchUrl;
                foreach (var category in GetCategories()) {
                    categories.Add(category, item[category.InternalName] != null ? item[category.InternalName].ToString() : null);//???
                }
                foreach (var key in view.Fields.Keys) {
                    if (item.Fields.ContainsField(key)) {
                        fields.Add(key, item[key] != null ? item[key].ToString() : null);
                    }
                }
            } else {
                foreach (var category in GetCategories()) {
                    categories.Add(category, null);
                }
            }
            view.Fields = fields;
            view.Categories = categories;
            view.Roles = GetRoles();
        }

        public string Generate() {
            string productId = string.Empty;
            if (!view.ProductId.Equals(Guid.Empty)) {
                productId = view.ProductId.ToString();
                service.Upgrade(view.Title, view.LaunchUrl, productId);
            } else {
                productId = service.Create(view.Title, view.LaunchUrl);
            }
            service.Update(view.Fields, productId);
            var fields = new Dictionary<string, string>();
            foreach (var category in view.Categories) {
                fields.Add(category.Key.InternalName, category.Value);
            }
            service.Update(fields, productId);
            service.Push(productId);
            var roles = view.Roles;
            return productId;
        }

        public bool Destroy() {
            service.Pull(view.ProductId.ToString());
            return service.Delete(view.ProductId.ToString());
        }

        public TaxonomyField GetCategory(string fieldName) {
            return GetCategories().Where(c => c.InternalName == fieldName).FirstOrDefault();
        }

        IEnumerable<TaxonomyField> GetCategories() {
            foreach (SPField field in CatalogAccessor.List.Fields) {
                if (field.FieldValueType == typeof(TaxonomyFieldValue) ||
                    field.FieldValueType == typeof(TaxonomyFieldValueCollection)) {
                    TaxonomyField taxonomyField = field as TaxonomyField;
                    yield return taxonomyField;
                }
            }
        }

        List<ListItem> GetRoles() {
            var roles = new List<ListItem>();
            roles.Add(new ListItem() { 
                Text = "Role1",
                Value = "1",
                Selected = true
            });
            roles.Add(new ListItem() {
                Text = "Role2",
                Value = "2",
            });
            roles.Add(new ListItem() {
                Text = "Role3",
                Value = "3",
            });
            roles.Add(new ListItem() {
                Text = "Role4",
                Value = "4",
                Selected = true
            });
            return roles;
        }
    }
}
