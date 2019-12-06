using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class InventoryItemMaster
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int TotalAvailable { get; set; }
        public int TotalWasted { get; set; }
        public int TotalSales { get; set; }
        public decimal Price { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
