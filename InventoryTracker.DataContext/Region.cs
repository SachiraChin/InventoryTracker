using System;

namespace InventoryTracker.DataContext
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}