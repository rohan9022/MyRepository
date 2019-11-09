using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Forms.Integration;
using System.Linq;

namespace IMS.ViewModels
{
    internal class SalesReportViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;
        private DateTime _fromSalesDate;

        public DateTime fromSalesDate
        {
            get { return _fromSalesDate; }
            set
            {
                _fromSalesDate = value;
                OnPropertyChanged(this, "fromSalesDate");
            }
        }

        private DateTime _toSalesDate;

        public DateTime toSalesDate
        {
            get { return _toSalesDate; }
            set
            {
                _toSalesDate = value;
                OnPropertyChanged(this, "toSalesDate");
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

        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set
            {
                _CategoryID = value;
                OnPropertyChanged(this, "CategoryID");
                if (CategoryID == 0)
                {
                    lstSubCategory = new ObservableCollection<SubCategory>();
                    lstSubCategory.Add(new SubCategory { SubCategoryID = 0, CategoryID = 0, Name = "--ALL--" });
                }
                else
                {
                    lstSubCategory = new ObservableCollection<SubCategory>();
                    lstSubCategory = pxy.GetSubCategoryList(CategoryID);
                    lstSubCategory.Add(new SubCategory { SubCategoryID = 0, CategoryID = 0, Name = "--ALL--" });
                }
                OnPropertyChanged(this, "SubCategoryID");
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

        private string _PrdCd;

        public string PrdCd
        {
            get { return _PrdCd; }
            set
            {
                _PrdCd = value;
                OnPropertyChanged(this, "PrdCd");
            }
        }

        private ObservableCollection<ProductList> _lstPrdCd;

        public ObservableCollection<ProductList> lstPrdCd
        {
            get { return _lstPrdCd; }
            set
            {
                _lstPrdCd = value;
                OnPropertyChanged(this, "lstPrdCd");
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
        public DelegateCommand<object> cmdMonth { get; set; }
        public DelegateCommand<object> cmdVendor { get; set; }
        private ObservableCollection<SalesReport> lstSalesReport;
        private int _Count;

        public int Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(this, "Count");
            }
        }

        private int _Quantity;

        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                OnPropertyChanged(this, "Quantity");
            }
        }

        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                OnPropertyChanged(this, "Amount");
            }
        }

        public SalesReportViewModel()
        {
            try
            {
                if (pxy == null) pxy = new DataServiceClient();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstVendor = pxy.GetVendorList();
                lstVendor.Add(new VendorMaster { ID = 0, Name = "--ALL--" });
                lstPrdCd = new ObservableCollection<ProductList>();
                lstPrdCd = pxy.GetProductList();
                lstPrdCd.Add(new ProductList { ID = string.Empty, Name = "--ALL--" });
                lstGroup = new ObservableCollection<Lookup>();
                lstGroup = pxy.GetSubLookupList(Const.ProductGroup);
                lstGroup.Add(new Lookup { ID = 0, Name = "--ALL--" });
                lstCategory = new ObservableCollection<Category>();
                lstCategory = pxy.GetCategoryList();
                lstCategory.Add(new Category { CategoryID = 0, Name = "--ALL--" });
                lstSubCategory = new ObservableCollection<SubCategory>();
                cmdView = new DelegateCommand<object>(cmdView_Execute);
                fromSalesDate = toSalesDate = DateTime.Now.Date;
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
                if (fromSalesDate > toSalesDate)
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
                lstSalesReport = pxy.Rpt_Sales(fromSalesDate, toSalesDate, PrdCd, VendorID, GroupID, CategoryID, SubCategoryID);
                Count = lstSalesReport.Count;
                Quantity = lstSalesReport.Sum(p => p.Quantity);
                Amount = lstSalesReport.Sum(p => p.InvoiceAmount);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("datasetSalesReport", lstSalesReport));
                reportViewer.LocalReport.ReportPath = System.Windows.Forms.Application.StartupPath + "//Views//Reports//I0005.rdlc";
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
    }
}