using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of EmailConfigElement in custom config section FileMaintenance
    /// </summary>
    public class EmailConfigElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets Email property
        /// </summary>
        [ConfigurationProperty("email", IsKey = true, DefaultValue = "test@test.com", IsRequired = true)]
        [RegexStringValidator("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$")]
        public string Email
        {
            get { return (string) base["email"]; }
            set { base["email"] = value; }
        }
    }
}
