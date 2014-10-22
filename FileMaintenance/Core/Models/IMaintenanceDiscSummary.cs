
namespace FileMaintenance.Core.Models
{
    public interface IMaintenanceDiskSummary
    {
        string Name { get; }
        int DeletedFileCount { get; }
        long SpaceFreed { get; }
        long FreeDiskSpace { get; }
        long TotalDiskSize { get; }
        string FreeDiskSpacePct { get; }
        void IncrementDeletedFileCount();
    }
}
