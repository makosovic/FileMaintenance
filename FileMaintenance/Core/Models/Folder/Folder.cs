using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileMaintenance.Services;

namespace FileMaintenance.Core.Models
{
    public class Folder : BaseFolder, IBackupable
    {

        #region private fields

        private readonly IList<BackupFolder> _backups;

        #endregion

        #region properties

        public bool IsBackedUp
        {
            get { return _backups != null && _backups.Any(); }
        }
        public IReadOnlyCollection<BackupFolder> Backups
        {
            get { return new ReadOnlyCollection<BackupFolder>(_backups); }
        }

        #endregion

        #region constructors

        public Folder(string path, TimeSpan keepFor)
            : base(path, keepFor)
        {
            _backups = new List<BackupFolder>();
        }

        public Folder(string path, TimeSpan keepFor, BackupFolder backup)
            : base(path, keepFor)
        {
            _backups = new List<BackupFolder>();
            _backups.Add(backup);
        }

        #endregion

        #region public methods

        public override void ExecuteMaintenance(IMaintenanceServiceAction maintenanceService)
        {
            if (IsBackedUp)
            {
                this.CreateMaintenance()
                    .Where(file => DateTime.UtcNow.Subtract(this.KeepFor) > file.LastWriteTimeUtc)
                    .Backup((sourceDirectoryPath, backupDirectoryPath) => maintenanceService.Backup(sourceDirectoryPath, backupDirectoryPath))
                    .Delete(filePath => maintenanceService.Delete(filePath))
                    .Execute();
            }
            else
            {
                base.ExecuteMaintenance(maintenanceService);
            }
        }

        public void AddBackup(BackupFolder backupFolder)
        {
            this._backups.Add(backupFolder);
        }

        public void AddBackup(string path, TimeSpan keepFor)
        {
            this._backups.Add(new BackupFolder(path, keepFor));
        }

        #endregion

    }
}
