using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class InvoiceMaster
    {
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductId { get; set; }
        public string PartysName { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetSale { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal Igst { get; set; }
        public decimal Total { get; set; }
        public decimal PackagingAndForwarding { get; set; }
        public int VendorId { get; set; }
        public DateTime SettlementDate { get; set; }
        public decimal SettlementAmount { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ReturnedDate { get; set; }
    }
}
