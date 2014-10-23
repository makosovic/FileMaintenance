using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileMaintenance.Services;

namespace FileMaintenance.Core.Models
{
    public class MaintenanceItem : BaseMaintenanceItem, IBackupable
    {

        #region private fields

        private readonly IList<MaintenanceItemBackup> _backups;

        #endregion

        #region properties

        public bool IsBackedUp
        {
            get { return _backups != null && _backups.Any(); }
        }

        public IReadOnlyCollection<MaintenanceItemBackup> Backups
        {
            get { return new ReadOnlyCollection<MaintenanceItemBackup>(_backups); }
        }

        #endregion

        #region constructors

        public MaintenanceItem(string path, TimeSpan keepFor)
            : base(path, keepFor)
        {
            _backups = new List<MaintenanceItemBackup>();
        }

        public MaintenanceItem(string path, TimeSpan keepFor, MaintenanceItemBackup backup)
            : base(path, keepFor)
        {
            _backups = new List<MaintenanceItemBackup>();
            _backups.Add(backup);
        }

        #endregion

        #region public methods

        public override void ExecuteMaintenance(IMaintenanceManager maintenanceService)
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
            }
        }

        public void AddBackup(MaintenanceItemBackup maintenanceItemBackup)
        {
            this._backups.Add(maintenanceItemBackup);
        }

        public void AddBackup(string path, TimeSpan keepFor)
        {
            this._backups.Add(new MaintenanceItemBackup(path, keepFor));
        }

        #endregion

    }
}
