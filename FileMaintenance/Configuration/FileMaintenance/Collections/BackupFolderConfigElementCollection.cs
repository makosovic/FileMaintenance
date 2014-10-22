using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of BackupFolderConfigElementCollection in custom config section FileMaintenance
    /// </summary>
    [ConfigurationCollection(typeof(BackupFolderConfigElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class BackupFolderConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a BackupFolderConfigElement by Name
        /// </summary>
        public BackupFolderConfigElement this[string name]
        {
            get { return (BackupFolderConfigElement)base.BaseGet(name); }
        }

        /// <summary>
        /// Gets a BackupFolderConfigElement by Index
        /// </summary>
        public BackupFolderConfigElement this[int index]
        {
            get { return (BackupFolderConfigElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new BackupFolderConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new BackupFolderConfigElement();
        }

        /// <summary>
        /// Gets the key of a single BackupFolderConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BackupFolderConfigElement)element).Name;
        }
    }
}
