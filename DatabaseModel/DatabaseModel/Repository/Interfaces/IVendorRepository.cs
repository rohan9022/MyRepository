using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IVendorRepository
    {
        void Add(VendorMaster vendorMaster);

        void Update(VendorMaster vendorMaster);

        void Delete(int id);

        VendorMaster Get(int id);

        IList<VendorMaster> GetAll();
    }
}