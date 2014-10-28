using System;
using System.Collections.Generic;

namespace FileMaintenance.Core.Models
{
    public interface IMaintenanceSummary
    {
        /// <summary>
        /// Gets the number of maintenance items being maintained.
        /// </summary>
        int MaintenanceItemCount { get; }

        /// <summary>
        /// Gets whether there are any affected disks that are low.
        /// </summary>
        bool IsAnyDiskLow { get; }

        /// <summary>
        /// Gets whether there were any errors during the maintenance.
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Gets the computer name being maintained.
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Gets the collection of disk summaries.
        /// </summary>
        IEnumerable<IMaintenanceDiskSummary> MaintenanceDiskSummaries { get; }

        /// <summary>
        /// Gets the collection of errors.
        /// </summary>
        IEnumerable<string> Errors { get; }

        /// <summary>
        /// Gets the duration of maintenance.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets the start time of maintenance in UTC.
        /// </summary>
        DateTime ExecutionStartTimeUtc { get; set; }

        /// <summary>
        /// Gets the end time of maintenance in UTC.
        /// </summary>
        DateTime ExecutionEndTimeUtc { get; set; }

        /// <summary>
        /// Gets report for all disks affected depending on what alerts have been set up.
        /// </summary>
        string GetDiskSpaceReport();

        /// <summary>
        /// Increments deleted file count for disk specified path is located on.
        /// </summary>
        /// <param name="path"></param>
        void IncrementDeletedFileCount(string path);

        /// <summary>
        /// Add an error to the collection.
        /// </summary>
        /// <param name="message"></param>
        void AddError(string message);
    }
}
