namespace Practices.SharePoint.Apps {
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot("AppPartConfig", Namespace = "http://schemas.microsoft.com/sharepoint/2012/app/partconfiguration")]
    public class AppPartConfig {
        [XmlElement]
        public string Id { get; set; }
    }
}