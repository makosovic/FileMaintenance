using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of FileMaintenanceConfigSection in custom config section FileMaintenance
    /// </summary>
    public class FileMaintenanceConfigSection : ConfigurationSection
    {
        /// <summary>
        /// Gets a collection of FolderConfigElement
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public FolderConfigElementCollection Folders
        {
            get { return (FolderConfigElementCollection)this[""]; }
        }

        /// <summary>
        /// Gets or sets Summary property
        /// </summary>
        [ConfigurationProperty("summary", DefaultValue = true)]
        public bool Summary
        {
            get { return (bool)base["summary"]; }
            set { base["summary"] = value; }
        }

        /// <summary>
        /// Gets a collection of BackupFolderConfigElement
        /// </summary>
        [ConfigurationProperty("alerts", IsDefaultCollection = false)]
        public AlertConfigElementCollection Alerts
        {
            get { return (AlertConfigElementCollection)base["alerts"]; }
        }

        /// <summary>
        /// Gets or sets xmlns property
        /// </summary>
        [ConfigurationProperty("xmlns")]
        public string Xmlns
        {
            get { return (string)base["xmlns"]; }
            set { base["xmlns"] = value; }
        }

        /// <summary>
        /// Gets or sets XmlnsXsi property
        /// </summary>
        [ConfigurationProperty("xmlns:xsi")]
        public string XmlnsXsi
        {
            get { return (string)base["xmlns:xsi"]; }
            set { base["xmlns:xsi"] = value; }
        }
    }
}
