using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileMaintenance.Core;
using FileMaintenance.Core.Models;
using FileMaintenance.Properties;
using RazorEngine.Templating;

namespace FileMaintenance.Services
{
    public class MaintenanceService : IMaintenanceService
    {

        #region private fields

        private readonly IEnumerable<INotificationService> _notificationServices;
        private readonly IMaintenanceSummary _maintenanceSummary;
        private readonly IMaintenanceServiceConfig _maintenanceServiceConfig;

        #endregion

        #region properties

        /// <summary>
        /// Gets the maintenance summary.
        /// </summary>
        public IMaintenanceSummary MaintenanceSummary { get { return _maintenanceSummary; } }

        #endregion

        #region constructors

        /// <summary>
        /// Creates and instance of maintenance service configured with Configuration file.
        /// </summary>
        public MaintenanceService()
        {
            IMailServiceConfig mailServiceConfig = new MailServiceConfig();
            INotificationService mailService = new MailService(mailServiceConfig);
            ICollection<INotificationService> notificationServicesCollection = new Collection<INotificationService>();
            notificationServicesCollection.Add(mailService);

            _notificationServices = notificationServicesCollection;
            _maintenanceServiceConfig = new MaintenanceServiceConfig();
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        /// <summary>
        /// Creates and instance of maintenance service configured with maintenanceServiceConfig and Configuration file for mailing.
        /// </summary>
        public MaintenanceService(IMaintenanceServiceConfig maintenanceServiceConfig)
        {
            IMailServiceConfig mailServiceConfig = new MailServiceConfig();
            INotificationService mailService = new MailService(mailServiceConfig);
            ICollection<INotificationService> notificationServicesCollection = new Collection<INotificationService>();
            notificationServicesCollection.Add(mailService);

            _notificationServices = notificationServicesCollection;
            _maintenanceServiceConfig = maintenanceServiceConfig;
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        /// <summary>
        /// Creates an instance of maintenance service configured with Configuration file and with given collection of notificationServices.
        /// </summary>
        /// <param name="notificationServices"></param>
        public MaintenanceService(IEnumerable<INotificationService> notificationServices)
        {
            _notificationServices = notificationServices;
            _maintenanceServiceConfig = new MaintenanceServiceConfig();
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        /// <summary>
        /// Creates an instance of maintenance service configured with given maintenanceServiceConfig and collection of notificationServices.
        /// </summary>
        /// <param name="notificationServices"></param>
        /// <param name="maintenanceServiceConfig"></param>
        public MaintenanceService(IEnumerable<INotificationService> notificationServices, IMaintenanceServiceConfig maintenanceServiceConfig)
        {
            _notificationServices = notificationServices;
            _maintenanceServiceConfig = maintenanceServiceConfig;
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        #endregion

        #region public methods

        /// <summary>
        /// Starts the maintenance.
        /// </summary>
        public void Start()
        {
            _maintenanceSummary.ExecutionStartTimeUtc = DateTime.UtcNow;

            foreach (BaseMaintenanceItem item in _maintenanceServiceConfig.MaintenanceItems)
            {
                try
                {
                    IMaintenanceManager maintenanceManager = new MaintenanceManager(_maintenanceSummary, item.Path);
                    item.ExecuteMaintenance(maintenanceManager);
                }
                catch (Exception ex)
                {
                    _maintenanceSummary.AddError(ex.ToString());
                }
            }

            _maintenanceSummary.ExecutionEndTimeUtc = DateTime.UtcNow;
            SendAlertsAndSummary();
        }

        #endregion

        #region private methods

        private void SendAlertsAndSummary()
        {
            if (_maintenanceServiceConfig.AlertSummary)
            {
                foreach (INotificationService notificationService in _notificationServices)
                {
                    TemplateService templateService = new TemplateService();
                    string templatePath = System.Configuration.ConfigurationManager.AppSettings["EmailTemplate.Path"];
                    if (!string.IsNullOrEmpty(templatePath) && File.Exists(templatePath))
                    {
                        string emailHtmlBody = templateService.Parse(File.ReadAllText(templatePath), _maintenanceSummary, null, null);
                        notificationService.Send(Resources.NotificationService_SummaryMessage_Subject, emailHtmlBody);
                    }
                    else
                    {
                        notificationService.Send(Resources.NotificationService_SummaryMessage_Subject, _maintenanceSummary.ToString());
                    }
                }
            }

            if (_maintenanceServiceConfig.AlertLowDisk && _maintenanceSummary.IsAnyDiskLow == true)
            {
                foreach (INotificationService notificationService in _notificationServices)
                {
                    notificationService.Send(Resources.NotificationService_AlertLowDiskMessage_Subject, _maintenanceSummary.GetDiskSpaceReport());
                }
            }
        }

        #endregion

    }
}
