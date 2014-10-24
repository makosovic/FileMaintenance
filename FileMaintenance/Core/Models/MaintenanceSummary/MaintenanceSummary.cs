using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileMaintenance.Core.Helpers;

namespace FileMaintenance.Core.Models
{
    public class MaintenanceSummary : IMaintenanceSummary
    {
        #region private fields

        private readonly IDictionary<string, IMaintenanceDiskSummary> _maintenanceDiskSummaries;
        private readonly int _maintenanceItemCount;
        private bool? _anyDiskLow;

        #endregion

        #region properties

        /// <summary>
        /// Gets the number of maintenanceItems that are being maintained
        /// </summary>
        public int MaintenanceItemCount { get { return _maintenanceItemCount; } }

        /// <summary>
        /// Gets wheather any of the disks maintained are low
        /// </summary>
        /// <returns>Null if there is no disks registered for maintenance.</returns>
        public bool? IsAnyDiskLow
        {
            get
            {
                if (_maintenanceDiskSummaries.Count == 0) return null;

                if (_anyDiskLow != null) return _anyDiskLow;

                _anyDiskLow = _maintenanceDiskSummaries.Any(x => x.Value.FreeDiskSpace / (float)x.Value.TotalDiskSize < 0.1);
                return _anyDiskLow;
            }
        }

        /// <summary>
        /// Gets the collection of IMaintenanceDiskSummary for enumeration
        /// </summary>
        public IEnumerable<IMaintenanceDiskSummary> MaintenanceDiskSummaries
        {
            get { return _maintenanceDiskSummaries.Values; }
        }

        /// <summary>
        /// Gets the duration of maintenance
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (ExecutionStartTimeUtc != DateTime.MinValue && ExecutionEndTimeUtc != DateTime.MinValue &&
                    ExecutionEndTimeUtc > ExecutionStartTimeUtc)
                {
                    return ExecutionEndTimeUtc - ExecutionStartTimeUtc;
                }
                else
                {
                    return new TimeSpan();
                }
            }
        }

        /// <summary>
        /// Gets the ExecutionStartTime of maintenance
        /// </summary>
        public DateTime ExecutionStartTimeUtc { get; set; }

        /// <summary>
        /// Gets the ExecutionEndTimeUtc of maintenance
        /// </summary>
        public DateTime ExecutionEndTimeUtc { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maintenanceItemPaths"></param>
        public MaintenanceSummary(IEnumerable<string> maintenanceItemPaths)
        {
            _maintenanceItemCount = 0;
            _maintenanceDiskSummaries = new Dictionary<string, IMaintenanceDiskSummary>();
            foreach (var maintenanceItemPath in maintenanceItemPaths)
            {
                this._maintenanceItemCount++;
                this.TryAddDisk(IoHelper.GetDiskName(maintenanceItemPath));
            }
        }

        #endregion

        #region public methods

        public void IncrementDeletedFileCount(string path)
        {
            string name = IoHelper.GetDiskName(path);

            if (!string.IsNullOrEmpty(name) && _maintenanceDiskSummaries.ContainsKey(name))
            {
                _maintenanceDiskSummaries[name].IncrementDeletedFileCount();
            }
        }

        public sealed override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("Duration: {0:g}", this.Duration));
            builder.AppendLine(string.Format("Time started: {0:F} UTC", this.ExecutionStartTimeUtc));
            builder.AppendLine(string.Format("Time ended: {0:F} UTC", this.ExecutionEndTimeUtc));
            builder.AppendLine();
            builder.AppendLine(string.Format("Total items maintained: {0}", this.MaintenanceItemCount));
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("Disks affected:");

            foreach (var maintenanceDiskSummary in this._maintenanceDiskSummaries)
            {
                builder.AppendLine();
                builder.AppendLine(string.Format("Name: {0}", maintenanceDiskSummary.Key));
                builder.AppendLine(string.Format("Deleted file count: {0}", maintenanceDiskSummary.Value.DeletedFileCount));
                builder.AppendLine(string.Format("Space {0}: {1} MB", maintenanceDiskSummary.Value.SpaceFreed >= 0 ? "freed" : "used", maintenanceDiskSummary.Value.SpaceFreed / 1024 / 1024));
                builder.AppendLine(string.Format("Free disk space: {0}", maintenanceDiskSummary.Value.FreeDiskSpacePct));
            }

            return builder.ToString();
        }

        public string GetDiskSpaceReport()
        {
            StringBuilder builder = new StringBuilder();

            if (IsAnyDiskLow == true)
            {
                List<IMaintenanceDiskSummary> lowDisks =
                    _maintenanceDiskSummaries.Where(x => x.Value.FreeDiskSpace / (float)x.Value.TotalDiskSize < 0.1)
                        .Select(x => x.Value)
                        .ToList();

                builder.AppendLine(string.Format("During maintenance preformed on {0:F} UTC; following disks have shown low disk space:", ExecutionStartTimeUtc));

                lowDisks.ForEach(x => builder.AppendLine(string.Format("  - {0} ({1})", x.Name, x.FreeDiskSpacePct)));
            }

            return builder.ToString();
        }

        #endregion

        #region private methods

        private bool TryAddDisk(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (_maintenanceDiskSummaries.ContainsKey(name))
            {
                return false;
            }

            _maintenanceDiskSummaries.Add(name, new MaintenanceDiscSummary(name));
            return true;
        }

        #endregion
    }
}
