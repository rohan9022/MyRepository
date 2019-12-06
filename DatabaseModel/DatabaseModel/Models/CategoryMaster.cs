using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class CategoryMaster
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Cgst { get; set; }
        public decimal Sgst { get; set; }
        public decimal Igst { get; set; }
    }
}