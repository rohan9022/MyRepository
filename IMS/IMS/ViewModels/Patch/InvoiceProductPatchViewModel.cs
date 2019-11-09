using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.InvoiceServiceRef;
using IMS.Models;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class InvoiceProductPatchViewModel : ViewModelBase
    {
        #region ==Properties && Members==

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

        public DelegateCommand<object> cmdGenerateExcel { get; set; }
        public DelegateCommand<object> cmdUploadExcel { get; set; }
        public DelegateCommand<object> cmdErrorFile { get; set; }
        public DelegateCommand<object> cmdBlankDate { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        private ObservableCollection<ProductList> _lstProduct;

        public ObservableCollection<ProductList> lstProduct
        {
            get { return _lstProduct; }
            set
            {
                _lstProduct = value;
                OnPropertyChanged(this, "lstProduct");
            }
        }

        private string _ProductID;

        public string ProductID
        {
            get { return _ProductID; }
            set
            {
                _ProductID = value;
                OnPropertyChanged(this, "ProductID");
            }
        }

        private bool _Busy;

        public bool Busy
        {
            get { return _Busy; }
            set
            {
                _Busy = value;
                OnPropertyChanged(this, "Busy");
            }
        }

        private string _PercentageStr;

        public string PercentageStr
        {
            get { return _PercentageStr; }
            set
            {
                _PercentageStr = value;
                OnPropertyChanged(this, "PercentageStr");
            }
        }

        private int _Percentage;

        public int Percentage
        {
            get { return _Percentage; }
            set
            {
                _Percentage = value;
                OnPropertyChanged(this, "Percentage");
            }
        }

        private readonly InvoiceServiceClient pxyInvoice;
        private string ErrorFile, ErrorFolder;
        private bool btnUpload, btnErrorFile;

        #endregion ==Properties && Members==

        public InvoiceProductPatchViewModel()
        {
            if (pxyInvoice == null) pxyInvoice = new InvoiceServiceClient();
            lstProduct = new ObservableCollection<ProductList>();
            using (DataServiceClient pxyData = new DataServiceClient())
            {
                lstProduct = pxyData.GetProductList();
            }
            cmdGenerateExcel = new DelegateCommand<object>(cmdGenerateExcel_Execute);
            cmdUploadExcel = new DelegateCommand<object>(cmdUploadExcel_Execute, cmdUploadExcel_CanExecute);
            cmdErrorFile = new DelegateCommand<object>(cmdErrorFile_Execute, cmdErrorFile_CanExecute);
            cmdBlankDate = new DelegateCommand<object>(cmdBlankDate_Execute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            ClearValues();
        }

        #region ==Clear Values==

        private void cmdClear_Execute(object obj)
        {
            ClearValues();
        }

        private void ClearValues()
        {
            fromInvoiceNo = toInvoiceNo = 0;
            fromInvoiceDate = toInvoiceDate = new DateTime(1900, 01, 01);
            ProductID = string.Empty;
            Busy = btnErrorFile = false;
            btnUpload = true;
        }

        private void cmdBlankDate_Execute(object obj)
        {
            fromInvoiceDate = toInvoiceDate = new DateTime(1900, 01, 01);
        }

        #endregion ==Clear Values==

        #region ==Upload==

        private void cmdUploadExcel_Execute(object obj)
        {
            try
            {
                string fileName = string.Empty;
                string safeFileName = string.Empty;
                // Initialize an OpenFileDialog
                OpenFileDialog openFileDialog = new OpenFileDialog();

                // Set filter and RestoreDirectory
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Excel documents(*.xlsx)|*.xlsx";

                bool? result = openFileDialog.ShowDialog();
                if (result == true && openFileDialog.FileName.Length > 0)
                {
                    fileName = openFileDialog.FileName;
                }

                if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                {
                    return;
                }

                safeFileName = openFileDialog.SafeFileName;

                IMS.InvoiceServiceRef.TupleOfstringstringboolean objTuple = pxyInvoice.UploadPatchSheet(IMSLibrary.Class.Helper.ConvertToByte(fileName.ToString()), safeFileName, Global.UserID, Const.InvoiceProductSheetType);
                ErrorFolder = objTuple.m_Item1;
                ErrorFile = objTuple.m_Item2;
                if (objTuple.m_Item3)
                {
                    UIHelper.ShowMessage("File uploaded successfully!");
                    btnUpload = false;
                    btnErrorFile = true;
                    return;
                }
                UIHelper.ShowMessage("File not uploaded!");
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdUploadExcel_CanExecute(object obj)
        {
            return btnUpload;
        }

        #endregion ==Upload==

        #region ==Download Error File==

        private bool cmdErrorFile_CanExecute(object obj)
        {
            return btnErrorFile;
        }

        private void cmdErrorFile_Execute(object obj)
        {
            using (System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog())
            {
                browse.ShowDialog();
                if (string.IsNullOrEmpty(browse.SelectedPath))
                {
                    UIHelper.ShowErrorMessage("Select Folder");
                    return;
                }
                Busy = true;
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileAsync(new Uri(Const.HostedPath + ErrorFolder + ErrorFile), browse.SelectedPath + "\\" + ErrorFile);
                }
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Percentage = e.ProgressPercentage;
            PercentageStr = Percentage.ToString() + "%";
        }

        private void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            UIHelper.ShowMessage("Download Completed");
            Busy = false;
        }

        #endregion ==Download Error File==

        #region ==Generate Excel==

        private void cmdGenerateExcel_Execute(object obj)
        {
            try
            {
                string folderPath = string.Empty;
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
                if (fromInvoiceNo > toInvoiceNo)
                {
                    UIHelper.ShowErrorMessage("From Invoice No. cannot not be greater than To Invoice No.");
                    return;
                }
                using (System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog())
                {
                    browse.ShowDialog();
                    folderPath = browse.SelectedPath;
                }
                if (string.IsNullOrEmpty(folderPath))
                {
                    UIHelper.ShowErrorMessage("Select Path to save excel file");
                    return;
                }

                using (InvoiceServiceClient pxy = new InvoiceServiceClient())
                {
                    ObservableCollection<IMS.InvoiceServiceRef.ProductDetails> lstInvoiceProduct = pxy.InvoiceProductPatch(fromInvoiceNo, toInvoiceNo, fromInvoiceDate, toInvoiceDate, ProductID, Global.UserID);
                    if (lstInvoiceProduct == null || lstInvoiceProduct.Count == 0)
                    {
                        UIHelper.ShowMessage("Data not found");
                        return;
                    }
                    using (ReportViewer reportViewer = new ReportViewer())
                    {
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(new ReportDataSource("InvoiceProductDataSet", lstInvoiceProduct));
                        reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//InvoiceProductPatch.rdlc";
                        reportViewer.RefreshReport();
                        byte[] Bytes = reportViewer.LocalReport.Render(format: "Excel", deviceInfo: "");
                        using (FileStream stream = new FileStream(folderPath + "//InvoiceProductPatch.xls", FileMode.Create))
                        {
                            stream.Write(Bytes, 0, Bytes.Length);
                        }
                    }
                }
                UIHelper.ShowMessage("File generated!");
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

        #endregion ==Generate Excel==
    }
}