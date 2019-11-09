using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class MenuMaster
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int ScreenID { get; set; }
    }
}