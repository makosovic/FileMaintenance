using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of CredentialsConfigElement in custom config section FileMaintenance
    /// </summary>
    public class CredentialsConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets Username property
        /// </summary>
        [ConfigurationProperty("username", IsKey = true, IsRequired = true)]
        public string Username
        {
            get { return (string) base["username"]; }
            set { base["username"] = value; }
        }

        /// <summary>
        /// Gets or sets Password property
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
            
        }
    }
}
