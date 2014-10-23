using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of MaintenanceItemConfigElementCollection in custom config section FileMaintenance
    /// </summary>
    [ConfigurationCollection(typeof(MaintenanceItemConfigElement), AddItemName = "item", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MaintenanceItemConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a MaintenanceItemConfigElement by Name
        /// </summary>
        public MaintenanceItemConfigElement this[string name]
        {
            get { return (MaintenanceItemConfigElement)base.BaseGet(name); }
        }

        /// <summary>
        /// Gets a MaintenanceItemConfigElement by Index
        /// </summary>
        public MaintenanceItemConfigElement this[int index]
        {
            get { return (MaintenanceItemConfigElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new MaintenanceItemConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MaintenanceItemConfigElement();
        }

        /// <summary>
        /// Gets the key of a single MaintenanceItemConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MaintenanceItemConfigElement)element).Name;
        }
    }
}
