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

        /// <summary>
        /// Gets wheather the file has a backup or not.
        /// </summary>
        public bool IsBackedUp
        {
            get { return _backups != null && _backups.Any(); }
        }

        /// <summary>
        /// Gets a collection of backups set up for this file.
        /// </summary>
        public IReadOnlyCollection<MaintenanceItemBackup> Backups
        {
            get { return new ReadOnlyCollection<MaintenanceItemBackup>(_backups); }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Instantiate a maintenance item at a given path for a specified amount of time.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        public MaintenanceItem(string path, TimeSpan keepFor)
            : base(path, keepFor)
        {
            _backups = new List<MaintenanceItemBackup>();
        }

        /// <summary>
        /// Instantiate a maintenance item at a given path, to be kept for a specified amount of time, with a backup.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        /// <param name="backup"></param>
        public MaintenanceItem(string path, TimeSpan keepFor, MaintenanceItemBackup backup)
            : base(path, keepFor)
        {
            _backups = new List<MaintenanceItemBackup>();
            _backups.Add(backup);
        }

        #endregion

        #region public methods

        /// <summary>
        /// Method executed in maintenance service.
        /// </summary>
        /// <param name="maintenanceManager"></param>
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

        /// <summary>
        /// Add a backup file to this maintenance item.
        /// </summary>
        /// <param name="maintenanceItemBackup"></param>
        public void AddBackup(MaintenanceItemBackup maintenanceItemBackup)
        {
            this._backups.Add(maintenanceItemBackup);
        }

        /// <summary>
        /// Add a backup file to this maintenance item.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        public void AddBackup(string path, TimeSpan keepFor)
        {
            this._backups.Add(new MaintenanceItemBackup(path, keepFor));
        }

        #endregion

    }
}
