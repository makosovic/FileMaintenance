using System;

namespace FileMaintenance.Core.Models
{
    public abstract class BaseMaintenanceItem
    {

        #region properties

        /// <summary>
        /// Gets the Path to the item being maintained.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the timespan of the item being maintained.
        /// </summary>
        public TimeSpan KeepFor { get; private set; }

        #endregion

        #region constructors

        /// <summary>
        /// Instantiate a maintenance item at a given path, to be kept for a specified amount of time.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keepFor"></param>
        protected BaseMaintenanceItem(string path, TimeSpan keepFor)
        {
            Path = path;
            KeepFor = keepFor;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Method executed in maintenance service.
        /// </summary>
        /// <param name="maintenanceManager"></param>
        public abstract void ExecuteMaintenance(IMaintenanceManager maintenanceManager);

        #endregion

    }
}