using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Forms.Integration;

namespace IMS.ViewModels
{
    internal class InvoiceStatusViewModel : ViewModelBase
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

        private int _Status;

        public int Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                OnPropertyChanged(this, "Status");
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

        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set
            {
                _CategoryID = value;
                OnPropertyChanged(this, "_CategoryID");
            }
        }

        private int _SubCategoryID;

        public int SubCategoryID
        {
            get { return _SubCategoryID; }
            set
            {
                _SubCategoryID = value;
                OnPropertyChanged(this, "SubCategoryID");
            }
        }

        private int _GroupID;

        public int GroupID
        {
            get { return _GroupID; }
            set
            {
                _GroupID = value;
                OnPropertyChanged(this, "GroupID");
            }
        }

        private ObservableCollection<Lookup> _lstStatus;

        public ObservableCollection<Lookup> lstStatus
        {
            get { return _lstStatus; }
            set
            {
                _lstStatus = value;
                OnPropertyChanged(this, "lstStatus");
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

        private ObservableCollection<Lookup> _lstGroup;

        public ObservableCollection<Lookup> lstGroup
        {
            get { return _lstGroup; }
            set
            {
                _lstGroup = value;
                OnPropertyChanged(this, "lstGroup");
            }
        }

        private ObservableCollection<Category> _lstCategory;

        public ObservableCollection<Category> lstCategory
        {
            get { return _lstCategory; }
            set
            {
                _lstCategory = value;
                OnPropertyChanged(this, "lstCategory");
            }
        }

        private ObservableCollection<SubCategory> _lstSubCategory;

        public ObservableCollection<SubCategory> lstSubCategory
        {
            get { return _lstSubCategory; }
            set
            {
                _lstSubCategory = value;
                OnPropertyChanged(this, "lstSubCategory");
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
        public DelegateCommand<object> cmdCategory { get; set; }

        public InvoiceStatusViewModel()
        {
            try
            {
                if (pxy == null) pxy = new DataServiceClient();
                cmdCategory = new DelegateCommand<object>(cmdCategory_Execute);
                lstStatus = new ObservableCollection<Lookup>();
                lstStatus = pxy.GetSubLookupList(Const.InvoiceStatus);
                lstStatus.Add(new Lookup { ID = 0, Name = "--ALL--" });
                lstVendor = new ObservableCollection<VendorMaster>();
                lstVendor = pxy.GetVendorList();
                lstVendor.Add(new VendorMaster { ID = 0, Name = "--ALL--" });
                lstCategory = new ObservableCollection<Category>();
                lstCategory = pxy.GetCategoryList();
                lstCategory.Add(new Category { CategoryID = 0, Name = "--ALL--" });
                lstGroup = new ObservableCollection<Lookup>();
                lstGroup = pxy.GetSubLookupList(Const.ProductGroup);
                lstGroup.Add(new Lookup { ID = 0, Name = "--ALL--" });
                lstSubCategory = new ObservableCollection<SubCategory>
                {
                    new SubCategory { SubCategoryID = 0, CategoryID = 0, Name = "--ALL--" }
                };
                cmdView = new DelegateCommand<object>(cmdView_Execute);
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
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("datasetInvoiceStatus", pxy.StatuswiseInvoiceReport(fromInvoiceDate, toInvoiceDate, CategoryID, SubCategoryID, GroupID, VendorID, Status)));
                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0002.rdlc";
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

        private void cmdCategory_Execute(object obj)
        {
            lstSubCategory = new ObservableCollection<SubCategory>();
            lstSubCategory = pxy.GetSubCategoryList(CategoryID);
            lstSubCategory.Add(new SubCategory { CategoryID = 0, SubCategoryID = 0, Name = "--ALL--" });
        }
    }
}