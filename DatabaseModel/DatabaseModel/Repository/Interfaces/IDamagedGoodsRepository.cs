using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IDamagedGoodsRepository
    {
        void Add(DamagedGoodsMaster damagedGoodsMaster, int type);

        void Update(DamagedGoodsMaster damagedGoodsMaster, int type);

        void Delete(DamagedGoodsMaster damagedGoodsMaster, int type);

        DamagedGoodsMaster Get(DateTime damagedDate, string productId);

        IList<DamagedGoodsMaster> GetAll();

        void InsertUpdateDeleteDamage(DamagedGoodsMaster damagedGoodsMaster, int type, int mode);
    }
}