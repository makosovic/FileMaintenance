using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Abstract class for FolderElement in custom config section FileMaintenance
    /// </summary>
    public abstract class BaseFolderConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets Name property
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// Gets or sets Path property
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }

        /// <summary>
        /// Gets or sets KeepFor property
        /// </summary>
        [ConfigurationProperty("keepFor", DefaultValue = "180.0:0", IsRequired = true)]
        [RegexStringValidator("^[0-9]{1,}\\.([0-9]|0[0-9]|1[0-9]|2[0-3]){0,1}[0-9]:[0-5]{0,1}[0-9]")]
        public string KeepFor
        {
            get { return (string)base["keepFor"]; }
            set { base["keepFor"] = value; }
        }
    }
}
