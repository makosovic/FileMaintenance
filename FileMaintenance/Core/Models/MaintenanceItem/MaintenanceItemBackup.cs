using System;

namespace FileMaintenance.Core.Models
{
    public class MaintenanceItemBackup : BaseMaintenanceItem
    {

        #region constructors

        public MaintenanceItemBackup(string path, TimeSpan keepFor) 
            : base(path, keepFor)
        {
        }

        #endregion

    }
}
