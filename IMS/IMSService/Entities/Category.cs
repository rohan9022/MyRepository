using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public decimal CGST { get; set; }
        [DataMember]
        public decimal SGST { get; set; }
        [DataMember]
        public decimal IGST { get; set; }
    }
}