using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class ProductMaster
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int TotalSale { get; set; }
        public int TotalAvailableQuota { get; set; }
        public int TotalDamage { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
