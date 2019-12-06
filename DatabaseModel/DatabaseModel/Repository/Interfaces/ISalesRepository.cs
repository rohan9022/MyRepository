using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ISalesRepository
    {
        void Add(SalesMaster salesMaster);

        void Update(SalesMaster salesMaster);

        void Delete(int id);

        SalesMaster Get(int id);

        IList<SalesMaster> GetAll();
    }
}