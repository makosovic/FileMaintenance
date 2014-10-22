using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of FolderConfigElement in custom config section FileMaintenance
    /// </summary>
    public class FolderConfigElement : BaseFolderConfigElement
    {
        /// <summary>
        /// Gets a collection of BackupFolderConfigElement
        /// </summary>
        [ConfigurationProperty("backups", IsDefaultCollection = false)]
        public BackupFolderConfigElementCollection Backups
        {
            get { return (BackupFolderConfigElementCollection)base["backups"]; }
        }
    }
}
