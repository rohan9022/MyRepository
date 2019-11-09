using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class SubCategory
    {
        [DataMember]
        public int SubCategoryID { get; set; }
        [DataMember]
        public int CategoryID { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}