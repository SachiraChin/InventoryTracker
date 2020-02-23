using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Models
{
    public class DatabaseConnectionSaveModel
    {
        public SqlServerConnectionInfoModel SqlServerConnectionInfo { get; set; }
        public bool SaveConnectionInfo { get; set; }
        public bool ShowAdvancedOptions { get; set; }
        public bool CreateDatabaseIfNotExists { get; set; }

    }
}
