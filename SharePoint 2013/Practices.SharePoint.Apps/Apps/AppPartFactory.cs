namespace Practices.SharePoint.Apps {
    using System;
    using System.IO;
    using System.IO.Packaging;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public static class AppPartFactory {
        public static PackagePart CreateAppManifest(this Package package, Guid productId, Guid identifier, string title, string launchUrl) {
            AppManifest o = new AppManifest() {
                Name = Guid.NewGuid().ToString(),
                ProductID = productId,
                Version = "1.0.0.0",
                SharePointMinVersion = "15.0.0.0",
                Properties = new AppManifest.AppProperties() {
                    Title = title,
                    LaunchUrl = launchUrl
                },
                Principal = new AppManifest.AppPrincipal {
                    RemoteWebApplication = new AppManifest.RemoteWebApplication {
                        ClientId = identifier
                    }
                }
            };

            Uri partUri = new Uri("/AppManifest.xml", UriKind.Relative);
            string contentType = "text/xml";
            PackagePart part = package.CreatePart(partUri, contentType);
            using (Stream stream = part.GetStream(FileMode.Create, FileAccess.Write))
            using (XmlWriter writer = XmlTextWriter.Create(stream)) {
                XmlSerializer serializer = new XmlSerializer(typeof(AppManifest));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "http://schemas.microsoft.com/sharepoint/2012/app/manifest");
                serializer.Serialize(writer, o, namespaces);
            }
            return part;
        }

        public static PackagePart UpgradeAppManifest(this Package package, string title, string launchUrl) {
            PackagePart part = package.GetPart(new Uri("/AppManifest.xml", UriKind.Relative));
            using (Stream stream = part.GetStream(FileMode.Open, FileAccess.Read)) {
                using (XmlReader reader = XmlReader.Create(stream)) {
                    XDocument xdoc = XDocument.Load(reader);
                    xdoc.Root.Element(ParseXName("Properties")).Element(ParseXName("Title")).Value = title;
                    xdoc.Root.Element(ParseXName("Properties")).Element(ParseXName("StartPage")).ReplaceNodes(new XCData(launchUrl));
                    Version version;
                    if (Version.TryParse(xdoc.Root.Attribute("Version").Value, out version)) {
                        xdoc.Root.Attribute("Version").SetValue((version.Major + 1) + ".0.0.0");
                    }
                    using (XmlWriter writer = XmlTextWriter.Create(part.GetStream(FileMode.Create, FileAccess.Write))) {
                        xdoc.Save(writer);
                    }
                }
            }
            return part;
        }

        // Microsoft.SharePoint.Packaging.SPAppManifest.ParseManifest();
        public static void ParseManifest(this Package package, out string identifier, out string title, out string launchUrl) {
            PackagePart part = package.GetPart(new Uri("/AppManifest.xml", UriKind.Relative));
            using (Stream stream = part.GetStream(FileMode.Open, FileAccess.Read)) {
                using (XmlReader reader = XmlReader.Create(stream)) {
                    XDocument xdoc = XDocument.Load(reader);
                    //productId = xdoc.Root.Attribute("ProductId").Value;
                    identifier = xdoc.Root.Element(ParseXName("AppPrincipal")).Element(ParseXName("RemoteWebApplication")).Attribute("ClientId").Value;
                    title = xdoc.Root.Element(ParseXName("Properties")).Element(ParseXName("Title")).Value;
                    launchUrl = xdoc.Root.Element(ParseXName("Properties")).Element(ParseXName("StartPage")).Value;
                }
            }
        }

        public static PackagePart CreateAppIcon(this Package package, MemoryStream imageStream) {
            Uri partUri = new Uri("/AppIcon.png", UriKind.Relative);
            string contentType = "application/wsp";
            PackagePart part = package.CreatePart(partUri, contentType);
            using (Stream stream = part.GetStream(FileMode.Create, FileAccess.Write)) {
                using (BinaryWriter writer = new BinaryWriter(stream)) {
                    writer.Write(imageStream.ToArray());
                }
            }
            return part;
        }

        public static PackagePart CreateAppIconConfig(this Package package) {
            AppPartConfig o = new AppPartConfig() {
                Id = Guid.NewGuid().ToString()
            };
            Uri partUri = new Uri("/AppIcon.png.config.xml", UriKind.Relative);
            string contentType = "text/xml";
            PackagePart part = package.CreatePart(partUri, contentType);
            using (Stream stream = part.GetStream(FileMode.Create, FileAccess.Write))
            using (XmlWriter writer = XmlTextWriter.Create(stream)) {
                XmlSerializer serializer = new XmlSerializer(typeof(AppPartConfig));
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "http://schemas.microsoft.com/sharepoint/2012/app/partconfiguration");
                serializer.Serialize(writer, o, namespaces);
            }
            return part;
        }

        private static string ParseXName(string name) {
            return "{http://schemas.microsoft.com/sharepoint/2012/app/manifest}" + name;
        }
    }
}
