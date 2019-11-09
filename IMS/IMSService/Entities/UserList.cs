using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class UserList
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int UserType { get; set; }
    }
}