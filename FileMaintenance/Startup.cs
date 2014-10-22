using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileMaintenance.Services;

namespace FileMaintenance
{
    static class Startup
    {
        static void Main()
        {
            IMaintenanceServiceConfig maintenanceServiceConfig = new MaintenanceServiceConfig();
            IMailServiceConfig mailServiceConfig = new MailServiceConfig();

            INotificationService mailService = new MailService(mailServiceConfig);

            ICollection<INotificationService> notificationServices = new Collection<INotificationService>();
            notificationServices.Add(mailService);

            IMaintenanceService maintenanceService = new MaintenanceService(notificationServices, maintenanceServiceConfig);
            maintenanceService.Start();
        }
    }
}
