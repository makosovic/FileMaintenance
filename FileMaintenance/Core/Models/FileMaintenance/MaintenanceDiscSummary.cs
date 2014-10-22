using FileMaintenance.Core.Helpers;

namespace FileMaintenance.Core.Models
{
    public class MaintenanceDiscSummary : IMaintenanceDiskSummary
    {

        #region private fields

        private readonly string _name;
        private int _deletedFileCount;
        private readonly long _startFreeDiskSpace;
        private readonly long _totalDiskSize; 

        #endregion

        #region properties

        /// <summary>
        /// Gets the disk name.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the number of deleted files on a disk.
        /// </summary>
        public int DeletedFileCount { get { return _deletedFileCount; } }

        /// <summary>
        /// Gets the amount of bytes freed with maintenance.
        /// </summary>
        /// <returns>Returns the number of bytes freed or negative amount if the free disk space is lower than before the maintenance.</returns>
        public long SpaceFreed
        {
            get
            {
                if (FreeDiskSpace != long.MinValue)
                {
                    return _startFreeDiskSpace - FreeDiskSpace;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the amount of free disk space.
        /// </summary>
        /// <returns>Number of free bytes.</returns>
        public long FreeDiskSpace { get { return IoHelper.GetTotalFreeSpace(_name); } }

        /// <summary>
        /// Gets the total disk size.
        /// </summary>
        /// <returns>Total disk size in bytes.</returns>
        public long TotalDiskSize { get { return _totalDiskSize; } }

        public string FreeDiskSpacePct
        {
            get
            {
                if (_totalDiskSize != long.MinValue && FreeDiskSpace != long.MinValue)
                {
                    return string.Format("{0:F}%", ((float) FreeDiskSpace / _totalDiskSize) * 100);
                }
                else
                {
                    return "-";
                }
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of MaintenanceDiscSummary class with a given name, and disk information based on that name.
        /// </summary>
        /// <param name="name"></param>
        public MaintenanceDiscSummary(string name)
        {
            _name = name;
            _startFreeDiskSpace = IoHelper.GetTotalFreeSpace(name);
            _totalDiskSize = IoHelper.GetTotalSize(name);
            _deletedFileCount = 0;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Increments deleted file count.
        /// </summary>
        public void IncrementDeletedFileCount()
        {
            _deletedFileCount++;
        }

        #endregion

    }
}
