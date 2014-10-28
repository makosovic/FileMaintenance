using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using FileMaintenance.Configuration;

namespace FileMaintenance.Services
{
    public class MailServiceConfig : IMailServiceConfig
    {

        #region private fields

        private readonly SmtpClient _smtpClient;

        private readonly string _from;

        private readonly ICollection<string> _recipients;
        private readonly ICollection<string> _cc;
        private readonly ICollection<string> _bcc;

        #endregion

        #region properties

        public SmtpClient SmtpClient { get { return _smtpClient; } }

        public string From { get { return _from; } }

        public IEnumerable<string> Recipients { get { return _recipients.AsEnumerable(); } }
        public IEnumerable<string> Cc { get { return _cc.AsEnumerable(); } }
        public IEnumerable<string> Bcc { get { return _bcc.AsEnumerable(); } }

        #endregion

        #region constructors

        public MailServiceConfig()
        {
            MailSettingsConfigSection mailConfig = ConfigurationManager.GetSection("mailSettings") as MailSettingsConfigSection;

            if (mailConfig == null)
                throw new ArgumentNullException("mailSettings");

            ServerConfigElement server = mailConfig.Server;
            CredentialsConfigElement credentials = mailConfig.Credentials;
            EmailConfigElement from = mailConfig.From;
            EmailConfigElementCollection recipients = mailConfig.Recipients;
            EmailConfigElementCollection cc = mailConfig.Cc;
            EmailConfigElementCollection bcc = mailConfig.Bcc;

            if (server == null)
                throw new ArgumentNullException("server");

            if (credentials == null)
                throw new ArgumentNullException("credentials");

            if (from == null)
                throw new ArgumentNullException("credentials");


            _smtpClient = new SmtpClient(server.Name, server.Port);
            _smtpClient.EnableSsl = server.Ssl;
            _smtpClient.Timeout = server.Timeout;
            _smtpClient.Credentials = new NetworkCredential(credentials.Username, credentials.Password);

            _from = from.Email;
            _recipients = new List<string>();
            _cc = new List<string>();
            _bcc = new List<string>();



            if (recipients == null)
                throw new ArgumentNullException("recipients");

            if (recipients.Count == 0)
                throw new ArgumentOutOfRangeException("recipients");

            foreach (EmailConfigElement recipient in recipients)
            {
                _recipients.Add(recipient.Email);
            }



            if (cc == null)
                throw new ArgumentNullException("cc");

            foreach (EmailConfigElement item in cc)
            {
                _cc.Add(item.Email);
            }



            if (bcc == null)
                throw new ArgumentNullException("bcc");

            foreach (EmailConfigElement item in bcc)
            {
                _bcc.Add(item.Email);
            }

        }

        #endregion

    }
}
