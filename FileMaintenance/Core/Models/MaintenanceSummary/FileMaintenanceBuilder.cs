using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FileMaintenance.Core.Helpers;

namespace FileMaintenance.Core.Models
{
    public class FileMaintenanceBuilder<T> : IFileMaintenanceBuilder<T> where T : BaseMaintenanceItem
    {

        #region private fields

        private readonly T _maintenanceItemInstance;
        private Action<string> _cleaningAction;
        private Action<string, string> _backupAction;
        private readonly ICollection<Func<FileInfo, bool>> _fileFilters;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the new instance of a FileMaintenanceBuilder class with a specified MaintenanceItem instance.
        /// </summary>
        /// <param name="maintenanceItemInstance"></param>
        public FileMaintenanceBuilder(T maintenanceItemInstance)
        {
            _maintenanceItemInstance = maintenanceItemInstance;
            _fileFilters = new Collection<Func<FileInfo, bool>>();
        }
        
        #endregion

        #region public methods

        /// <summary>
        /// Adds an expression used for filtering the files.
        /// </summary>
        /// <param name="expression"></param>
        /// <remarks>Multiple Where calls are supported.</remarks>
        public IFileMaintenanceBuilder<T> Where(Func<FileInfo, bool> expression)
        {
            _fileFilters.Add(expression);
            return this;
        }

        /// <summary>
        /// Adds a Delete action to be preformed on a filtered set of files.
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>Only one action is supported. Subsequent actions will override existing one.</remarks>
        public IFileMaintenanceBuilder<T> Delete(Action<string> action)
        {
            _cleaningAction = action;
            return this;
        }

        /// <summary>
        /// Adds a Backup action to be preformed on a filtered set of files.
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>Only one action is supported. Subsequent actions will override existing one.</remarks>
        public IFileMaintenanceBuilder<T> Backup(Action<string, string> action)
        {
            _backupAction = action;
            return this;
        }

        /// <summary>
        /// Executes the actions with given filters.
        /// </summary>
        public void Execute()
        {
            if (_backupAction != null || _cleaningAction != null)
            {
                Queue<string> directoriesToBeVisited = new Queue<string>();
                ICollection<FileInfo> fileInfos = new Collection<FileInfo>();

                PopulateDirectoriesAndFileInfos(_maintenanceItemInstance.Path, directoriesToBeVisited, fileInfos);

                while (directoriesToBeVisited.Count != 0)
                {
                    PopulateDirectoriesAndFileInfos(directoriesToBeVisited.Dequeue(), directoriesToBeVisited, fileInfos);
                }

                fileInfos = fileInfos.ToList();
                IBackupable backupableLog = _maintenanceItemInstance as IBackupable;

                if (backupableLog != null && _backupAction != null && fileInfos.Any())
                {
                    DateTime dateStart = fileInfos.Min(x => x.LastWriteTimeUtc);
                    DateTime dateEnd = fileInfos.Max(x => x.LastWriteTimeUtc);

                    DirectoryInfo di = Directory.CreateDirectory(Path.Combine(_maintenanceItemInstance.Path, string.Format("{0:yy-MM-dd}_{1:yy-MM-dd}", dateStart, dateEnd)));

                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string fileSubdirectoryPath = fileInfo.FullName.Replace(_maintenanceItemInstance.Path + "\\", "");

                        FileInfo newPath = new FileInfo(Path.Combine(di.FullName, fileSubdirectoryPath));
                        if (newPath.Directory != null && !newPath.Directory.Exists) newPath.Directory.Create();
                        File.Copy(fileInfo.FullName, newPath.FullName);
                        _cleaningAction.Invoke(fileInfo.FullName);
                    }

                    foreach (var backupFile in backupableLog.Backups)
                    {
                        _backupAction.Invoke(di.FullName, Path.Combine(backupFile.Path, di.Name) + ".zip");
                    }
                }
                else if (_cleaningAction != null)
                {
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        _cleaningAction.Invoke(fileInfo.FullName);
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("actions");
            }
        }

        private void PopulateDirectoriesAndFileInfos(string directoryToBeVisited, Queue<string> directoriesToBeVisited, ICollection<FileInfo> fileInfos)
        {
            foreach (string directory in IoHelper.GetDirectories(directoryToBeVisited, _fileFilters))
            {
                directoriesToBeVisited.Enqueue(directory);
            }

            foreach (FileInfo fileInfo in IoHelper.GetFileInfos(directoryToBeVisited, _fileFilters))
            {
                fileInfos.Add(fileInfo);
            }
        }

        #endregion

        #region private methods

        #endregion

    }
}
