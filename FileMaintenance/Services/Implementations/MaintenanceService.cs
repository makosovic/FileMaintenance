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

        #region constructors

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

        public MaintenanceService(IEnumerable<INotificationService> notificationServices)
        {
            _notificationServices = notificationServices;
            _maintenanceServiceConfig = new MaintenanceServiceConfig();
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        public MaintenanceService(IEnumerable<INotificationService> notificationServices, IMaintenanceServiceConfig maintenanceServiceConfig)
        {
            _notificationServices = notificationServices;
            _maintenanceServiceConfig = maintenanceServiceConfig;
            _maintenanceSummary = new MaintenanceSummary(_maintenanceServiceConfig.MaintenanceItems.Select(x => x.Path));
        }

        #endregion

        #region public methods

        public void Start()
        {
            _maintenanceSummary.ExecutionStartTimeUtc = DateTime.UtcNow;

            foreach (BaseMaintenanceItem item in _maintenanceServiceConfig.MaintenanceItems)
            {
                if (Directory.Exists(item.Path))
                {
                    IMaintenanceManager maintenanceManager = new MaintenanceManager(_maintenanceSummary, item.Path);
                    item.ExecuteMaintenance(maintenanceManager);
                }
                else if ((item as IBackupable) != null)
                {
                    throw new ApplicationException("Invalid maintenance item path");
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
                foreach (var notificationService in _notificationServices)
                {
                    var templateService = new TemplateService();
                    var emailHtmlBody = templateService.Parse(File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["EmailTemplate.Path"]), _maintenanceSummary, null, null);
                    notificationService.Send(Resources.NotificationService_SummaryMessage_Subject, emailHtmlBody);
                }
            }

            if (_maintenanceServiceConfig.AlertLowDisk && _maintenanceSummary.IsAnyDiskLow == true)
            {
                foreach (var notificationService in _notificationServices)
                {
                    notificationService.Send(Resources.NotificationService_AlertLowDiskMessage_Subject, _maintenanceSummary.GetDiskSpaceReport());
                }
            }
        }

        #endregion

    }
}
