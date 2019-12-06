using System;
using System.Collections.Generic;
using System.Text;
using DatabaseModel.Models;

namespace DatabaseModel.Repository
{
    internal class InvoiceSettlementRepository : IInvoiceSettlementRepository
    {
        private readonly TestContext testContext;

        public InvoiceSettlementRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(InvoiceSettlementMaster invoiceSettlementMaster)
        {
            throw new NotImplementedException();
        }

        //[sp_im_DeleteInvoiceSettlement]
        public void Delete(DateTime invDate, int invNo, string ordId, DateTime ordDate, string prdId, DateTime stlDate, int srNo)
        {
            var invSettlement = testContext.InvoiceSettlementMaster.Find(invDate, invNo, ordId, ordDate, prdId, stlDate, srNo);
            testContext.InvoiceSettlementMaster.Remove(invSettlement);
            testContext.SaveChanges();
        }

        public InvoiceSettlementMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<InvoiceSettlementMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceSettlementMaster invoiceSettlementMaster)
        {
            throw new NotImplementedException();
        }
    }
}