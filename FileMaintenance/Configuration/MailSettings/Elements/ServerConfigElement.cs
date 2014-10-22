using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of ServerConfigElement in custom config section FileMaintenance
    /// </summary>
    public class ServerConfigElement : ConfigurationElement
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
        /// Gets or sets Port property
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = 587)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        /// <summary>
        /// Gets or sets Ssl property
        /// </summary>
        [ConfigurationProperty("ssl", DefaultValue = false)]
        public bool Ssl
        {
            get { return (bool)base["ssl"]; }
            set { base["ssl"] = value; }
        }

        /// <summary>
        /// Gets or sets Timeout property
        /// </summary>
        [ConfigurationProperty("timeout", DefaultValue = 100000)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }
    }
}
