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

        public abstract void ExecuteMaintenance(IMaintenanceManager maintenanceService);

        #endregion

    }
}