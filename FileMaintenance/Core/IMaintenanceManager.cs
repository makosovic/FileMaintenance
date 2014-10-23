using System;
using System.Collections.Generic;
using System.IO;


namespace FileMaintenance.Core
{
    public interface IMaintenanceManager
    {
        void Backup(string sourcePath, string targetFilePath);
        void Delete(string path);

        IEnumerable<string> Traverse(string path);
        void AddCondition(Func<FileInfo, bool> expression);
    }
}