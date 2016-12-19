namespace Practices.SharePoint.Services {
    using Models;
    using Practices.SharePoint.Configuration;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    [ServiceContract]
    public interface IProfilesCatalogService {
        [OperationContract]
        [WebGet(UriTemplate = "/Orgs",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<Organization> GetOrgs(int orgId);

        [OperationContract]
        [WebGet(UriTemplate = "/Users",
            BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<SiteUser> GetUsers(int orgId);
    }
}