using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Damage
    {
        [DataMember]
        public string ProductID { get; set; }
        [DataMember]
        public DateTime DamageDate { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal Price { get; set; }
    }
}