using System;
using System.IO;

namespace FileMaintenance.Core.Models
{
    public interface IFileMaintenanceBuilder<out T> where T : BaseMaintenanceItem
    {
        IFileMaintenanceBuilder<T> Where(Func<FileInfo, bool> expression);
        IFileMaintenanceBuilder<T> Delete(Action<string> action);
        IFileMaintenanceBuilder<T> Backup(Action<string, string> action);
        void Execute();
    }
}