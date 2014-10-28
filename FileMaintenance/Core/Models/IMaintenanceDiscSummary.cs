
namespace FileMaintenance.Core.Models
{
    public interface IMaintenanceDiskSummary
    {
        /// <summary>
        /// Gets the disk label.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the deleted file count.
        /// </summary>
        int DeletedFileCount { get; }

        /// <summary>
        /// Gets the space freed or used during the maintenance.
        /// </summary>
        long SpaceFreed { get; }

        /// <summary>
        /// Gets the free disk space in bytes.
        /// </summary>
        long FreeDiskSpace { get; }

        /// <summary>
        /// Gets the total disk space in bytes.
        /// </summary>
        long TotalDiskSize { get; }

        /// <summary>
        /// Gets the percentage of free disk space.
        /// </summary>
        string FreeDiskSpacePct { get; }

        /// <summary>
        /// Increments the deleted file count.
        /// </summary>
        void IncrementDeletedFileCount();
    }
}
