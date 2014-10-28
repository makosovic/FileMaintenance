using System.Collections.Generic;
using System.Net.Mail;

namespace FileMaintenance.Services
{
    public interface IMailServiceConfig
    {
        /// <summary>
        /// Gets the configured smtp client.
        /// </summary>
        SmtpClient SmtpClient { get; }

        /// <summary>
        /// Gets the email address message is being sent from.
        /// </summary>
        string From { get; }

        /// <summary>
        /// Gets the collection of recipients.
        /// </summary>
        IEnumerable<string> Recipients { get; }

        /// <summary>
        /// Gets the collection of Cc.
        /// </summary>
        IEnumerable<string> Cc { get; }

        /// <summary>
        /// Gets the collection of Bcc.
        /// </summary>
        IEnumerable<string> Bcc { get; }
    }
}
