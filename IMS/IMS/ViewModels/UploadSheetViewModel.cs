using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Windows;

namespace IMS.ViewModels
{
    internal class UploadSheetViewModel : ViewModelBase
    {
        public DelegateCommand<object> cmdUpload { get; set; }
        public DelegateCommand<object> cmdBrowse { get; set; }
        public DelegateCommand<object> cmdTemplate { get; set; }
        public DelegateCommand<object> cmdDownload { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        private readonly DataServiceClient pxy;

        private int _sheetType;

        public int sheetType
        {
            get { return _sheetType; }
            set
            {
                _sheetType = value;
                OnPropertyChanged(this, "sheetType");
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

        private string _fileName;

        public string fileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged(this, "fileName");
            }
        }

        private byte[] _imgTemplate;

        public byte[] imgTemplate
        {
            get { return _imgTemplate; }
            set
            {
                _imgTemplate = value;
                OnPropertyChanged(this, "imgTemplate");
            }
        }

        private ObservableCollection<Lookup> _lstSheetType;

        public ObservableCollection<Lookup> lstSheetType
        {
            get { return _lstSheetType; }
            set
            {
                _lstSheetType = value;
                OnPropertyChanged(this, "lstSheetType");
            }
        }

        private string safeFileName;

        public UploadSheetViewModel()
        {
            cmdUpload = new DelegateCommand<object>(cmdUpload_execute, cmdUpload_canExecute);
            cmdBrowse = new DelegateCommand<object>(cmdBrowse_execute, cmdBrowse_CanExecute);
            cmdTemplate = new DelegateCommand<object>(cmdTemplate_execute);
            cmdDownload = new DelegateCommand<object>(cmdDownload_Execute, cmdDownload_CanExecute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            pxy = new DataServiceClient();
            lstSheetType = pxy.GetSubLookupList(Const.SheetType);
            Busy = false;
            btnBrowse = true;
        }

        private bool cmdBrowse_CanExecute(object obj)
        {
            return btnBrowse;
        }

        private void cmdClear_Execute(object obj)
        {
            sheetType = 0;
            fileName = string.Empty;
            imgTemplate = null;
            Busy = false;
            btnBrowse = true;
            btnUpload = btnDownload = false;
        }

        private void cmdDownload_Execute(object obj)
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

        private bool cmdDownload_CanExecute(object obj)
        {
            return btnDownload;
        }

        private void cmdTemplate_execute(object obj)
        {
            try
            {
                if (sheetType > 0)
                {
                    imgTemplate = pxy.GetTemplate(sheetType);
                    MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to download template?", "Excel Template", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mresult == MessageBoxResult.Yes)
                    {
                        string sheetFileName = lstSheetType.First(p => p.ID == sheetType).Name + ".xlsx";
                        string urlPath = System.Configuration.ConfigurationManager.AppSettings["TemplatePath"].ToString() + sheetFileName;
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
                                wc.DownloadFileAsync(new Uri(urlPath), browse.SelectedPath + "\\" + sheetFileName);
                            }
                        }
                    }
                }
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdBrowse_execute(object obj)
        {
            // Initialize an OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set filter and RestoreDirectory
            openFileDialog.RestoreDirectory = true;

            bool? result = openFileDialog.ShowDialog();
            if (result == true && openFileDialog.FileName.Length > 0)
            {
                fileName = openFileDialog.FileName;
            }

            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                UIHelper.ShowMessage("The file is invalid. Please select an existing file again.");
            }
            else
            {
                safeFileName = openFileDialog.SafeFileName;
                btnUpload = true;
            }
        }

        private bool cmdUpload_canExecute(object obj)
        {
            if (sheetType > 0) return btnUpload;
            return false;
        }

        private void cmdUpload_execute(object obj)
        {
            try
            {
                TupleOfstringstringboolean objTuple = pxy.UploadSheet(IMSLibrary.Class.Helper.ConvertToByte(fileName.ToString()), safeFileName, Global.UserID, sheetType);
                ErrorFolder = objTuple.m_Item1;
                ErrorFile = objTuple.m_Item2;
                if (objTuple.m_Item3)
                {
                    UIHelper.ShowMessage("File uploaded successfully!");
                    btnUpload = btnBrowse = false;
                    btnDownload = true;
                    return;
                }
                UIHelper.ShowMessage("File not uploaded!");
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private string ErrorFile, ErrorFolder;
        private bool btnBrowse, btnUpload, btnDownload;
    }
}