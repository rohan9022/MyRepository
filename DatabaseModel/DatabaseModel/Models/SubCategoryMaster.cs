using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class SubCategoryMaster
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
    }
}
