using System;
using FileMaintenance.Services;

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

        #region public methods

        public override void ExecuteMaintenance(IMaintenanceManager maintenanceManager)
        {
            maintenanceManager.AddCondition(file => DateTime.UtcNow.Subtract(this.KeepFor) > file.LastWriteTimeUtc);
            string[] files = maintenanceManager.Traverse(Path);

            foreach (var file in files)
            {
                maintenanceManager.Delete(file);
            }
        }

        #endregion

    }
}
