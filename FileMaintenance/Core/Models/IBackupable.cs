using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IBackupable
    {
        IReadOnlyCollection<MaintenanceItemBackup> Backups { get; }
        bool IsBackedUp { get; }
        string Path { get; }

        void AddBackup(MaintenanceItemBackup maintenanceItemBackup);
        void AddBackup(string path, TimeSpan keepFor);
    }
}
