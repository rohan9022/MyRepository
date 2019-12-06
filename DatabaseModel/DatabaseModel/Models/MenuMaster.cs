using System;
using System.Collections.Generic;

namespace DatabaseModel.Models
{
    public partial class MenuMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int ScreenId { get; set; }
        public bool IsActive { get; set; }
    }
}
