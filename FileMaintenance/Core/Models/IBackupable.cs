using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IBackupable
    {
        /// <summary>
        /// Gets the read only collection of backups set for a maintenance item.
        /// </summary>
        IReadOnlyCollection<MaintenanceItemBackup> Backups { get; }

        /// <summary>
        /// Gets wheather maintenance item is being backed up.
        /// </summary>
        bool IsBackedUp { get; }

        /// <summary>
        /// Gets the path of maintenance item.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Adds a backup file.
        /// </summary>
        /// <param name="maintenanceItemBackup"></param>
        void AddBackup(MaintenanceItemBackup maintenanceItemBackup);

        /// <summary>
        /// Adds a backup file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        void AddBackup(string path, TimeSpan keepFor);
    }
}
