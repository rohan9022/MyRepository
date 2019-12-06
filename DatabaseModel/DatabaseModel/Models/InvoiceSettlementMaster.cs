using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class InvoiceSettlementMaster
    {
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductId { get; set; }
        public DateTime SettlementDate { get; set; }
        public int SrNo { get; set; }
        public decimal SettlementAmount { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
