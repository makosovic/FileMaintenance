using System;
using FileMaintenance.Services;

namespace FileMaintenance
{
    static class Startup
    {
        static void Main()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();

            try
            {
                maintenanceService.Start();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
