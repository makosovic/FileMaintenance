using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public override void ExecuteMaintenance(IMaintenanceManager maintenanceManager)
        {
            maintenanceManager.AddCondition(file => DateTime.UtcNow.Subtract(this.KeepFor) > file.LastWriteTimeUtc);

            if (maintenanceManager.Files.Any())
            {
                string sourcePath = maintenanceManager.GroupFilesInNewDirectory();
                string sourceRelativePath = sourcePath.Replace(Path + "\\", "");

                foreach (MaintenanceItemBackup backup in Backups)
                {
                    maintenanceManager.Backup(sourcePath, System.IO.Path.Combine(backup.Path, sourceRelativePath) + ".zip");
                }

                foreach (string file in maintenanceManager.Files)
                {
                    maintenanceManager.Delete(file);
                }
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
