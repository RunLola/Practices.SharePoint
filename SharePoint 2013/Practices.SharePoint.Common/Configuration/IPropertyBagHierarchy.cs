namespace Practices.SharePoint.Configuration {
    using System.Collections.Generic;

    /// <summary>
    /// Represents an interface for a container of property bags.  The container
    /// is responsible for ordering the property bags as well in hierarchical 
    /// sequence.  The first bag in the PropertyBags is the lowest in the hierachy.
    /// </summary>
    public interface IPropertyBagHierarchy {
        /// <summary>
        /// Gets the enumeration for the property bags in the hierarchy, in order of lowest to 
        /// highest.  The number and types of property bags available will depend upon the context.
        /// </summary>
        IEnumerable<IPropertyBag> PropertyBags { get; }

        /// <summary>
        /// Retrieves the property bag for a specific level in the hierarchy.
        /// </summary>
        /// <param name="scope">The level of the property bag to get</param>
        /// <returns>The property bag for the level, null if the property bag is not available.</returns>
        IPropertyBag GetPropertyBag(ConfigScope scope);
    }
}
