using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class InvoiceProduct
    {
        [DataMember]
        public DateTime InvoiceDate { get; set; }
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal NetSale { get; set; }
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
        public decimal Total { get; set; }
        [DataMember]
        public decimal UnitPrice { get; set; }
        [DataMember]
        public int Status { get; set; }
    }
}