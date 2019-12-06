using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class InvoiceRepository : IInvoiceRepository
    {
        private readonly TestContext testContext;

        public InvoiceRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(InvoiceMaster invoiceMaster)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public InvoiceMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<InvoiceMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceMaster invoiceMaster)
        {
            throw new NotImplementedException();
        }
    }
}