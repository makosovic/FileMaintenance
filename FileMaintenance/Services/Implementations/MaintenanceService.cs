using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FileMaintenance.Core.Models;
using FileMaintenance.Properties;

namespace FileMaintenance.Services
{
    public class MaintenanceService : IMaintenanceService, IMaintenanceServiceAction
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

        public void Backup(string sourcePath, string targetFilePath)
        {
            FileInfo sourceFi = new FileInfo(sourcePath);
            FileInfo targetFi = new FileInfo(targetFilePath);

            if (!string.Equals(targetFi.Extension, ".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("targetPath");
            }

            if (targetFi.Exists)
            {
                throw new Exception("Target path already exists.");
            }

            if (targetFi.DirectoryName != null)
            {
                if (!Directory.Exists(targetFi.DirectoryName))
                {
                    Directory.CreateDirectory(targetFi.DirectoryName);
                }
            }

            if ((sourceFi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {

                ZipFile.CreateFromDirectory(sourcePath, targetFilePath);
                Directory.Delete(sourcePath, true);
            }
            else
            {
                using (var zip = ZipFile.Open(targetFilePath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(sourceFi.FullName, sourceFi.Name);
                }

                File.Delete(sourcePath);
            }
        }

        public void Delete(string path)
        {
            File.Delete(path);
            _maintenanceSummary.IncrementDeletedFileCount(path);
        }

        public void Start()
        {
            _maintenanceSummary.ExecutionStartTimeUtc = DateTime.UtcNow;

            foreach (BaseMaintenanceItem item in _maintenanceServiceConfig.MaintenanceItems)
            {
                if (Directory.Exists(item.Path))
                {
                    item.ExecuteMaintenance(this);
                }
                else if ((item as IBackupable) != null)
                {
                    throw new ApplicationException("Invalid backup path");
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
                    notificationService.Send(Resources.NotificationService_SummaryMessage_Subject, _maintenanceSummary.ToString());
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
