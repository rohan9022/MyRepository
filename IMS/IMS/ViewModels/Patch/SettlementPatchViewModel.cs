using IMSLibrary.UI;
using IMS.Helper;
using IMS.InvoiceServiceRef;
using IMS.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class SettlementPatchViewModel : ViewModelBase
    {
        #region ==Properties && Members==

        public DelegateCommand<object> cmdUploadExcel { get; set; }
        public DelegateCommand<object> cmdErrorFile { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
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

        public SettlementPatchViewModel()
        {
            if (pxyInvoice == null) pxyInvoice = new InvoiceServiceClient();
            cmdUploadExcel = new DelegateCommand<object>(cmdUploadExcel_Execute, cmdUploadExcel_CanExecute);
            cmdErrorFile = new DelegateCommand<object>(cmdErrorFile_Execute, cmdErrorFile_CanExecute);
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
            Busy = btnErrorFile = false;
            btnUpload = true;
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

                TupleOfstringstringboolean objTuple = pxyInvoice.UploadSheetUpdateDelete(IMSLibrary.Class.Helper.ConvertToByte(fileName.ToString()), safeFileName, Global.UserID, Const.SettlementSheetType);
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
    }
}