using System.Configuration;

namespace FileMaintenance.Configuration
{
    public class MailSettingsConfigSection : ConfigurationSection
    {
        /// <summary>
        /// Gets an EmailConfigElement
        /// </summary>
        [ConfigurationProperty("from")]
        public EmailConfigElement From
        {
            get { return (EmailConfigElement)this["from"]; }
        }

        /// <summary>
        /// Gets an ServerConfigElement
        /// </summary>
        [ConfigurationProperty("server")]
        public ServerConfigElement Server
        {
            get { return (ServerConfigElement)this["server"]; }
        }

        /// <summary>
        /// Gets an CredentialsConfigElement
        /// </summary>
        [ConfigurationProperty("credentials")]
        public CredentialsConfigElement Credentials
        {
            get { return (CredentialsConfigElement)this["credentials"]; }
        }

        /// <summary>
        /// Gets a collection of EmailConfigElement representing Recipients
        /// </summary>
        [ConfigurationProperty("recipients", IsDefaultCollection = true)]
        public EmailConfigElementCollection Recipients
        {
            get { return (EmailConfigElementCollection)this["recipients"]; }
        }

        /// <summary>
        /// Gets a collection of EmailConfigElement representing Cc
        /// </summary>
        [ConfigurationProperty("cc", IsDefaultCollection = false)]
        public EmailConfigElementCollection Cc
        {
            get { return (EmailConfigElementCollection)this["cc"]; }
        }

        /// <summary>
        /// Gets a collection of EmailConfigElement representing Bcc
        /// </summary>
        [ConfigurationProperty("bcc", IsDefaultCollection = false)]
        public EmailConfigElementCollection Bcc
        {
            get { return (EmailConfigElementCollection)this["bcc"]; }
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
