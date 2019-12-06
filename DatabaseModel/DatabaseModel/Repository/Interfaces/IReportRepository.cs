using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseModel.Repository
{
    internal interface IReportRepository
    {
        Tuple<IQueryable<InvoiceReportSummary>, IQueryable<InvoiceReportDetails>> FinalInvoiceReport(int invNo);
    }
}