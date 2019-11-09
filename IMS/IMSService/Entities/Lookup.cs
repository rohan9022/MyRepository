using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Lookup
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}