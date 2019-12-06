using DatabaseModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseModel.Repository
{
    internal class ReportRepository : IReportRepository
    {
        private readonly TestContext testContext;
        private readonly IInvoiceDetailsRepository invoiceDetailsRepository;
        private readonly IInvoiceProductRepository invoiceProductRepository;
        private readonly IInvoiceSettlementRepository invoiceSettlementRepository;

        public ReportRepository(IDbContextRepository dbContextRepository, IInvoiceDetailsRepository invoiceDetailsRepository, IInvoiceProductRepository invoiceProductRepository, IInvoiceSettlementRepository invoiceSettlementRepository)
        {
            testContext = dbContextRepository.TestContext;
            this.invoiceDetailsRepository = invoiceDetailsRepository;
            this.invoiceProductRepository = invoiceProductRepository;
            this.invoiceSettlementRepository = invoiceSettlementRepository;
        }

        //[sp_im_BulkDeleteInvoice_Latest_July]
        public void BulkDeleteInvoice_Latest_July(int fromInvoice, int toInvoice)
        {
            var invDetails = testContext.InvoiceDetailsMaster.Where(x => x.InvoiceNo >= fromInvoice && x.InvoiceNo <= toInvoice).ToList();
            testContext.InvoiceDetailsMaster.RemoveRange(invDetails);
            testContext.SaveChanges();
        }

        //[sp_im_FinalInvoiceReport]
        public Tuple<IQueryable<InvoiceReportSummary>, IQueryable<InvoiceReportDetails>> FinalInvoiceReport(int invNo)
        {
            var invReportSummary = from IM in testContext.Set<InvoiceDetailsMaster>()
                                   join VM in testContext.Set<VendorMaster>()
                                   on IM.VendorId equals VM.VendorId
                                   where IM.InvoiceNo == invNo && IM.FinalStatus != 9
                                   select GetDetails(IM, VM);

            var invReportDetails = from IPM in testContext.Set<InvoiceProductMaster>()
                                   join PDM in testContext.Set<ProductDescriptionMaster>() on IPM.ProductId equals PDM.ProductId
                                   join SLM in testContext.Set<SubLookupMaster>() on IPM.Status equals SLM.SubLookupId
                                   where IPM.InvoiceNo == invNo && SLM.LookupId == 14 && IPM.Status != 9
                                   select GetSummary(IPM, PDM, SLM);

            return new Tuple<IQueryable<InvoiceReportSummary>, IQueryable<InvoiceReportDetails>>(invReportSummary, invReportDetails);
        }

        private static InvoiceReportSummary GetDetails(InvoiceDetailsMaster IM, VendorMaster VM)
        {
            return new InvoiceReportSummary()
            {
                InvoiceNo = IM.InvoiceNo,
                PartyName = IM.PartysName,
                Address = IM.Address,
                InvoiceDate = IM.InvoiceDate,
                OrderNo = IM.OrderId,
                OrderDate = IM.OrderDate,
                GSTNo = VM.Gstno,
                PanNo = VM.PanNo,
                CommissionTab = VM.CommissionTab
            };
        }

        private static InvoiceReportDetails GetSummary(InvoiceProductMaster IPM, ProductDescriptionMaster PDM, SubLookupMaster SLM)
        {
            return new InvoiceReportDetails()
            {
                InvoiceNo = IPM.InvoiceNo,
                Description = PDM.ProductId + " - " + PDM.Title + " " + PDM.PosterDimensions + " [" + SLM.SubLookupName + "]",
                Quantity = IPM.Quantity,
                UnitPrice = IPM.UnitPrice,
                NetSale = IPM.NetSale,
                CGST = IPM.Cgst,
                SGST = IPM.Sgst,
                IGST = IPM.Igst,
                ShippingUnitPrice = IPM.ShippingUnitPrice,
                ShippingNetSale = IPM.ShippingNetSale,
                ShippingCGST = IPM.ShippingCgst,
                ShippingSGST = IPM.ShippingSgst,
                ShippingIGST = IPM.ShippingIgst,
                ShippingTotal = IPM.ShippingTotal,
                Rate = IPM.Rate,
                SettlementAmount = IPM.SettlementAmount,
                DifferenceAmount = (IPM.Total - IPM.SettlementAmount),
                CGSTPerc = IPM.Cgstperc,
                SGSTPerc = IPM.Sgstperc,
                IGSTPerc = IPM.Igstperc
            };
        }

        //[sp_im_GenInvoiceReport]
        public Tuple<IQueryable<InvoiceReportSummary>, IQueryable<InvoiceReportDetails>> GenInvoiceReport(int fromInvoiceNo, int toInvoiceNo, DateTime fromInvoiceDate, DateTime toInvoiceDate)
        {
            toInvoiceNo = (toInvoiceNo == 0) ? int.MaxValue : toInvoiceNo;
            toInvoiceDate = (toInvoiceDate == new DateTime()) ? DateTime.MaxValue : toInvoiceDate;

            var invReportSummary = from IM in testContext.Set<InvoiceDetailsMaster>()
                                   join VM in testContext.Set<VendorMaster>()
                                   on IM.VendorId equals VM.VendorId
                                   where IM.InvoiceNo >= fromInvoiceNo && IM.InvoiceNo <= toInvoiceNo
                                   && IM.InvoiceDate >= fromInvoiceDate && IM.InvoiceDate <= toInvoiceDate
                                   select GetDetails(IM, VM);

            var invReportDetails = from IPM in testContext.Set<InvoiceProductMaster>()
                                   join PDM in testContext.Set<ProductDescriptionMaster>() on IPM.ProductId equals PDM.ProductId
                                   join SLM in testContext.Set<SubLookupMaster>() on IPM.Status equals SLM.SubLookupId
                                   where IPM.InvoiceNo == fromInvoiceNo && SLM.LookupId == 14 && IPM.Status != 2 && IPM.Status != 9
                                   select GetSummary(IPM, PDM, SLM);

            return new Tuple<IQueryable<InvoiceReportSummary>, IQueryable<InvoiceReportDetails>>(invReportSummary, invReportDetails);
        }

        //[sp_im_GetInvoiceDetails]
        public Tuple<IQueryable<InvoiceDetailsMaster>, IQueryable<InvoiceProductMaster>, IQueryable<InvoiceSettlementMaster>> GetInvoiceDetails(int fromInvoiceNo, int toInvoiceNo, DateTime fromInvoiceDate, DateTime toInvoiceDate, string ordId, int searchMode)
        {
            IQueryable<InvoiceDetailsMaster> lstInvDetails = null;
            IQueryable<InvoiceProductMaster> lstInvProduct = null;
            IQueryable<InvoiceSettlementMaster> lstInvSettlement = null;
            if (searchMode == 1)
            {
                lstInvDetails = from IDM in testContext.Set<InvoiceDetailsMaster>()
                                where IDM.OrderId == ordId
                                select new InvoiceDetailsMaster();
                lstInvProduct = from IPM in testContext.Set<InvoiceProductMaster>()
                                where IPM.OrderId == ordId
                                select new InvoiceProductMaster();
                lstInvSettlement = from ISM in testContext.Set<InvoiceSettlementMaster>()
                                   where ISM.OrderId == ordId
                                   select new InvoiceSettlementMaster();
            }
            if (searchMode == 2)
            {
                lstInvDetails = from IDM in testContext.Set<InvoiceDetailsMaster>()
                                where IDM.InvoiceNo >= fromInvoiceNo && IDM.InvoiceNo <= toInvoiceNo
                                select new InvoiceDetailsMaster();
                lstInvProduct = from IPM in testContext.Set<InvoiceProductMaster>()
                                where IPM.InvoiceNo >= fromInvoiceNo && IPM.InvoiceNo <= toInvoiceNo
                                select new InvoiceProductMaster();
                lstInvSettlement = from ISM in testContext.Set<InvoiceSettlementMaster>()
                                   where ISM.InvoiceNo >= fromInvoiceNo && ISM.InvoiceNo <= toInvoiceNo
                                   select new InvoiceSettlementMaster();
            }
            if (searchMode == 3)
            {
                lstInvDetails = from IDM in testContext.Set<InvoiceDetailsMaster>()
                                where IDM.InvoiceDate >= fromInvoiceDate && IDM.InvoiceDate <= toInvoiceDate
                                select new InvoiceDetailsMaster();
                lstInvProduct = from IPM in testContext.Set<InvoiceProductMaster>()
                                where IPM.InvoiceDate >= fromInvoiceDate && IPM.InvoiceDate <= toInvoiceDate
                                select new InvoiceProductMaster();
                lstInvSettlement = from ISM in testContext.Set<InvoiceSettlementMaster>()
                                   where ISM.InvoiceDate >= fromInvoiceDate && ISM.InvoiceDate <= toInvoiceDate
                                   select new InvoiceSettlementMaster();
            }
            if (searchMode == 4)
            {
                lstInvDetails = from IDM in testContext.Set<InvoiceDetailsMaster>()
                                where IDM.InvoiceNo >= fromInvoiceNo && IDM.InvoiceNo <= toInvoiceNo && IDM.InvoiceDate >= fromInvoiceDate && IDM.InvoiceDate <= toInvoiceDate
                                select new InvoiceDetailsMaster();
                lstInvProduct = from IPM in testContext.Set<InvoiceProductMaster>()
                                where IPM.InvoiceNo >= fromInvoiceNo && IPM.InvoiceNo <= toInvoiceNo && IPM.InvoiceDate >= fromInvoiceDate && IPM.InvoiceDate <= toInvoiceDate
                                select new InvoiceProductMaster();
                lstInvSettlement = from ISM in testContext.Set<InvoiceSettlementMaster>()
                                   where ISM.InvoiceNo >= fromInvoiceNo && ISM.InvoiceNo <= toInvoiceNo && ISM.InvoiceDate >= fromInvoiceDate && ISM.InvoiceDate <= toInvoiceDate
                                   select new InvoiceSettlementMaster();
            }
            return new Tuple<IQueryable<InvoiceDetailsMaster>, IQueryable<InvoiceProductMaster>, IQueryable<InvoiceSettlementMaster>(lstInvDetails, lstInvProduct, lstInvSettlement);
        }
    }

    public class InvoiceReportSummary
    {
        public int InvoiceNo { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string GSTNo { get; set; }
        public string PanNo { get; set; }
        public bool CommissionTab { get; set; }
    }

    public class InvoiceReportDetails
    {
        public int InvoiceNo { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetSale { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal ShippingUnitPrice { get; set; }
        public decimal ShippingNetSale { get; set; }
        public decimal ShippingCGST { get; set; }
        public decimal ShippingSGST { get; set; }
        public decimal ShippingIGST { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal Rate { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal DifferenceAmount { get; set; }
        public int Status { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
    }
}