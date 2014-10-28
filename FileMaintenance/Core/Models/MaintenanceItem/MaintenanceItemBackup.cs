using System;

namespace FileMaintenance.Core.Models
{
    public class MaintenanceItemBackup : BaseMaintenanceItem
    {

        #region constructors

        /// <summary>
        /// Instantiate a maintenance item backup at a given path, to be kept for a specified amount of time.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        public MaintenanceItemBackup(string path, TimeSpan keepFor) 
            : base(path, keepFor)
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Method executed in maintenance service.
        /// </summary>
        /// <param name="maintenanceManager"></param>
        public override void ExecuteMaintenance(IMaintenanceManager maintenanceManager)
        {
            maintenanceManager.AddCondition(file => DateTime.UtcNow.Subtract(this.KeepFor) > file.LastWriteTimeUtc);

            foreach (string file in maintenanceManager.Files)
            {
                maintenanceManager.Delete(file);
            }
        }

        #endregion

    }
}
