using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class SalesRepository : ISalesRepository
    {
        private readonly TestContext testContext;

        public SalesRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(SalesMaster salesMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SalesMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<SalesMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(SalesMaster salesMaster)
        {
            throw new NotImplementedException();
        }
    }
}