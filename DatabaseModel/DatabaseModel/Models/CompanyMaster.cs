using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class CompanyMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string EmailId { get; set; }
        public string Gstno { get; set; }
        public byte[] Logo { get; set; }
    }
}
