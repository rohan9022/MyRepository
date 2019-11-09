using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class ProductList
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}