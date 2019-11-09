using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Purchase
    {
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public DateTime PurchaseDate { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public decimal MRP { get; set; }
        [DataMember]
        public decimal PP { get; set; }
        [DataMember]
        public decimal SP { get; set; }
    }
}