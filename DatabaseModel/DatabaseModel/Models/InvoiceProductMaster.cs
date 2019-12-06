using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class InvoiceProductMaster
    {
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetSale { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal Igst { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingUnitPrice { get; set; }
        public decimal ShippingNetSale { get; set; }
        public decimal ShippingCgst { get; set; }
        public decimal ShippingSgst { get; set; }
        public decimal ShippingIgst { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal SettlementAmount { get; set; }
        public int Status { get; set; }
        public decimal Cgstperc { get; set; }
        public decimal Sgstperc { get; set; }
        public decimal Igstperc { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
