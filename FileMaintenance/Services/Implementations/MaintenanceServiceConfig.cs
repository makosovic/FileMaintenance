using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FileMaintenance.Core.Helpers;
using FileMaintenance.Configuration;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public class MaintenanceServiceConfig : IMaintenanceServiceConfig
    {
        #region private fields

        private readonly ICollection<BaseFolder> _folders; 

        #endregion

        #region properties

        public bool AlertLowDisk { get; private set; }
        public bool AlertSummary { get; private set; }

        public IEnumerable<BaseFolder> Folders { get { return _folders.AsEnumerable(); } }

        #endregion

        #region constructors

        public MaintenanceServiceConfig()
        {
            _folders = new List<BaseFolder>();

            InitFoldersFromConfig();
        }

        #endregion

        #region private methods

        private void InitFoldersFromConfig()
        {
            var config = ConfigurationManager.GetSection("fileMaintenance") as FileMaintenanceConfigSection;

            if (config == null)
                throw new ArgumentNullException("fileMaintenance");

            AlertSummary = config.Summary;
            FolderConfigElementCollection folders = config.Folders;
            AlertConfigElementCollection alerts = config.Alerts;


            if (folders == null)
                throw new ArgumentNullException("folders");

            if (folders.Count == 0)
                throw new ArgumentOutOfRangeException("folders");

            foreach (FolderConfigElement folder in folders)
            {
                int days, hours, minutes;
                if (!ConfigurationHelper.TryParseKeepFor(folder.KeepFor, out days, out hours, out minutes))
                    throw new ArgumentOutOfRangeException("keepFor");

                Folder folderItem = new Folder(folder.Path, new TimeSpan(days, hours, minutes, 0));
                BackupFolderConfigElementCollection backups = folder.Backups;

                if (backups == null)
                    throw new ArgumentNullException("backups");

                if (backups.Count == 0)
                    throw new ArgumentOutOfRangeException("backups");

                foreach (BackupFolderConfigElement backup in backups)
                {
                    int bDays, bHours, bMinutes;
                    if (!ConfigurationHelper.TryParseKeepFor(folder.KeepFor, out bDays, out bHours, out bMinutes))
                        throw new ArgumentOutOfRangeException("keepFor");

                    BackupFolder backupFolderItem = new BackupFolder(backup.Path, new TimeSpan(bDays, bHours, bMinutes, 0));
                    folderItem.AddBackup(backupFolderItem);
                    _folders.Add(backupFolderItem);

                }

                _folders.Add(folderItem);
            }

            if (alerts != null)
            {
                foreach (AlertConfigElement alert in alerts)
                {
                    switch (alert.When)
                    {
                        case "LowDisk":
                            AlertLowDisk = true;
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
