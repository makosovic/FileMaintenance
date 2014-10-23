using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileMaintenance.Core.Models;
using FileMaintenance.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FileMaintenance.UnitTest
{
    [TestClass]
    public class MaintenanceServiceShould
    {
        [TestMethod]
        public void Backup()
        {
            const string rootDirectory = "c:\\Temp";
            const string directoryToBackup = "c:\\Temp\\BackupTest";
            const string backupDirectory = "c:\\Temp\\Backup";
            const string backupfile = "c:\\Temp\\Backup\\BackupTest.zip";

            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            if (!Directory.Exists(directoryToBackup))
            {
                Directory.CreateDirectory(directoryToBackup);
            }

            if (Directory.Exists(backupDirectory))
            {
                Directory.Delete(backupDirectory, true);
            }

            Assert.IsFalse(Directory.Exists(backupDirectory));

            var stubNotificationServices = new Collection<INotificationService>();
            var stubMainenanceServiceConfig = MockRepository.GenerateStub<IMaintenanceServiceConfig>();
            var stubMailService = MockRepository.GenerateStub<INotificationService>();
            stubNotificationServices.Add(stubMailService);

            var items = new List<BaseMaintenanceItem>();
            stubMainenanceServiceConfig.Stub(x => x.MaintenanceItems).Return(items);

            var maintenanceService = new MaintenanceService(stubNotificationServices, stubMainenanceServiceConfig);

            maintenanceService.Backup(directoryToBackup, backupfile);

            Assert.IsTrue(File.Exists(backupfile));

            if (Directory.Exists(backupDirectory))
            {
                Directory.Delete(backupDirectory, true);
            }
        }

        [TestMethod]
        public void Delete()
        {
            const string directoryPath = "c:\\Temp";
            const string fileName = "unitTest.txt";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);

            File.Create(filePath).Dispose();

            Assert.IsTrue(File.Exists(filePath));

            var stubNotificationServices = new Collection<INotificationService>();
            var stubMainenanceServiceConfig = MockRepository.GenerateStub<IMaintenanceServiceConfig>();
            var stubMailService = MockRepository.GenerateStub<INotificationService>();
            stubNotificationServices.Add(stubMailService);

            var items = new List<BaseMaintenanceItem>();
            stubMainenanceServiceConfig.Stub(x => x.MaintenanceItems).Return(items);

            var maintenanceService = new MaintenanceService(stubNotificationServices, stubMainenanceServiceConfig);

            maintenanceService.Delete(filePath);

            Assert.IsFalse(File.Exists(filePath));
        }
    }
}
