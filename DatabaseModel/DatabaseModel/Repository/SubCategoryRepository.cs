using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly TestContext testContext;

        public SubCategoryRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(SubCategoryMaster subCategoryMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SubCategoryMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<SubCategoryMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(SubCategoryMaster subCategoryMaster)
        {
            throw new NotImplementedException();
        }
    }
}