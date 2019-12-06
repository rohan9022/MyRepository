using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class SalesMaster
    {
        public DateTime SalesDate { get; set; }
        public string ProductId { get; set; }
        public int InvoiceNo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsReturned { get; set; }
    }
}
