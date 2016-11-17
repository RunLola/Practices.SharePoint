namespace Practices.SharePoint.Apps {
    using Microsoft.SharePoint;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Security;
    using System.Security.Cryptography;

    /// <summary>
    /// Microsoft.SharePoint.Packaging.SPPackageFactory; SPAppPackage; SPAppManifest
    /// Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c
    /// </summary>
    public static class AppPackageFactory {
        public static Stream CreatePackage(Guid productId, Guid identifier, string title, string launchUrl) {
            string base64String = "iVBORw0KGgoAAAANSUhEUgAAAGAAAABgCAIAAABt+uBvAAAABGdBTUEAALGOfPtRkwAAACBjSFJNAACHDwAAjA8AAP1SAACBQAAAfXkAAOmLAAA85QAAGcxzPIV3AAAKOWlDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAEjHnZZ3VFTXFofPvXd6oc0wAlKG3rvAANJ7k15FYZgZYCgDDjM0sSGiAhFFRJoiSFDEgNFQJFZEsRAUVLAHJAgoMRhFVCxvRtaLrqy89/Ly++Osb+2z97n77L3PWhcAkqcvl5cGSwGQyhPwgzyc6RGRUXTsAIABHmCAKQBMVka6X7B7CBDJy82FniFyAl8EAfB6WLwCcNPQM4BOB/+fpFnpfIHomAARm7M5GSwRF4g4JUuQLrbPipgalyxmGCVmvihBEcuJOWGRDT77LLKjmNmpPLaIxTmns1PZYu4V8bZMIUfEiK+ICzO5nCwR3xKxRoowlSviN+LYVA4zAwAUSWwXcFiJIjYRMYkfEuQi4uUA4EgJX3HcVyzgZAvEl3JJS8/hcxMSBXQdli7d1NqaQffkZKVwBALDACYrmcln013SUtOZvBwAFu/8WTLi2tJFRbY0tba0NDQzMv2qUP91829K3NtFehn4uWcQrf+L7a/80hoAYMyJarPziy2uCoDOLQDI3fti0zgAgKSobx3Xv7oPTTwviQJBuo2xcVZWlhGXwzISF/QP/U+Hv6GvvmckPu6P8tBdOfFMYYqALq4bKy0lTcinZ6QzWRy64Z+H+B8H/nUeBkGceA6fwxNFhImmjMtLELWbx+YKuGk8Opf3n5r4D8P+pMW5FonS+BFQY4yA1HUqQH7tBygKESDR+8Vd/6NvvvgwIH554SqTi3P/7zf9Z8Gl4iWDm/A5ziUohM4S8jMX98TPEqABAUgCKpAHykAd6ABDYAasgC1wBG7AG/iDEBAJVgMWSASpgA+yQB7YBApBMdgJ9oBqUAcaQTNoBcdBJzgFzoNL4Bq4AW6D+2AUTIBnYBa8BgsQBGEhMkSB5CEVSBPSh8wgBmQPuUG+UBAUCcVCCRAPEkJ50GaoGCqDqqF6qBn6HjoJnYeuQIPQXWgMmoZ+h97BCEyCqbASrAUbwwzYCfaBQ+BVcAK8Bs6FC+AdcCXcAB+FO+Dz8DX4NjwKP4PnEIAQERqiihgiDMQF8UeikHiEj6xHipAKpAFpRbqRPuQmMorMIG9RGBQFRUcZomxRnqhQFAu1BrUeVYKqRh1GdaB6UTdRY6hZ1Ec0Ga2I1kfboL3QEegEdBa6EF2BbkK3oy+ib6Mn0K8xGAwNo42xwnhiIjFJmLWYEsw+TBvmHGYQM46Zw2Kx8lh9rB3WH8vECrCF2CrsUexZ7BB2AvsGR8Sp4Mxw7rgoHA+Xj6vAHcGdwQ3hJnELeCm8Jt4G749n43PwpfhGfDf+On4Cv0CQJmgT7AghhCTCJkIloZVwkfCA8JJIJKoRrYmBRC5xI7GSeIx4mThGfEuSIemRXEjRJCFpB+kQ6RzpLuklmUzWIjuSo8gC8g5yM/kC+RH5jQRFwkjCS4ItsUGiRqJDYkjiuSReUlPSSXK1ZK5kheQJyeuSM1J4KS0pFymm1HqpGqmTUiNSc9IUaVNpf+lU6RLpI9JXpKdksDJaMm4ybJkCmYMyF2TGKQhFneJCYVE2UxopFykTVAxVm+pFTaIWU7+jDlBnZWVkl8mGyWbL1sielh2lITQtmhcthVZKO04bpr1borTEaQlnyfYlrUuGlszLLZVzlOPIFcm1yd2WeydPl3eTT5bfJd8p/1ABpaCnEKiQpbBf4aLCzFLqUtulrKVFS48vvacIK+opBimuVTyo2K84p6Ss5KGUrlSldEFpRpmm7KicpFyufEZ5WoWiYq/CVSlXOavylC5Ld6Kn0CvpvfRZVUVVT1Whar3qgOqCmrZaqFq+WpvaQ3WCOkM9Xr1cvUd9VkNFw08jT6NF454mXpOhmai5V7NPc15LWytca6tWp9aUtpy2l3audov2Ax2yjoPOGp0GnVu6GF2GbrLuPt0berCehV6iXo3edX1Y31Kfq79Pf9AAbWBtwDNoMBgxJBk6GWYathiOGdGMfI3yjTqNnhtrGEcZ7zLuM/5oYmGSYtJoct9UxtTbNN+02/R3Mz0zllmN2S1zsrm7+QbzLvMXy/SXcZbtX3bHgmLhZ7HVosfig6WVJd+y1XLaSsMq1qrWaoRBZQQwShiXrdHWztYbrE9Zv7WxtBHYHLf5zdbQNtn2iO3Ucu3lnOWNy8ft1OyYdvV2o/Z0+1j7A/ajDqoOTIcGh8eO6o5sxybHSSddpySno07PnU2c+c7tzvMuNi7rXM65Iq4erkWuA24ybqFu1W6P3NXcE9xb3Gc9LDzWepzzRHv6eO7yHPFS8mJ5NXvNelt5r/Pu9SH5BPtU+zz21fPl+3b7wX7efrv9HqzQXMFb0ekP/L38d/s/DNAOWBPwYyAmMCCwJvBJkGlQXlBfMCU4JvhI8OsQ55DSkPuhOqHC0J4wybDosOaw+XDX8LLw0QjjiHUR1yIVIrmRXVHYqLCopqi5lW4r96yciLaILoweXqW9KnvVldUKq1NWn46RjGHGnIhFx4bHHol9z/RnNjDn4rziauNmWS6svaxnbEd2OXuaY8cp40zG28WXxU8l2CXsTphOdEisSJzhunCruS+SPJPqkuaT/ZMPJX9KCU9pS8Wlxqae5Mnwknm9acpp2WmD6frphemja2zW7Fkzy/fhN2VAGasyugRU0c9Uv1BHuEU4lmmfWZP5Jiss60S2dDYvuz9HL2d7zmSue+63a1FrWWt78lTzNuWNrXNaV78eWh+3vmeD+oaCDRMbPTYe3kTYlLzpp3yT/LL8V5vDN3cXKBVsLBjf4rGlpVCikF84stV2a9021DbutoHt5turtn8sYhddLTYprih+X8IqufqN6TeV33zaEb9joNSydP9OzE7ezuFdDrsOl0mX5ZaN7/bb3VFOLy8qf7UnZs+VimUVdXsJe4V7Ryt9K7uqNKp2Vr2vTqy+XeNc01arWLu9dn4fe9/Qfsf9rXVKdcV17w5wD9yp96jvaNBqqDiIOZh58EljWGPft4xvm5sUmoqbPhziHRo9HHS4t9mqufmI4pHSFrhF2DJ9NProje9cv+tqNWytb6O1FR8Dx4THnn4f+/3wcZ/jPScYJ1p/0Pyhtp3SXtQBdeR0zHYmdo52RXYNnvQ+2dNt293+o9GPh06pnqo5LXu69AzhTMGZT2dzz86dSz83cz7h/HhPTM/9CxEXbvUG9g5c9Ll4+ZL7pQt9Tn1nL9tdPnXF5srJq4yrndcsr3X0W/S3/2TxU/uA5UDHdavrXTesb3QPLh88M+QwdP6m681Lt7xuXbu94vbgcOjwnZHokdE77DtTd1PuvriXeW/h/sYH6AdFD6UeVjxSfNTws+7PbaOWo6fHXMf6Hwc/vj/OGn/2S8Yv7ycKnpCfVEyqTDZPmU2dmnafvvF05dOJZ+nPFmYKf5X+tfa5zvMffnP8rX82YnbiBf/Fp99LXsq/PPRq2aueuYC5R69TXy/MF72Rf3P4LeNt37vwd5MLWe+x7ys/6H7o/ujz8cGn1E+f/gUDmPP8usTo0wAAAAlwSFlzAAAOwwAADsMBx2+oZAAAAulJREFUeF7t289qE1EUx/GufBoRfQNfwYVP4M6FSMFlQXDln507EXQhQrG6EYVuuhBMQJQUixVr0aDYxlabmqbN1p8cGcKv6c3cO5OZc4YDH0J/JIvLt2nuomRubr7lQng7wtsR3o7wdoS3I7wd4e0Ib0d4O8LbEd6O8HaEtyO8HeHtCG9HeDvC2xHejvCOdP7u2qXHG2pdXvyMRzpzHN6RHr7eGh2MlKMzx+Ed6VH7f6De7rC98bv1SYvu9kBFIHkH9fuHeFx8s33qWpteUIsbL7s4z+FATaB7r35s7QyVNJI6wz9HV5Y2tQTCmc7dWdXQKKtz4f766ZsdRYHwc+2NxutgqgsEWaMnb3sVN6I6oDEQ1NLoeB1QGgjO3OpU2WhiHdAbCCprdFIdUB0IKmgUqAPaA8FMG11/8TVQBwwEghk1mloHbASC0hvlqQNmAkGJjXLWAUuBoJRG+euAsUBQsFFUHbAXCJIbxdYBk4EgoVFCHbAaCKIapdUBw4EgZ6PkOmA7EExtVKQOmA8EgUYF60ATAsHERsXrQEMCATUqpQ40JxBkjd5390upA40KBGi0t/fv/2tHg9HFBx/p2QRNCyR/WSJ89+fUqEDZ587Vp5vhuz+/5gSiT+XA3R+lIYEm3lmlNGpCoMCNjkbfewdFGpkPFKgjCjayHWjh+ZQ6okgjw4Fy1hHJjawGiqoj0hqZDJRQRyQ0shcouY6IbWQsUME6IqqRpUCl1BH5G5kJVGIdkbORjUCl1xF5GhkINKM6Ymoj7YFmWkeEG6kOVEEdEWikN1BldcRJjZQGqriOmNhIY6Ba6ojjjdQFqrGOoEa6AtVeR4w3Ont7VUuglQ+/NNQRWaPltV0tgfTUEVmj+gM9e/dTDrH+bZ++FFmvzpc+fmf1B8reQZrRmePwjqT8a+GCzhyHtyO8HeHtCG9HeDvC2xHejvB2hLcjvB3h7QhvR3g7wtsR3o7wdoS3I7wd4e0IbzduvvUXj3apOnmPgDQAAAAASUVORK5CYII=";
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream imageStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            return CreatePackage(identifier, identifier, title, launchUrl, imageStream);
        }

        public static Stream CreatePackage(Guid productId, Guid identifier, string title, string launchUrl, MemoryStream imageStream) {
            MemoryStream stream = new MemoryStream();
            using (Package package = Package.Open(stream, FileMode.Create)) {
                PackagePart manifest = package.CreateAppManifest(productId, identifier, title, launchUrl);
                PackagePart icon = package.CreateAppIcon(imageStream);
                PackagePart iconConfig = package.CreateAppIconConfig();
                package.CreateRelationship(manifest.Uri, TargetMode.Internal,
                    "http://schemas.microsoft.com/sharepoint/2012/app/relationships/package-manifest");
                manifest.CreateRelationship(icon.Uri, TargetMode.Internal,
                    "http://schemas.microsoft.com/sharepoint/2012/app/relationships/manifest-icon");
                icon.CreateRelationship(iconConfig.Uri, TargetMode.Internal,
                    "http://schemas.microsoft.com/sharepoint/2012/app/relationships/partconfiguration");
            }
            return stream;
        }

        public static Stream UpgradePackage(Stream stream, string title, string launchUrl) {
            using (Package package = Package.Open(stream, FileMode.Open)) {
                package.UpgradeAppManifest(title, launchUrl);
            }
            return stream;
        }

        public static string ParseAppLaunchUrl(Stream stream) {
            using (Package package = Package.Open(stream, FileMode.Open)) {
                string identifier;
                string title;
                string launchUrl;
                package.ParseManifest(out identifier, out title, out launchUrl);
                return launchUrl;
            }
        }

        public static SPAppPrincipal RegisterPackage(Stream stream, SPWeb web) {
            using (Package package = Package.Open(stream, FileMode.Open)) {
                string identifier;
                string title;
                string launchUrl;
                package.ParseManifest(out identifier, out title, out launchUrl);
                return AppRegister(identifier, title, launchUrl, web);
            }
        }
        
        static SPAppPrincipal AppRegister(string identifier, string title, string launchUrl, SPWeb web) {
            List<string> endpointAuthorities = new List<string>() {
                "http://" + new Uri(launchUrl).Host
            };
            SecureString symmetricKey = GenerateSecret(32);
            SPAppPrincipalCredential credential = SPAppPrincipalCredential.CreateFromSymmetricKey(symmetricKey, DateTime.UtcNow, DateTime.UtcNow.AddYears(10));
            SPExternalAppPrincipalCreationParameters creationParameters = new SPExternalAppPrincipalCreationParameters(identifier, title, endpointAuthorities, credential);
            return SPAppPrincipalManager.GetManager(web).CreateAppPrincipal(creationParameters);
        }

        static SecureString GenerateSecret(int length) {
            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create()) {
                byte[] array = new byte[length];
                randomNumberGenerator.GetBytes(array);
                string strIn = System.Convert.ToBase64String(array);
                if (strIn == null) {
                    return null;
                }
                SecureString secureString = new SecureString();
                for (int i = 0; i < strIn.Length; i++) {
                    char c = strIn[i];
                    secureString.AppendChar(c);
                }
                secureString.MakeReadOnly();
                return secureString;
            }
        }
    }
}