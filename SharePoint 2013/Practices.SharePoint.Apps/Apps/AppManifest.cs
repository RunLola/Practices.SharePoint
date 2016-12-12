namespace Practices.SharePoint {
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Microsoft.SharePoint.Packaging.SPAppManifest
    /// Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c
    /// </summary>
    [XmlRoot("App", Namespace = "http://schemas.microsoft.com/sharepoint/2012/app/manifest")]
    public class AppManifest {
        [XmlAttribute("ProductID")]
        public Guid ProductID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
        
        [XmlAttribute("Version")]
        public string Version { get; set; }


        [XmlAttribute("SharePointMinVersion")]
        public string SharePointMinVersion { get; set; }

        [XmlElement]
        public AppProperties Properties { get; set; }
        
        [XmlElement(ElementName = "AppPrincipal")]
        public AppPrincipal Principal { get; set; }
        
        [XmlRoot]
        public class AppProperties {
            [XmlElement]
            public string Title { get; set; }

            [XmlIgnore]
            public string LaunchUrl { get; set; }
            
            [XmlElement]
            public XmlCDataSection StartPage {
                get {
                    return new XmlDocument().CreateCDataSection(LaunchUrl);
                }
                set {
                    LaunchUrl = value.Value;
                }
            }
        }

        [XmlRoot]
        public class AppPrincipal {
            [XmlElement]
            public RemoteWebApplication RemoteWebApplication { get; set; }
        }

        [XmlRoot]
        public class RemoteWebApplication {
            [XmlAttribute("ClientId")]
            public Guid ClientId { get; set; }
        }
    }
}