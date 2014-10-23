using System;
using System.Collections.Generic;
using System.IO;


namespace FileMaintenance.Core
{
    public interface IMaintenanceManager
    {
        IEnumerable<string> Files { get; }

        void Backup(string sourcePath, string targetFilePath);
        void Delete(string path);
        void AddCondition(Func<FileInfo, bool> expression);
        string GroupFilesInNewDirectory();
    }
}