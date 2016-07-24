namespace Practices.SharePoint.Apps {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Taxonomy;
    using Microsoft.SharePoint.WebControls;
    using Microsoft.SharePoint.Publishing;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class PackagePage : LayoutsPageBase, IPackageView {
        protected SPWeb PortalWeb {
            get {
                foreach (SPSite site in SPContext.Current.Site.WebApplication.Sites) {
                    if (PublishingSite.IsPublishingSite(site)) {
                        return site.RootWeb;
                    }
                }
                return null;
            }
        }

        protected SPList ThumbnailsList {
            get {
                var list = PortalWeb.Lists.TryGetList("AppThumbnails");
                if (list == null) {
                    var listTemplate = PortalWeb.ListTemplates["文档库"];
                    var listId = PortalWeb.Lists.Add("AppThumbnails", "", listTemplate);
                    list = PortalWeb.Lists.TryGetList("AppThumbnails");
                }
                return PortalWeb.Lists["AppThumbnails"];
            }
        }

        PackagePresenter presenter;

        #region IAppView

        public Guid ProductId {
            get {
                var productId = Guid.Empty;
                var queryString = Request.QueryString["ProductId"];
                if (!string.IsNullOrEmpty(queryString)) {
                    if (Guid.TryParse(queryString, out productId)) {
                    }
                }
                return productId;
            }
        }

        public string LaunchUrl {
            get {
                return AppLaunchUrl.Text.Trim();
            }
            set {
                AppLaunchUrl.Text = value;
            }
        }

        new public string Title {
            get {
                return AppTitle.Text.Trim();
            }
            set {
                AppTitle.Text = value;
            }
        }

        public Dictionary<TaxonomyField, string> Categories {
            get {
                var categories = new Dictionary<TaxonomyField, string>();
                foreach (TableRow row in TaxonomyWebTaggingControls.Rows) {
                    foreach (TableCell cell in row.Cells) {
                        foreach (var control in cell.Controls) {
                            if (control is TaxonomyWebTaggingControl) {
                                var fieldValueControl = (TaxonomyWebTaggingControl)control;
                                var field = presenter.GetCategory(fieldValueControl.FieldId);
                                var fieldValue = fieldValueControl.Text;
                                categories.Add(field, fieldValue);
                            }
                        }
                    }
                }
                return categories;
            }
            set {
                foreach (var category in value) {
                    var field = category.Key;
                    TableRow row = new TableRow();
                    TableCell fieldNameCell = new TableCell();
                    LiteralControl fieldNameControl = new LiteralControl() {
                        Text = field.Title
                    };
                    fieldNameCell.Controls.Add(fieldNameControl);
                    row.Controls.Add(fieldNameCell);
                    TableCell fieldControlCell = new TableCell();
                    TaxonomyWebTaggingControl fieldValueControl = new TaxonomyWebTaggingControl() {
                        FieldId = field.InternalName,
                        FieldName = field.Title,
                        SSPList = field.SspId.ToString(),
                        TermSetList = field.TermSetId.ToString(),
                        AnchorId = field.AnchorId,
                        IsMulti = field.AllowMultipleValues,
                        AllowFillIn = true,
                        CssClass = "pull-right",
                        Text = category.Value
                    };
                    fieldControlCell.Controls.Add(fieldValueControl);
                    row.Controls.Add(fieldControlCell);
                    TaxonomyWebTaggingControls.Controls.Add(row);
                }
            }
        }

        public Dictionary<string, string> Fields {
            get {
                var fields = new Dictionary<string, string>();
                fields.Add(CorporateCatalogBuiltInFields.Type, AppType.SelectedValue);
                fields.Add(CorporateCatalogBuiltInFields.IconUrl, AppThumbnailURL.Text.Trim());
                
                fields.Add(CorporateCatalogBuiltInFields.Recommend, AppRecommend.Checked.ToString());
                fields.Add(CorporateCatalogBuiltInFields.OpenInNewWindow, AppOpenInNewWindow.Checked.ToString());
                fields.Add(CorporateCatalogBuiltInFields.StyleClass, AppStyleClass.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.StyleColor, AppStyleColor.Text.Trim());

                fields.Add(CorporateCatalogBuiltInFields.ShortDescription, AppShortDescription.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.Description, AppDescription.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.SupportURL, AppSupportURL.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.VideoURL, AppVideoURL.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.ImageURL1, AppImageURL1.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.ImageURL2, AppImageURL2.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.ImageURL3, AppImageURL3.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.ImageURL4, AppImageURL4.Text.Trim());
                fields.Add(CorporateCatalogBuiltInFields.ImageURL5, AppImageURL5.Text.Trim());
                return fields;
            }
            set {
                AppType.SelectedValue = value.ContainsKey(CorporateCatalogBuiltInFields.Type) ? value[CorporateCatalogBuiltInFields.Type] : string.Empty;
                AppThumbnailURL.Text = value.ContainsKey(CorporateCatalogBuiltInFields.IconUrl) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.IconUrl]).Url : string.Empty;
                AppShortDescription.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ShortDescription) ? value[CorporateCatalogBuiltInFields.ShortDescription] : string.Empty;
                AppDescription.Text = value.ContainsKey(CorporateCatalogBuiltInFields.Description) ? value[CorporateCatalogBuiltInFields.Description] : string.Empty;
                AppSupportURL.Text = value.ContainsKey(CorporateCatalogBuiltInFields.SupportURL) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.SupportURL]).Url : string.Empty;
                AppVideoURL.Text = value.ContainsKey(CorporateCatalogBuiltInFields.VideoURL) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.VideoURL]).Url : string.Empty;
                AppImageURL1.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ImageURL1) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.ImageURL1]).Url : string.Empty;
                AppImageURL2.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ImageURL2) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.ImageURL2]).Url : string.Empty;
                AppImageURL3.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ImageURL3) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.ImageURL3]).Url : string.Empty;
                AppImageURL4.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ImageURL4) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.ImageURL4]).Url : string.Empty;
                AppImageURL5.Text = value.ContainsKey(CorporateCatalogBuiltInFields.ImageURL5) ? new SPFieldUrlValue(value[CorporateCatalogBuiltInFields.ImageURL5]).Url : string.Empty;
            }
        }

        public IEnumerable<ListItem> Roles {
            get {
                var selected = AppRoles.GetSelectedIndices();
                for (int i = 0; i < selected.Length; i++) {
                    yield return AppRoles.Items[selected[i]];
                }
            }
            set {
                foreach (var item in value) {
                    AppRoles.Items.Add(item);
                }
            }
        }

        #endregion

        public PackagePage() {
            this.presenter = new PackagePresenter(this, PortalWeb);
        }

        protected override void OnInit(EventArgs e) {
            presenter.Init();
            if (this.ProductId.Equals(Guid.Empty)) {
                BtnSave.Visible = false;
                BtnNext.Visible = false;
            } else {
                BtnCreate.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void BtnCreate_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.TrailingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.Begin();
                var productId = presenter.Generate();
                var redirectURL = string.Concat(SPContext.Current.Web.Url, HttpContext.Current.Request.Url.AbsolutePath, "?ProductId=", productId);
                longOperation.End(redirectURL);
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.TrailingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.Begin();
                var productId = presenter.Generate();
                var redirectURL = string.Concat(SPContext.Current.Web.Url, HttpContext.Current.Request.Url.AbsolutePath, "?ProductId=", productId);
                longOperation.End(redirectURL);
            }
        }

        protected void BtnNext_Click(object sender, EventArgs e) {
            using (SPLongOperation longOperation = new SPLongOperation(this.Page)) {
                longOperation.LeadingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.TrailingHTML = "请稍等，这不会花费很长的时间...";
                longOperation.Begin();
                var redirectURL = string.Concat(SPContext.Current.Web.Url, HttpContext.Current.Request.Url.AbsolutePath);
                longOperation.End(redirectURL);
            }
        }
    }
}