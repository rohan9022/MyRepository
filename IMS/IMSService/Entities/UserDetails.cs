using System;
using System.Runtime.Serialization;

namespace IMSService.Entities
{
    [DataContract]
    public class UserDetails
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string UserPassword { get; set; }

        [DataMember]
        public int UserType { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public string ContactNo { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int Gender { get; set; }
    }
}