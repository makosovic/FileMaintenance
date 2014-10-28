using System;
using System.Linq;
using System.Net.Mail;
using FileMaintenance.Core.Helpers;

namespace FileMaintenance.Services
{
    public class MailService : INotificationService
    {

        #region private fields

        private readonly IMailServiceConfig _mailServiceConfig;

        #endregion

        #region constructors

        /// <summary>
        /// Gets an instance of mailing service configured with given MailServiceConfig class.
        /// </summary>
        /// <param name="mailServiceConfig"></param>
        public MailService(IMailServiceConfig mailServiceConfig)
        {
            _mailServiceConfig = mailServiceConfig;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Send an email message.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void Send(string title, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = title;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.Cc.Any()) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.Bcc.Any()) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            _mailServiceConfig.SmtpClient.Send(mailMessage);
        }

        /// <summary>
        /// Send a message asynchronously.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public async void SendAsync(string title, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();


            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = title;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.Cc.Any()) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.Bcc.Any()) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            await _mailServiceConfig.SmtpClient.SendMailAsync(mailMessage);
        }

        #endregion

    }
}
