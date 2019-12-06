using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ILookupRepository
    {
        void Add(LookupMaster lookupMaster);

        void Update(LookupMaster lookupMaster);

        void Delete(int id);

        LookupMaster Get(int id);

        IList<LookupMaster> GetAll();
    }
}