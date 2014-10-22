using System;
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

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.CcCount != 0) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.BccCount != 0) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            _mailServiceConfig.SmtpClient.Send(mailMessage);
        }

        public async void SendAsync(string subject, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException();


            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_mailServiceConfig.From);

            mailMessage.Subject = subject;
            mailMessage.Body = message;

            mailMessage.To.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Recipients));
            if (_mailServiceConfig.CcCount != 0) mailMessage.CC.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Cc));
            if (_mailServiceConfig.BccCount != 0) mailMessage.Bcc.Add(MailMessageHelper.AggregateEmails(_mailServiceConfig.Bcc));

            await _mailServiceConfig.SmtpClient.SendMailAsync(mailMessage);
        }

        #endregion

    }
}
