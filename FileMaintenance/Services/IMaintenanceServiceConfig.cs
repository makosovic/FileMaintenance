using System.Collections.Generic;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public interface IMaintenanceServiceConfig
    {
        bool AlertLowDisk { get; }
        bool AlertSummary { get; }
        IEnumerable<BaseMaintenanceItem> MaintenanceItems { get; }
    }
}
