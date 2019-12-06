using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IPurchaseRepository
    {
        void Add(PurchaseMaster purchaseMaster);

        void Update(PurchaseMaster purchaseMaster);

        void Delete(int id);

        PurchaseMaster Get(int id);

        IList<PurchaseMaster> GetAll();
    }
}