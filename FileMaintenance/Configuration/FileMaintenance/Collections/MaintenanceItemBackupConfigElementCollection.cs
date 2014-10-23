using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of MaintenanceItemBackupConfigElementCollection in custom config section FileMaintenance
    /// </summary>
    [ConfigurationCollection(typeof(MaintenanceItemBackupConfigElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MaintenanceItemBackupConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a MaintenanceItemBackupConfigElement by Name
        /// </summary>
        public MaintenanceItemBackupConfigElement this[string name]
        {
            get { return (MaintenanceItemBackupConfigElement)base.BaseGet(name); }
        }

        /// <summary>
        /// Gets a MaintenanceItemBackupConfigElement by Index
        /// </summary>
        public MaintenanceItemBackupConfigElement this[int index]
        {
            get { return (MaintenanceItemBackupConfigElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new MaintenanceItemBackupConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MaintenanceItemBackupConfigElement();
        }

        /// <summary>
        /// Gets the key of a single MaintenanceItemBackupConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MaintenanceItemBackupConfigElement)element).Name;
        }
    }
}
