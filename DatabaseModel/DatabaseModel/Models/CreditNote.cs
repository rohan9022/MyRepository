using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class CreditNote
    {
        public string CrNoteNo { get; set; }
        public DateTime CrNoteDate { get; set; }
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsReturned { get; set; }
    }
}
