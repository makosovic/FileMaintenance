using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of MaintenanceItemBackupConfigElement in custom config section FileMaintenance
    /// </summary>
    public class AlertConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets When property
        /// </summary>
        [ConfigurationProperty("when", IsKey = true, IsRequired = true)]
        public string When
        {
            get { return (string)base["when"]; }
            set { base["when"] = value; }
        }
    }
}
