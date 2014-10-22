using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileMaintenance.Core.Helpers
{
    /// <summary>
    /// Provides methods for I/O and disk related operations.
    /// </summary>
    public class IoHelper
    {
        /// <summary>
        /// Traverses a directory with a given filter for files and populates two collections (FileInfos and subdirectories).
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileFilters"></param>
        /// <param name="fileInfoCollection"></param>
        /// <param name="subdirectories"></param>
        public static void GetFilesAndFolders(string directoryPath
            , IEnumerable<Func<FileInfo, bool>> fileFilters
            , out IEnumerable<FileInfo> fileInfos
            , out string[] subdirectories)
        {
            string[] files = Directory.GetFiles(directoryPath);
            fileInfos = files.Select(x => new FileInfo(x));

            foreach (var expression in fileFilters)
                fileInfos = fileInfos.Where(expression);

            subdirectories = Directory.GetDirectories(directoryPath);
        }

        /// <summary>
        /// Gets the total free space of a given drive.
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns>Returns the number of free bytes, or -1 if drive was not found.</returns>
        public static long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the total size of a given drive.
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns>Returns the number of bytes, or -1 if the drive was not found.</returns>
        public static long GetTotalSize(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalSize;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the disk name from a given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns disk name, or an empty string if path was invalid or didn't exist.</returns>
        public static string GetDiskName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (!Directory.Exists(path))
            {
                return string.Empty;
            }

            return path.Split('\\')[0] + '\\';
        }
    }
}
