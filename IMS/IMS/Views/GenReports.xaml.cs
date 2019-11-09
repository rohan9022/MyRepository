using IMSLibrary.UI;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for GenReports.xaml
    /// </summary>
    public partial class GenReports : Window
    {
        private int fromInvoiceNo, toInvoiceNo;
        private DateTime fromInvoiceDate, toInvoiceDate;
        private int Mode;

        public GenReports()
        {
            InitializeComponent();
        }

        public GenReports(int IVNO, DateTime IVDT)
        {
            InitializeComponent();
            btnView.IsEnabled = false;
            fromInvoiceNo = toInvoiceNo = IVNO;
            fromInvoiceDate = toInvoiceDate = IVDT;
            frmIvNo.Text = toIvNo.Text = IVNO.ToString();
            frmIvDt.Text = toIvDt.Text = IVDT.ToString("dd-MM-yyyy");
            Mode = 1;
            GenerateReport();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try { fromInvoiceNo = Convert.ToInt32(frmIvNo.Text); }
            catch { fromInvoiceNo = 0; }
            try { toInvoiceNo = Convert.ToInt32(toIvNo.Text); }
            catch { toInvoiceNo = 0; }
            try { fromInvoiceDate = Convert.ToDateTime(frmIvDt.Value); if (fromInvoiceDate < new DateTime(1900, 01, 01)) fromInvoiceDate = new DateTime(1900, 01, 01); }
            catch { fromInvoiceDate = new DateTime(1900, 01, 01); }
            try { toInvoiceDate = Convert.ToDateTime(toIvDt.Value); if (toInvoiceDate < new DateTime(1900, 01, 01)) toInvoiceDate = new DateTime(1900, 01, 01); }
            catch { toInvoiceDate = new DateTime(1900, 01, 01); }
            if (fromInvoiceNo > toInvoiceNo)
            {
                UIHelper.ShowErrorMessage("From Invoice No. cannot not be greater than To Invoice No.");
                return;
            }
            if (fromInvoiceDate > toInvoiceDate)
            {
                UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                return;
            }
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (Mode != 1)
            {
                System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
                browse.ShowDialog();
                folderPath.Text = browse.SelectedPath;
                if (string.IsNullOrEmpty(folderPath.Text.Trim()))
                {
                    UIHelper.ShowErrorMessage("Select Path to save reports");
                    return;
                }
            }
            IMS.DataServiceRef.DataServiceClient pxy = new DataServiceRef.DataServiceClient();
            ObservableCollection<IMS.DataServiceRef.CompanyMaster> objComp = new ObservableCollection<DataServiceRef.CompanyMaster>();
            objComp.Add(pxy.GetCompanyDetails());
            ObservableCollection<IMS.DataServiceRef.InvoiceReport> lstInvRept = pxy.GenInvoiceReport(fromInvoiceNo, toInvoiceNo, fromInvoiceDate, toInvoiceDate);
            if (lstInvRept == null || lstInvRept.Count == 0)
            {
                UIHelper.ShowMessage("Data not found");
                return;
            }
            for (int i = 0; i < lstInvRept.Count; i++)
            {
                reportViewer.LocalReport.DataSources.Clear();
                ObservableCollection<IMS.DataServiceRef.InvoiceReport> objTemp = new ObservableCollection<DataServiceRef.InvoiceReport>();
                objTemp.Add(lstInvRept[i]);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetCompany", objComp));
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetInvoiceReport", objTemp));
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetInvoice", lstInvRept[i].lstDetails));
                if (lstInvRept[i].lstDetails.Sum(p => p.IGST) > 0)
                {
                    reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0001_IGST.rdlc";
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0001_CSGST.rdlc";
                }
                ////reportViewer.ServerReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString() + "I0001.rdlc";
                reportViewer.RefreshReport();
                if (Mode != 1 && (fromInvoiceNo != 0 && fromInvoiceNo != toInvoiceNo))
                {
                    byte[] Bytes = reportViewer.LocalReport.Render(format: "PDF", deviceInfo: "");
                    using (FileStream stream = new FileStream(folderPath.Text + "//" + lstInvRept[i].InvoiceNo + ".pdf", FileMode.Create))
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }
    }
}