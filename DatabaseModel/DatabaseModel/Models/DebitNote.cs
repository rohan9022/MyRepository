using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class DebitNote
    {
        public string DrNoteNo { get; set; }
        public DateTime DrNoteDate { get; set; }
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
