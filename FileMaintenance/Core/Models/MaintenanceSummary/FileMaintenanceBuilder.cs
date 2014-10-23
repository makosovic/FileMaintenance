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
            Queue<string> directoriesToBeVisited = new Queue<string>();

            if (_backupAction != null || _cleaningAction != null)
            {
                IEnumerable<string> directories = TraverseAndInvoke(_maintenanceItemInstance.Path);
                foreach (string directory in directories)
                {
                    directoriesToBeVisited.Enqueue(directory);
                }

                while (directoriesToBeVisited.Count != 0)
                {
                    IEnumerable<string> subdirectories = TraverseAndInvoke(directoriesToBeVisited.Dequeue());
                    foreach (string subdirectory in subdirectories)
                    {
                        directoriesToBeVisited.Enqueue(subdirectory);
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("actions");
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Traverses the given directory path and invokes the actions specified on a filtered set of files.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        private IEnumerable<string> TraverseAndInvoke(string directoryPath)
        {
            IEnumerable<FileInfo> fileInfos;
            string[] subdirectories;
            IoHelper.GetFilesAndFolders(directoryPath, _fileFilters , out fileInfos, out subdirectories);

            fileInfos = fileInfos.ToList();
            IBackupable backupableLog = _maintenanceItemInstance as IBackupable;

            if (backupableLog != null && fileInfos.Any())
            {
                DateTime dateStart = fileInfos.Min(x => x.LastWriteTimeUtc);
                DateTime dateEnd = fileInfos.Max(x => x.LastWriteTimeUtc);

                DirectoryInfo di = Directory.CreateDirectory(Path.Combine(directoryPath, string.Format("{0:yy-MM-dd}_{1:yy-MM-dd}", dateStart, dateEnd)));

                foreach (FileInfo fileInfo in fileInfos)
                {
                    File.Copy(fileInfo.FullName, Path.Combine(di.FullName, fileInfo.Name));
                    _cleaningAction.Invoke(fileInfo.FullName);
                }

                string subdirectoryPath = di.FullName.Replace(_maintenanceItemInstance.Path + "\\", "");

                foreach (var backupFile in backupableLog.Backups)
                {
                    _backupAction.Invoke(di.FullName, Path.Combine(backupFile.Path, subdirectoryPath) + ".zip");
                }
            }
            else
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    _cleaningAction.Invoke(fileInfo.FullName);
                }
            }

            return subdirectories.ToList();
        }

        #endregion

    }
}
