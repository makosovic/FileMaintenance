using System.Collections.Generic;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public interface IMaintenanceServiceConfig
    {
        /// <summary>
        /// Gets whether alerts are sent if any disks are low.
        /// </summary>
        bool AlertLowDisk { get; }

        /// <summary>
        /// Gets whether alerts are sent if any disks are low.
        /// </summary>
        bool AlertSummary { get; }

        /// <summary>
        /// Gets the collection of maintenance items configured for maintenance.
        /// </summary>
        IEnumerable<BaseMaintenanceItem> MaintenanceItems { get; }
    }
}
