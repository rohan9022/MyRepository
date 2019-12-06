using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IInvoiceRepository
    {
        void Add(InvoiceMaster invoiceMaster);

        void Update(InvoiceMaster invoiceMaster);

        void Delete(int id);

        InvoiceMaster Get(int id);

        IList<InvoiceMaster> GetAll();
    }
}