using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Core
{
    public class MaintenanceManager : IMaintenanceManager
    {

        #region private fields

        private readonly IMaintenanceSummary _maintenanceSummary;

        #endregion

        #region constructors

        public MaintenanceManager(IMaintenanceSummary maintenanceSummary)
        {
            _maintenanceSummary = maintenanceSummary;
        }

        #endregion

        #region public methods

        public void Backup(string sourcePath, string targetFilePath)
        {
            FileInfo sourceFi = new FileInfo(sourcePath);
            FileInfo targetFi = new FileInfo(targetFilePath);

            if (!string.Equals(targetFi.Extension, ".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("targetPath");
            }

            if (targetFi.Exists)
            {
                throw new Exception("Target path already exists.");
            }

            if (targetFi.DirectoryName != null)
            {
                if (!Directory.Exists(targetFi.DirectoryName))
                {
                    Directory.CreateDirectory(targetFi.DirectoryName);
                }
            }

            if ((sourceFi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {

                ZipFile.CreateFromDirectory(sourcePath, targetFilePath);
                Directory.Delete(sourcePath, true);
            }
            else
            {
                using (var zip = ZipFile.Open(targetFilePath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(sourceFi.FullName, sourceFi.Name);
                }

                File.Delete(sourcePath);
            }
        }

        public void Delete(string path)
        {
            File.Delete(path);
            _maintenanceSummary.IncrementDeletedFileCount(path);
        } 

        #endregion

    }
}
