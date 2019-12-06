using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface ICompanyRepository
    {
        void Add(CompanyMaster companyMaster);

        void Update(CompanyMaster companyMaster);

        void Delete(int id);

        CompanyMaster Get(int id);

        IList<CompanyMaster> GetAll();
    }
}