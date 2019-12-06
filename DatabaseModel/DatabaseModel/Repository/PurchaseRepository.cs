using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class PurchaseRepository : IPurchaseRepository
    {
        private readonly TestContext testContext;

        public PurchaseRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(PurchaseMaster purchaseMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PurchaseMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<PurchaseMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(PurchaseMaster purchaseMaster)
        {
            throw new NotImplementedException();
        }
    }
}