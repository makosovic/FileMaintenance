using System.Collections.Generic;
using System.Net.Mail;

namespace FileMaintenance.Services
{
    public interface IMailServiceConfig
    {
        SmtpClient SmtpClient { get; }

        string From { get; }

        IEnumerable<string> Recipients { get; }
        IEnumerable<string> Cc { get; }
        IEnumerable<string> Bcc { get; }
    }
}
