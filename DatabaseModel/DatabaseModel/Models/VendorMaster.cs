using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class VendorMaster
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string Gstno { get; set; }
        public string PanNo { get; set; }
        public bool CommissionTab { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
