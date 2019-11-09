using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class CompanyMaster
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string ContactNo1 { get; set; }
        [DataMember]
        public string ContactNo2 { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public string GSTNo { get; set; }
        [DataMember]
        public byte[] Logo { get; set; }
    }
}