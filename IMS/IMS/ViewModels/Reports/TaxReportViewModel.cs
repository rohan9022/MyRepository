using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Forms.Integration;
using System.Linq;
using System.Text;

namespace IMS.ViewModels
{
    internal class TaxReportViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;
        private DateTime _fromInvoiceDate;

        public DateTime fromInvoiceDate
        {
            get { return _fromInvoiceDate; }
            set
            {
                _fromInvoiceDate = value;
                OnPropertyChanged(this, "fromInvoiceDate");
            }
        }

        private DateTime _toInvoiceDate;

        public DateTime toInvoiceDate
        {
            get { return _toInvoiceDate; }
            set
            {
                _toInvoiceDate = value;
                OnPropertyChanged(this, "toInvoiceDate");
            }
        }

        private int _VendorID;

        public int VendorID
        {
            get { return _VendorID; }
            set
            {
                _VendorID = value;
                OnPropertyChanged(this, "VendorID");
            }
        }

        private ObservableCollection<VendorMaster> _lstVendor;

        public ObservableCollection<VendorMaster> lstVendor
        {
            get { return _lstVendor; }
            set
            {
                _lstVendor = value;
                OnPropertyChanged(this, "lstVendor");
            }
        }

        private WindowsFormsHost _Viewer;

        public WindowsFormsHost Viewer
        {
            get { return _Viewer; }
            set
            {
                _Viewer = value;
                OnPropertyChanged(this, "Viewer");
            }
        }

        public DelegateCommand<object> cmdView { get; set; }
        public DelegateCommand<object> cmdMonth { get; set; }
        public DelegateCommand<object> cmdVendor { get; set; }
        private ObservableCollection<TaxReport> lstTaxReport;

        public TaxReportViewModel()
        {
            try
            {
                if (pxy == null) pxy = new DataServiceClient();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstVendor = pxy.GetVendorList();
                lstVendor.Add(new VendorMaster { ID = 0, Name = "--ALL--" });
                cmdView = new DelegateCommand<object>(cmdView_Execute);
                cmdMonth = new DelegateCommand<object>(cmdMonth_Execute);
                cmdVendor = new DelegateCommand<object>(cmdVendor_Execute);
                fromInvoiceDate = toInvoiceDate = DateTime.Now.Date;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdView_Execute(object obj)
        {
            try
            {
                if (fromInvoiceDate > toInvoiceDate)
                {
                    UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                    return;
                }
                ////if ((toInvoiceDate - fromInvoiceDate).Days > 31)
                ////{
                ////    UIHelper.ShowErrorMessage("Days Difference should not be greater than 31");
                ////    return;
                ////}
                Viewer = new WindowsFormsHost();
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.LocalReport.DataSources.Clear();
                lstTaxReport = pxy.MonthVendorTaxReport(VendorID, fromInvoiceDate, toInvoiceDate);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("datasetTaxMonthVendor", lstTaxReport));
                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0004.rdlc";
                reportViewer.RefreshReport();
                Viewer.Child = reportViewer;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdMonth_Execute(object obj)
        {
            try
            {
                StringBuilder line = new StringBuilder();
                line.Append("Month            CGST            SGST            IGST            Shipping CGST   Shipping SGST   Shipping IGST   Total");
                line.AppendLine();
                line.AppendLine();
                foreach (string item in lstTaxReport.Select(p => p.Month).Distinct())
                {
                    line.Append(item.PadRight(17, ' ') + lstTaxReport.Where(p => p.Month == item).Sum(p => p.CGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.SGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.IGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_CGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_SGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_IGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Total + p.Shipping_Total).ToString("0.00").PadRight(16, ' '));
                    line.AppendLine();
                }
                Helper.Helper.ReadWriteFile(line.ToString());
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdVendor_Execute(object obj)
        {
            try
            {
                StringBuilder line = new StringBuilder();
                line.Append("Vendor           CGST            SGST            IGST            Shipping CGST   Shipping SGST   Shipping IGST   Total");
                line.AppendLine();
                line.AppendLine();
                foreach (string item in lstTaxReport.Select(p => p.Vendor).Distinct())
                {
                    line.Append(item.PadRight(17, ' ') + lstTaxReport.Where(p => p.Vendor == item).Sum(p => p.CGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.SGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.IGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_CGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_SGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Shipping_IGST).ToString("0.00").PadRight(16, ' ') +
                        lstTaxReport.Where(p => p.Month == item).Sum(p => p.Total + p.Shipping_Total).ToString("0.00").PadRight(16, ' '));
                }
                Helper.Helper.ReadWriteFile(line.ToString());
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }
    }
}