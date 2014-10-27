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

        public MailService(IMailServiceConfig mailServiceConfig)
        {
            _mailServiceConfig = mailServiceConfig;
        }

        #endregion

        #region public methods

        public void Send(string subject, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.Cc.Any()) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.Bcc.Any()) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            _mailServiceConfig.SmtpClient.Send(mailMessage);
        }

        public async void SendAsync(string subject, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();


            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.Cc.Any()) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.Bcc.Any()) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            await _mailServiceConfig.SmtpClient.SendMailAsync(mailMessage);
        }

        #endregion

    }
}
