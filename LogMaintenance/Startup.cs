using System;
using FileMaintenance.Services;
using LogMaintenance.Logging;

namespace LogMaintenance
{
    static class Startup
    {
        static void Main()
        {
            ILoggingService loggingService = new LoggingService();

            try
            {
                IMaintenanceService maintenanceService = new MaintenanceService();
                maintenanceService.Start();

                foreach (string error in maintenanceService.MaintenanceSummary.Errors)
                {
                    loggingService.HandleMessage(error);
                }
            }
            catch (Exception ex)
            {
                loggingService.HandleException(ex);
            }
        }
    }
}
