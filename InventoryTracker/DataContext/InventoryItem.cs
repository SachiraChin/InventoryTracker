using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InventoryTracker.DataContext
{
    public class InventoryItem
    {
        [Key]
        public int InventoryItemId { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerNIC { get; set; }
        public string ItemSerialNumber { get; set; }
        public string ItemType { get; set; }
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }
        [ForeignKey("Division")]
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public string Unit { get; set; }
        public string ItemCondition { get; set; }
        public bool Returned { get; set; }
        public DateTimeOffset BorrowedDate { get; set; }
        public DateTimeOffset? ReturnedDate { get; set; }
    }
}
