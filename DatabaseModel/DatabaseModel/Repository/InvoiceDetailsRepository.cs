using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DatabaseModel.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseModel.Repository
{
    internal class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {
        private readonly TestContext testContext;

        public InvoiceDetailsRepository(IDbContextRepository dbContextRepository)
        {
            testContext = dbContextRepository.TestContext;
        }

        public void Add(InvoiceDetailsMaster invoiceDetailsMaster)
        {
            //invoiceDetailsMaster.CategoryId = testContext.CategoryMaster.Max(x => x.CategoryId) + 1;
            //testContext.CategoryMaster.Add(categoryMaster);
            //testContext.SaveChanges();
        }

        //[sp_im_DeleteInvoiceDetails]
        public void Delete(DateTime invDate, int invNo, String orderId, DateTime orderDate)
        {
            var invDetails = testContext.InvoiceDetailsMaster.Find(invDate, invNo, orderId, orderDate);
            testContext.InvoiceDetailsMaster.Remove(invDetails);
            testContext.SaveChanges();
        }

        public InvoiceDetailsMaster Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<InvoiceDetailsMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(InvoiceDetailsMaster invoiceDetailsMaster)
        {
            throw new NotImplementedException();
        }
    }
}