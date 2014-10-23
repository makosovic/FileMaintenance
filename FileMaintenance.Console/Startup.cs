using System;
using FileMaintenance.Services;

namespace FileMaintenance.Console
{
    static class Startup
    {
        static void Main()
        {
            try
            {
                IMaintenanceService maintenanceService = new MaintenanceService();
                maintenanceService.Start();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
