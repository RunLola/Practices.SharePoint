namespace Practices.SharePoint.Configuration {
    /// <summary>
    /// The Interface that represents a property bag (stores name-value pairs) for holding
    /// configuration information for a particular level like a web, site, web application, and farm. 
    /// </summary>
    public interface IPropertyBag {
        /// <summary>
        /// The config scope this PropertyBag represents. 
        /// </summary>
        ConfigScope Scope { get; }

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag.
        /// </summary>
        /// <param name="key">The key to find the config value in the config. </param>
        /// <returns>The config value defined in the property bag, null if not found </returns>
        string this[string key] { get; set; }
    }
}