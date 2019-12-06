using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class LookupRepository : ILookupRepository
    {
        private readonly TestContext testContext;

        public LookupRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(LookupMaster lookupMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public LookupMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<LookupMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(LookupMaster lookupMaster)
        {
            throw new NotImplementedException();
        }
    }
}