
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public interface IMaintenanceService
    {
        /// <summary>
        /// Starts the maintenance.
        /// </summary>
        void Start();

        /// <summary>
        /// Gets the maintenance summary.
        /// </summary>
        IMaintenanceSummary MaintenanceSummary { get; }
    }
}