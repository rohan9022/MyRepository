using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class LookupMaster
    {
        public int LookupId { get; set; }
        public string LookupName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
