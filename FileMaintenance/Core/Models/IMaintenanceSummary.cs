using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IMaintenanceSummary
    {
        int FolderCount { get; }
        bool? IsAnyDiskLow { get; }
        IEnumerable<IMaintenanceDiskSummary> MaintenanceDiskSummaries { get; }
        TimeSpan Duration { get; }
        DateTime ExecutionStartTimeUtc { get; set; }
        DateTime ExecutionEndTimeUtc { get; set; }
        void IncrementDeletedFileCount(string path);
        string GetDiskSpaceReport();
    }
}
