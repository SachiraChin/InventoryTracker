using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryTracker.DataContext
{
    public class Division
    {
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}