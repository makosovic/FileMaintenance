using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FileMaintenance.Core.Helpers;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Core
{
    public class MaintenanceManager : IMaintenanceManager
    {

        #region private fields

        private readonly IMaintenanceSummary _maintenanceSummary;
        private readonly ICollection<Func<FileInfo, bool>> _fileFilters;
        private IEnumerable<FileInfo> _files;
        private bool _conditionsChanged;
        private readonly string _maintenancePath;

        #endregion

        #region properties

        public IEnumerable<string> Files
        {
            get
            {
                if (!_conditionsChanged)
                {
                    return _files.Select(x => x.FullName);
                }
                else
                {
                    _files = Traverse(_maintenancePath);
                    return _files.Select(x => x.FullName);
                }
            }
        }

        #endregion

        #region constructors

        public MaintenanceManager(IMaintenanceSummary maintenanceSummary, string maintenancePath)
        {
            if (!Directory.Exists(maintenancePath))
            {
                throw new ArgumentException("maintenancePath");
            }

            _conditionsChanged = true;
            _maintenanceSummary = maintenanceSummary;
            _maintenancePath = maintenancePath;
            _fileFilters = new Collection<Func<FileInfo, bool>>();
            _files = new Collection<FileInfo>();
        }

        #endregion

        #region public methods

        public void AddCondition(Func<FileInfo, bool> expression)
        {
            _fileFilters.Add(expression);
            _conditionsChanged = true;
        }

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
                using (ZipArchive zip = ZipFile.Open(targetFilePath, ZipArchiveMode.Create))
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

        public string GroupFilesInNewDirectory()
        {
            if (_conditionsChanged)
            {
                _files = Traverse(_maintenancePath);
            }

            List<FileInfo> filesList = _files.ToList();

            DateTime dateStart = filesList.Min(x => x.LastWriteTimeUtc);
            DateTime dateEnd = filesList.Max(x => x.LastWriteTimeUtc);

            DirectoryInfo di = Directory.CreateDirectory(Path.Combine(_maintenancePath, string.Format("{0:yy-MM-dd}_{1:yy-MM-dd}", dateStart, dateEnd)));

            foreach (FileInfo fileInfo in filesList)
            {
                string fileSubdirectoryPath = fileInfo.FullName.Replace(_maintenancePath + "\\", "");
                FileInfo newPath = new FileInfo(Path.Combine(di.FullName, fileSubdirectoryPath));
                if (newPath.Directory != null && !newPath.Directory.Exists) newPath.Directory.Create();
                File.Copy(fileInfo.FullName, newPath.FullName);
            }

            return di.FullName;
        }

        #endregion

        #region private methods

        private IEnumerable<FileInfo> Traverse(string path)
        {
            Queue<string> directoriesToBeVisited = new Queue<string>();
            ICollection<FileInfo> fileInfos = new Collection<FileInfo>();

            PopulateDirectoriesAndFileInfos(path, directoriesToBeVisited, fileInfos);

            while (directoriesToBeVisited.Count != 0)
            {
                PopulateDirectoriesAndFileInfos(directoriesToBeVisited.Dequeue(), directoriesToBeVisited, fileInfos);
            }

            return fileInfos;
        }

        private void PopulateDirectoriesAndFileInfos(string targetdirectory, Queue<string> directoriesToBeVisited, ICollection<FileInfo> fileInfos)
        {
            foreach (string directory in IoHelper.GetDirectories(targetdirectory, _fileFilters))
            {
                directoriesToBeVisited.Enqueue(directory);
            }

            foreach (FileInfo fileInfo in IoHelper.GetFileInfos(targetdirectory, _fileFilters))
            {
                fileInfos.Add(fileInfo);
            }
        }
        #endregion

    }
}
