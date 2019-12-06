using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IInvoiceSettlementRepository
    {
        void Add(InvoiceSettlementMaster invoiceSettlementMaster);

        void Update(InvoiceSettlementMaster invoiceSettlementMaster);

        void Delete(DateTime invDate, int invNo, string ordId, DateTime ordDate, string prdId, DateTime stlDate, int srNo);

        InvoiceSettlementMaster Get(int id);

        IList<InvoiceSettlementMaster> GetAll();
    }
}