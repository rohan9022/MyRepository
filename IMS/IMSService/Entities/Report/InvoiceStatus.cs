using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class InvoiceStatus
    {
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public DateTime InvoiceDate { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string PartyName { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal SellingPrice { get; set; }
        [DataMember]
        public decimal Shipping_Total { get; set; }
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
        public decimal CGST { get; set; }
        [DataMember]
        public decimal SGST { get; set; }
        [DataMember]
        public decimal IGST { get; set; }


        [DataMember]
        public decimal CGSTPerc { get; set; }
        [DataMember]
        public decimal SGSTPerc { get; set; }
        [DataMember]
        public decimal IGSTPerc { get; set; }

        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public string GSTStatus { get; set; }
        [DataMember]
        public decimal SettlementAmount { get; set; }
        [DataMember]
        public DateTime SettlementDate { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Vendor { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}