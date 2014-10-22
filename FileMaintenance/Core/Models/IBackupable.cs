using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IBackupable
    {
        IReadOnlyCollection<BackupFolder> Backups { get; }
        bool IsBackedUp { get; }
        string Path { get; }

        void AddBackup(BackupFolder backupFolder);
        void AddBackup(string path, TimeSpan keepFor);
    }
}
