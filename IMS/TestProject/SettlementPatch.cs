using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class SettlementPatch
    {
        List<InvoiceDetailsMaster> lstInvoiceDetailsMaster;
        public SettlementPatch()
        {
            lstInvoiceDetailsMaster = new List<InvoiceDetailsMaster>();
        }
    }

    class InvoiceDetailsMaster
    {
        public string InvoiceDate = string.Empty;//new DateTime();
        public int InvoiceNo = 0;
        public string OrderID = string.Empty;
        public string OrderDate = string.Empty;//new DateTime();
    }
}
