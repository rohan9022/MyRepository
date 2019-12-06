using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class InvoiceDetailsMaster
    {
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string PartysName { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public int VendorId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal SettlementAmount { get; set; }
        public int FinalStatus { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
