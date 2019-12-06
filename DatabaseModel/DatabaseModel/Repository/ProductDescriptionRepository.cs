using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class ProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly TestContext testContext;

        public ProductDescriptionRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(ProductDescriptionMaster productDescriptionMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public ProductDescriptionMaster Get(string id)
        {
            throw new NotImplementedException();
        }

        public IList<ProductDescriptionMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(ProductDescriptionMaster productDescriptionMaster)
        {
            throw new NotImplementedException();
        }
    }
}