using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly TestContext testContext;

        public CompanyRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(CompanyMaster companyMaster)
        {
            companyMaster.Id = testContext.CompanyMaster.Max(x => x.Id) + 1;
            testContext.CompanyMaster.Add(companyMaster);
            testContext.SaveChanges();
        }

        public void Delete(int id)
        {
            try
            {
                var company = testContext.CompanyMaster.Find(id);
                testContext.CompanyMaster.Remove(company);
                testContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //[sp_comp_GetCompanyDetails]
        public CompanyMaster Get(int id)
        {
            try
            {
                return testContext.CompanyMaster.Find(id);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IList<CompanyMaster> GetAll()
        {
            return testContext.CompanyMaster.OrderBy(x => x.Name).ToList();
        }

        public void Update(CompanyMaster companyMaster)
        {
            var company = testContext.CompanyMaster.Find(companyMaster.Id);
            company.Address1 = companyMaster.Address1;
            company.Address2 = companyMaster.Address2;
            company.Address3 = companyMaster.Address3;
            company.ContactNo1 = companyMaster.ContactNo1;
            company.ContactNo2 = companyMaster.ContactNo2;
            company.EmailId = companyMaster.EmailId;
            company.Gstno = companyMaster.Gstno;
            company.Logo = companyMaster.Logo;
            company.Name = companyMaster.Name;

            testContext.SaveChanges();
        }
    }
}