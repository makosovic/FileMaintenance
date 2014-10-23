using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of MaintenanceItemConfigElement in custom config section FileMaintenance
    /// </summary>
    public class MaintenanceItemConfigElement : BaseMaintenanceItemConfigElement
    {
        /// <summary>
        /// Gets a collection of MaintenanceItemBackupConfigElement
        /// </summary>
        [ConfigurationProperty("backups", IsDefaultCollection = false)]
        public MaintenanceItemBackupConfigElementCollection Backups
        {
            get { return (MaintenanceItemBackupConfigElementCollection)base["backups"]; }
        }
    }
}
