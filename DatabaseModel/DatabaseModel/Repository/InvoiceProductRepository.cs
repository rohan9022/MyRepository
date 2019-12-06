using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class InvoiceProductRepository : IInvoiceProductRepository
    {
        private readonly TestContext testContext;

        public InvoiceProductRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(InvoiceProductMaster invoiceProductMaster)
        {
            throw new NotImplementedException();
        }

        //[sp_im_DeleteInvoiceProduct]
        public void Delete(DateTime invDate, int invNo, string ordId, DateTime ordDate, string prdId)
        {
            var invProduct = testContext.InvoiceProductMaster.Find(invDate, invNo, ordId, ordDate, prdId);
            testContext.InvoiceProductMaster.Remove(invProduct);
            testContext.SaveChanges();
        }

        public InvoiceProductMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<InvoiceProductMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceProductMaster invoiceProductMaster)
        {
            throw new NotImplementedException();
        }
    }
}