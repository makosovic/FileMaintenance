using System.Configuration;

namespace FileMaintenance.Configuration
{
    /// <summary>
    /// Implementation of EmailConfigElementCollection in custom config section MailSettings
    /// </summary>
    public class EmailConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets a EmailConfigElement by Name
        /// </summary>
        public EmailConfigElement this[string email]
        {
            get { return (EmailConfigElement) base.BaseGet(email); }
        }

        /// <summary>
        /// Gets a EmailConfigElement by Index
        /// </summary>
        public EmailConfigElement this[int index]
        {
            get { return (EmailConfigElement) base.BaseGet(index); }
        }

        /// <summary>
        /// Creates new EmailConfigElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailConfigElement();
        }

        /// <summary>
        /// Gets the key of a single EmailConfigElement
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmailConfigElement) element).Email;
        }
    }
}
