
namespace FileMaintenance.Services
{
    public interface IMaintenanceServiceAction
    {
        void Backup(string sourcePath, string targetFilePath);
        void Delete(string path);
    }
}
