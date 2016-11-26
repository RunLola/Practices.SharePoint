namespace Practices.SharePoint.Configuration {

    public abstract class PropertyBag : IPropertyBag {
        /// <summary>
        /// The config scope this PropertyBag represents. 
        /// </summary>
        public abstract ConfigScope Scope { get; }

        /// <summary>
        /// The prefix that's used by the PropertyBag to differentiate key's between other settings.
        /// </summary>
        protected abstract string KeyPrefix {
            get;
        }

        /// <summary>
        /// Checks if a specific key exist in the PropertyBag. 
        /// </summary>
        /// <param name="key">the key to check.</param>
        /// <returns><c>true</c> if the key exists, else <c>false</c>.</returns>
        protected abstract bool Contains(string key);

        /// <summary>
        /// Gets or sets a value based on the key. If the value is not defined in this PropertyBag.
        /// </summary>
        /// <param name="key">The key to find the config value in the config. </param>
        /// <returns>The config value defined in the property bag, null if not found </returns>
        public abstract string this[string key] {
            get;
            set;
        }

        /// <summary>
        /// Remove a particular config setting from this property bag.
        /// </summary>
        /// <param name="key">The key to remove</param>
        public abstract void Remove(string key);
    }
}
