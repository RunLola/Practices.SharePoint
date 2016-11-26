namespace Practices.SharePoint.Services {
    using Practices.SharePoint.Configuration;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface ICorporateCatalogService {
        #region CRUD of App

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string Create(string title, string launchUrl);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/{productId}",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string Upgrade(string title, string launchUrl, string productId);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/{productId}",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        int Update(Dictionary<string, string> fields, string productId);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{productId}",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string productId);

        #endregion

        #region Operations of AppInstance

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Push/{productId}",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string Push(string productId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Pull/{productId}",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        bool Pull(string productId);

        #endregion
    }
}