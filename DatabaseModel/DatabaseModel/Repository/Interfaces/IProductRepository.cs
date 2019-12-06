using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IProductRepository
    {
        void Add(ProductMaster productMaster);

        void Update(ProductMaster productMaster);

        void Delete(int id);

        ProductMaster Get(string id);

        IList<ProductMaster> GetAll();
    }
}