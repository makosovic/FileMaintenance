
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public interface IMaintenanceService
    {
        void Start();
        IMaintenanceSummary MaintenanceSummary { get; }
    }
}