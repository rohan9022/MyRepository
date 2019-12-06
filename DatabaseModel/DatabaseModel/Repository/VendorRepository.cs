using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class VendorRepository : IVendorRepository
    {
        private readonly TestContext testContext;

        public VendorRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(VendorMaster vendorMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VendorMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<VendorMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(VendorMaster vendorMaster)
        {
            throw new NotImplementedException();
        }
    }
}