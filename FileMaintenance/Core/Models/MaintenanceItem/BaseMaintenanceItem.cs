using System;
using FileMaintenance.Services;

namespace FileMaintenance.Core.Models
{
    public abstract class BaseMaintenanceItem
    {

        #region properties

        public string Path { get; private set; }

        public TimeSpan KeepFor { get; private set; }

        #endregion

        #region constructors

        protected BaseMaintenanceItem(string path, TimeSpan keepFor)
        {
            Path = path;
            KeepFor = keepFor;
        }

        #endregion

        #region public methods

        public IFileMaintenanceBuilder<BaseMaintenanceItem> CreateMaintenance()
        {
            return new FileMaintenanceBuilder<BaseMaintenanceItem>(this);
        }

        public virtual void ExecuteMaintenance(IMaintenanceServiceAction maintenanceService)
        {
            this.CreateMaintenance()
                .Where(file => DateTime.UtcNow.Subtract(this.KeepFor) > file.LastWriteTimeUtc)
                .Delete(filePath => maintenanceService.Delete(filePath))
                .Execute();
        }

        #endregion

    }
}