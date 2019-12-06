using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly TestContext testContext;

        public ProductRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(ProductMaster productMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ProductMaster Get(string id)
        {
            throw new NotImplementedException();
        }

        public IList<ProductMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ProductMaster productMaster)
        {
            throw new NotImplementedException();
        }
    }
}