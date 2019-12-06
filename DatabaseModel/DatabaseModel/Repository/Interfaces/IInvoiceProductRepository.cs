using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IInvoiceProductRepository
    {
        void Add(InvoiceProductMaster invoiceProductMaster);

        void Update(InvoiceProductMaster invoiceProductMaster);

        void Delete(DateTime invDate, int invNo, string ordId, DateTime ordDate, string prdId);

        InvoiceProductMaster Get(int id);

        IList<InvoiceProductMaster> GetAll();
    }
}