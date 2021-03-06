﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FileMaintenance.Core.Helpers;
using FileMaintenance.Configuration;
using FileMaintenance.Core.Models;

namespace FileMaintenance.Services
{
    public class MaintenanceServiceConfig : IMaintenanceServiceConfig
    {

        #region private fields

        private readonly ICollection<BaseMaintenanceItem> _maintenanceItems; 

        #endregion

        #region properties

        /// <summary>
        /// Gets whether alerts are sent if any disks are low.
        /// </summary>
        public bool AlertLowDisk { get; private set; }

        /// <summary>
        /// Gets whether alerts are sent if any disks are low.
        /// </summary>
        public bool AlertSummary { get; private set; }

        /// <summary>
        /// Gets the collection of maintenance items configured for maintenance.
        /// </summary>
        public IEnumerable<BaseMaintenanceItem> MaintenanceItems { get { return _maintenanceItems.AsEnumerable(); } }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an instance of MaintenanceServiceConfig from Configuration file.
        /// </summary>
        public MaintenanceServiceConfig()
        {
            _maintenanceItems = new List<BaseMaintenanceItem>();

            InitItemsFromConfig();
        }

        #endregion

        #region private methods

        private void InitItemsFromConfig()
        {
            FileMaintenanceConfigSection config = ConfigurationManager.GetSection("fileMaintenance") as FileMaintenanceConfigSection;

            if (config == null)
                throw new ArgumentNullException("fileMaintenance");

            AlertSummary = config.Summary;
            MaintenanceItemConfigElementCollection maintenanceItems = config.MaintenanceItems;
            AlertConfigElementCollection alerts = config.Alerts;


            if (maintenanceItems == null)
                throw new ArgumentNullException("items");

            if (maintenanceItems.Count == 0)
                throw new ArgumentOutOfRangeException("items");

            foreach (MaintenanceItemConfigElement item in maintenanceItems)
            {
                int days, hours, minutes;
                if (!ConfigurationHelper.TryParseKeepFor(item.KeepFor, out days, out hours, out minutes))
                    throw new ArgumentOutOfRangeException("keepFor");

                MaintenanceItem tmpMaintenanceItem = new MaintenanceItem(item.Path, new TimeSpan(days, hours, minutes, 0));
                MaintenanceItemBackupConfigElementCollection maintenanceItemBackups = item.Backups;

                foreach (MaintenanceItemBackupConfigElement backup in maintenanceItemBackups)
                {
                    int bDays, bHours, bMinutes;
                    if (!ConfigurationHelper.TryParseKeepFor(item.KeepFor, out bDays, out bHours, out bMinutes))
                        throw new ArgumentOutOfRangeException("keepFor");

                    MaintenanceItemBackup tmpMaintenanceItemBackup = new MaintenanceItemBackup(backup.Path, new TimeSpan(bDays, bHours, bMinutes, 0));
                    tmpMaintenanceItem.AddBackup(tmpMaintenanceItemBackup);
                    _maintenanceItems.Add(tmpMaintenanceItemBackup);

                }

                _maintenanceItems.Add(tmpMaintenanceItem);
            }

            if (alerts != null)
            {
                foreach (AlertConfigElement alert in alerts)
                {
                    switch (alert.When)
                    {
                        case "LowDisk":
                            AlertLowDisk = true;
                            break;
                    }
                }
            }
        }

        #endregion

    }
}
