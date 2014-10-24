using System.IO;
using FileMaintenance.Core;
using FileMaintenance.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FileMaintenance.UnitTests
{
    [TestClass]
    public class MaintenanceManagerShould
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

            IMaintenanceSummary stubMaintenanceSummary = MockRepository.GenerateStub<IMaintenanceSummary>();
            IMaintenanceManager stubMaintenanceManager = MockRepository.GenerateMock<MaintenanceManager>(stubMaintenanceSummary, rootDirectory);

            stubMaintenanceManager.Backup(directoryToBackup, backupfile);

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

            IMaintenanceSummary stubMaintenanceSummary = MockRepository.GenerateStub<IMaintenanceSummary>();
            IMaintenanceManager stubMaintenanceManager = MockRepository.GenerateMock<MaintenanceManager>(stubMaintenanceSummary, directoryPath);

            stubMaintenanceManager.Delete(filePath);

            Assert.IsFalse(File.Exists(filePath));
        }
    }
}
