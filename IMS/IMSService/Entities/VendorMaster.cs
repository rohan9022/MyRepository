using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class VendorMaster
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string GSTNo { get; set; }
        [DataMember]
        public string PanNo { get; set; }
        [DataMember]
        public bool CommissionTab { get; set; }
    }
}