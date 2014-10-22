using System;

namespace FileMaintenance.Core.Models
{
    public class BackupFolder : BaseFolder
    {

        #region constructors

        public BackupFolder(string path, TimeSpan keepFor) 
            : base(path, keepFor)
        {
        }

        #endregion

    }
}
