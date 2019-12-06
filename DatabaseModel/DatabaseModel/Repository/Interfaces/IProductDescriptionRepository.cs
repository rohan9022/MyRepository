using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IProductDescriptionRepository
    {
        void Add(ProductDescriptionMaster productDescriptionMaster);

        void Update(ProductDescriptionMaster productDescriptionMaster);

        void Delete(string id);

        ProductDescriptionMaster Get(string id);

        IList<ProductDescriptionMaster> GetAll();
    }
}