using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class PurchaseMaster
    {
        public DateTime PurchaseDate { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Mrp { get; set; }
        public decimal Pp { get; set; }
        public decimal Sp { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
