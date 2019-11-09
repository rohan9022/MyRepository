using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Sales
    {
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public DateTime SalesDate { get; set; }
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public int VendorID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public string PartyName { get; set; }
    }
}