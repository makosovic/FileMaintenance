using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IMaintenanceSummary
    {
        int MaintenanceItemCount { get; }
        bool IsAnyDiskLow { get; }
        bool HasErrors { get; }
        string ServerName { get; }
        IEnumerable<IMaintenanceDiskSummary> MaintenanceDiskSummaries { get; }
        IEnumerable<string> Errors { get; }
        TimeSpan Duration { get; }
        DateTime ExecutionStartTimeUtc { get; set; }
        DateTime ExecutionEndTimeUtc { get; set; }

        string GetDiskSpaceReport();
        void IncrementDeletedFileCount(string path);
        void AddError(string message);
    }
}
