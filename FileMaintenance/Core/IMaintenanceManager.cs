using System.Collections;
using System.Collections.ObjectModel;


namespace FileMaintenance.Core
{
    public interface IMaintenanceManager
    {
        void Backup(string sourcePath, string targetFilePath);
        void Delete(string path);

        IEnumerable<string> Traverse(string path);
    }
}