using System;
using System.Collections.Generic;
using System.IO;


namespace FileMaintenance.Core
{
    public interface IMaintenanceManager
    {
        /// <summary>
        /// Gets the collection of file paths (maintenance items) that satisfy a condition.
        /// </summary>
        IEnumerable<string> Files { get; }

        /// <summary>
        /// Create a compressed backup of a file or directory on sourcePath at target path and delete the source file or directory.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetFilePath"></param>
        void Backup(string sourcePath, string targetFilePath);

        /// <summary>
        /// Delete a file or directory at given path.
        /// </summary>
        /// <param name="path"></param>
        void Delete(string path);

        /// <summary>
        /// Add a condition how maintenance items should be filtered.
        /// </summary>
        /// <param name="expression"></param>
        void AddCondition(Func<FileInfo, bool> expression);

        /// <summary>
        /// Traverses a path and copies the files that satisfied the condition to a new directory.
        /// </summary>
        /// <returns></returns>
        string GroupFilesInNewDirectory();
    }
}