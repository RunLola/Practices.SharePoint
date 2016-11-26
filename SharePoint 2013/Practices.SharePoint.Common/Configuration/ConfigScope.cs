namespace Practices.SharePoint.Configuration {
    /// <summary>
    /// The levels at which configuration information can be stored. These levels are used to determine if a specific config value
    /// can be stored at a specific level.
    /// </summary>
    public enum ConfigScope {
        /// <summary>
        /// Store config information in the SPFarm property bag
        /// </summary>
        Farm = 0,

        /// <summary>
        /// Store config information in the SPWebApplication property bag
        /// </summary>
        WebApplication = 1,

        /// <summary>
        /// Store config information in the SPSite property bag
        /// </summary>
        Site = 2,

        /// <summary>
        /// Store config information in the SPWeb property bag
        /// </summary>
        Web = 3,

        /// <summary>
        /// Store config information in the SPWeb property bag
        /// </summary>
        List = 4,
    }
}