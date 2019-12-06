using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ISubLookupRepository
    {
        void Add(SubLookupMaster subLookupMaster);

        void Update(SubLookupMaster subLookupMaster);

        void Delete(int id);

        SubLookupMaster Get(int id);

        IList<SubLookupMaster> GetAll();
    }
}