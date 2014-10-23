using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of AlertConfigElementCollection in custom config section FileMaintenance
    /// </summary>
    [ConfigurationCollection(typeof(AlertConfigElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AlertConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a AlertConfigElement by Name
        /// </summary>
        public AlertConfigElement this[string name]
        {
            get { return (AlertConfigElement)base.BaseGet(name); }
        }

        /// <summary>
        /// Gets a AlertConfigElement by Index
        /// </summary>
        public AlertConfigElement this[int index]
        {
            get { return (AlertConfigElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new AlertConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AlertConfigElement();
        }

        /// <summary>
        /// Gets the key of a single AlertConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AlertConfigElement)element).When;
        }
    }
}
