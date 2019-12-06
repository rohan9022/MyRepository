using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IInvoiceDetailsRepository
    {
        void Add(InvoiceDetailsMaster invoiceDetailsMaster);

        void Update(InvoiceDetailsMaster invoiceDetailsMaster);

        void Delete(DateTime invDate, int invNo, String orderId, DateTime orderDate);

        InvoiceDetailsMaster Get(int id);

        IList<InvoiceDetailsMaster> GetAll();
    }
}