namespace Practices.SharePoint.Publishing {
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Microsoft.SharePoint.ApplicationRuntime;
    using Microsoft.SharePoint.Security;
    using System;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.UI;

    public class FriendlyUrlHttpModule : IHttpModule {
        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public void Init(HttpApplication app) {
            if (app == null || (app as SPHttpApplication) == null) {
                return;
            }
            app.PostAuthorizeRequest += PostAuthorizeRequest;
            app.PreSendRequestHeaders += PreSendRequestHeaders;
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel = true)]
        public void Dispose() {
        }

        protected void PostAuthorizeRequest(object sender, EventArgs e) {
            HttpApplication httpApplication = (HttpApplication)sender;
            HttpContext context = httpApplication.Context;
            string url = SPAlternateUrl.ContextUri.ToString();
            if (IsFriendlyUrlCandidate(url)) {
                string friendlyUrl = string.Empty;
                if (TryParseFriendlyUrl(url, out friendlyUrl)) {
                    context.RewritePath(friendlyUrl, false);
                }
            }
        }

        protected void PreSendRequestHeaders(object sender, EventArgs e) {
            HttpContext current = HttpContext.Current;
            //if (current == null || current.Items.Contains("PublishingCachingHeadersAdded")) {
            //    return;
            //}
            //current.Items["PublishingCachingHeadersAdded"] = true;
            if (current != null ) {
                RewriteRedirectToFriendly(current);
            }
        }

        private static void RewriteRedirectToFriendly(HttpContext context) {
            if (HttpContext.Current.Items["DefaultSPContext"] == null) {
                return;
            } 
            HttpResponse response = context.Response;
            if (response.StatusCode != 301 && response.StatusCode != 302) {
                return;
            }
            string redirectLocation = response.RedirectLocation;
            if (string.IsNullOrEmpty(redirectLocation)) {
                return;
            }
            if (!(context.Handler is Page)) {
                return;
            }
            if (redirectLocation.Contains("User.aspx")) {
                Uri uri;
                if (Uri.TryCreate(redirectLocation, UriKind.RelativeOrAbsolute, out uri)) {
                    SPContext current = SPContext.Current;
                    if (current != null && current.Web != null) {
                        if (!uri.IsAbsoluteUri || redirectLocation.StartsWith(current.Web.Url, StringComparison.OrdinalIgnoreCase)) {
                            string url = uri.IsAbsoluteUri ? uri.PathAndQuery : uri.ToString();
                            if (url.Contains("/Pages/User.aspx")) {
                                string friendlyUrl = url.Replace("/Pages/User.aspx", "/Pages/HomePage.aspx");
                                response.RedirectLocation = friendlyUrl;
                            }
                        }
                    }
                }
            }
        }

        private bool IsFriendlyUrlCandidate(string url) {
            if (url.EndsWith("/Apps")) {
                return true;
            } else {
                return false;
            }            
        }

        private bool TryParseFriendlyUrl(string url, out string friendlyUrl) {
            friendlyUrl = url.Replace("Apps", "Pages/Apps.aspx").Replace("http://practices.contoso.com", "");
            return true;
        }
    }
}
