using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class DamagedGoodsMaster
    {
        public DateTime Date { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
