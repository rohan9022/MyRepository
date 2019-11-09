using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms.Integration;

namespace IMS.ViewModels
{
    internal class CreditNoteViewModel : ViewModelBase
    {
        private DateTime _fromDate;

        public DateTime fromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                OnPropertyChanged(this, "fromDate");
            }
        }

        private DateTime _toDate;

        public DateTime toDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                OnPropertyChanged(this, "toDate");
            }
        }

        private string _creditNoteNo;

        public string creditNoteNo
        {
            get { return _creditNoteNo; }
            set
            {
                _creditNoteNo = value;
                OnPropertyChanged(this, "creditNoteNo");
            }
        }

        private string _orderID;

        public string orderID
        {
            get { return _orderID; }
            set
            {
                _orderID = value;
                OnPropertyChanged(this, "orderID");
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

        public CreditNoteViewModel()
        {
            cmdView = new DelegateCommand<object>(cmdView_Execute);
            cmdBlankDate = new DelegateCommand<object>(cmdBlankDate_Execute);
            fromDate = toDate = DateTime.Now.Date;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        private void cmdView_Execute(object obj)
        {
            try
            {
                if (fromDate > toDate)
                {
                    UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                    return;
                }
                if ((toDate - fromDate).Days > 31)
                {
                    UIHelper.ShowErrorMessage("Days Difference should not be greater than 31");
                    return;
                }
                if (fromDate > toDate)
                {
                    UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                    return;
                }
                if (string.IsNullOrEmpty(creditNoteNo) && string.IsNullOrEmpty(orderID))
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
                    ObservableCollection<CreditNoteReport> lstInvRept = pxy.GenCreditNoteReport(creditNoteNo, orderID, fromDate, toDate);
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
                            ObservableCollection<CreditNoteReport> objTemp = new ObservableCollection<CreditNoteReport>
                        {
                            lstInvRept[i]
                        };
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetCompany", objComp));
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetCreditNoteReport", objTemp));
                            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dataSetInvoice", lstInvRept[i].lstDetails));
                            if (lstInvRept[i].lstDetails.Sum(p => p.IGST) > 0)
                            {
                                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0001_IGST.rdlc";
                            }
                            else
                            {
                                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0001_CSGST.rdlc";
                            }
                            reportViewer.RefreshReport();
                            Viewer.Child = reportViewer;
                            if (string.IsNullOrEmpty(creditNoteNo) && string.IsNullOrEmpty(orderID))
                            {
                                byte[] Bytes = reportViewer.LocalReport.Render(format: "PDF", deviceInfo: "");
                                using (FileStream stream = new FileStream(folderPath + "//" + lstInvRept[i].CrNoteNo + ".pdf", FileMode.Create))
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
            fromDate = toDate = new DateTime(1900, 01, 01);
        }
    }
}