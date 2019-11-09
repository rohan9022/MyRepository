using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class SalesReport
    {
        [DataMember]
        public int InvoiceNo { get; set; }

        [DataMember]
        public DateTime InvoiceDate { get; set; }

        [DataMember]
        public string OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string PartyName { get; set; }

        [DataMember]
        public string PrdCd { get; set; }

        [DataMember]
        public string PrdName { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public decimal SellingPrice { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal NetSales { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }

        [DataMember]
        public decimal SubTotal { get; set; }

        [DataMember]
        public decimal InvoiceAmount { get; set; }

        [DataMember]
        public decimal CGSTPerc { get; set; }

        [DataMember]
        public decimal SGSTPerc { get; set; }

        [DataMember]
        public decimal IGSTPerc { get; set; }

        [DataMember]
        public decimal CGST { get; set; }

        [DataMember]
        public decimal SGST { get; set; }

        [DataMember]
        public decimal IGST { get; set; }

        [DataMember]
        public decimal Shipping_UnitPrice { get; set; }

        [DataMember]
        public decimal Shipping_NetSale { get; set; }

        [DataMember]
        public decimal Shipping_CGST { get; set; }

        [DataMember]
        public decimal Shipping_SGST { get; set; }

        [DataMember]
        public decimal Shipping_IGST { get; set; }

        [DataMember]
        public decimal Shipping_Total { get; set; }

        [DataMember]
        public decimal ShippingTaxAmount { get; set; }
    }
}