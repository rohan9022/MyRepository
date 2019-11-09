using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Forms.Integration;
using System.Linq;
using System.IO;

namespace IMS.ViewModels
{
    internal class FinalInvoiceViewModel : ViewModelBase
    {
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

        private int _fromInvoiceNo;

        public int fromInvoiceNo
        {
            get { return _fromInvoiceNo; }
            set
            {
                _fromInvoiceNo = value;
                OnPropertyChanged(this, "fromInvoiceNo");
            }
        }

        private int _toInvoiceNo;

        public int toInvoiceNo
        {
            get { return _toInvoiceNo; }
            set
            {
                _toInvoiceNo = value;
                OnPropertyChanged(this, "toInvoiceNo");
            }
        }

        private string _folderPath;

        public string folderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                OnPropertyChanged(this, "folderPath");
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
        public DelegateCommand<object> cmdBlankDate { get; set; }

        public FinalInvoiceViewModel()
        {
            cmdView = new DelegateCommand<object>(cmdView_Execute);
            cmdBlankDate = new DelegateCommand<object>(cmdBlankDate_Execute);
            fromInvoiceDate = toInvoiceDate = DateTime.Now.Date;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        private void cmdView_Execute(object obj)
        {
            try
            {
                if (fromInvoiceDate > toInvoiceDate)
                {
                    UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                    return;
                }
                if ((toInvoiceDate - fromInvoiceDate).Days > 31)
                {
                    UIHelper.ShowErrorMessage("Days Difference should not be greater than 31");
                    return;
                }
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
                if (fromInvoiceNo != 0 && fromInvoiceNo != toInvoiceNo)
                {
                    using (System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        browse.ShowDialog();
                        folderPath = browse.SelectedPath;
                    }
                    if (string.IsNullOrEmpty(folderPath))
                    {
                        UIHelper.ShowErrorMessage("Select Path to save reports");
                        return;
                    }
                }
                using (DataServiceClient pxy = new DataServiceClient())
                {
                    ObservableCollection<CompanyMaster> objComp = new ObservableCollection<CompanyMaster>
                    {
                        pxy.GetCompanyDetails()
                    };
                    ObservableCollection<InvoiceReport> lstInvRept = pxy.FinalInvoiceReport(fromInvoiceNo, toInvoiceNo, fromInvoiceDate, toInvoiceDate);
                    if (lstInvRept.Count <= 0)
                    {
                        UIHelper.ShowMessage("Data not found");
                        return;
                    }
                    for (int i = 0; i < lstInvRept.Count; i++)
                    {
                        Viewer = new WindowsFormsHost();
                        using (ReportViewer reportViewer = new ReportViewer())
                        {
                            reportViewer.LocalReport.DataSources.Clear();
                            ObservableCollection<InvoiceReport> objTemp = new ObservableCollection<InvoiceReport>
                            {
                                lstInvRept[i]
                            };
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetCompany", objComp));
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetInvoiceReport", objTemp));
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetInvoice", lstInvRept[i].lstDetails));
                            if (lstInvRept[i].lstDetails.Sum(p => p.IGST) > 0)
                            {
                                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0003_IGST.rdlc";
                            }
                            else
                            {
                                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0003_CSGST.rdlc";
                            }

                            reportViewer.RefreshReport();
                            Viewer.Child = reportViewer;
                            if (fromInvoiceNo != 0 && fromInvoiceNo != toInvoiceNo)
                            {
                                byte[] Bytes = reportViewer.LocalReport.Render(format: "PDF", deviceInfo: "");
                                using (FileStream stream = new FileStream(folderPath + "//" + lstInvRept[i].InvoiceNo + ".pdf", FileMode.Create))
                                {
                                    stream.Write(Bytes, 0, Bytes.Length);
                                }
                            }
                        }
                    }
                }
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

        private void cmdBlankDate_Execute(object obj)
        {
            fromInvoiceDate = toInvoiceDate = new DateTime(1900, 01, 01);
        }
    }
}