using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class SubLookupMaster
    {
        public int LookupId { get; set; }
        public int SubLookupId { get; set; }
        public string SubLookupName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
