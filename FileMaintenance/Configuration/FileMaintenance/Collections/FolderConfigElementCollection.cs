using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of FolderConfigElementCollection in custom config section FileMaintenance
    /// </summary>
    [ConfigurationCollection(typeof(FolderConfigElement), AddItemName = "folder", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class FolderConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a FolderConfigElement by Name
        /// </summary>
        public FolderConfigElement this[string name]
        {
            get { return (FolderConfigElement)base.BaseGet(name); }
        }

        /// <summary>
        /// Gets a FolderConfigElement by Index
        /// </summary>
        public FolderConfigElement this[int index]
        {
            get { return (FolderConfigElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new FolderConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderConfigElement();
        }

        /// <summary>
        /// Gets the key of a single FolderConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderConfigElement)element).Name;
        }
    }
}
