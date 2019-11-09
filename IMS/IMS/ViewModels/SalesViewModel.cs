using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Linq;

namespace IMS.ViewModels
{
    internal class SalesViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;
        private readonly int Mode;

        private ObservableCollection<Search> _lstSearch;

        public ObservableCollection<Search> lstSearch
        {
            get { return _lstSearch; }
            set
            {
                _lstSearch = value;
                OnPropertyChanged(this, "lstSearch");
            }
        }

        private int _searchMode;

        public int searchMode
        {
            get { return _searchMode; }
            set
            {
                _searchMode = value;
                OnPropertyChanged(this, "searchMode");
            }
        }

        private string _searchVal;

        public string searchVal
        {
            get { return _searchVal; }
            set
            {
                _searchVal = value;
                OnPropertyChanged(this, "searchVal");
            }
        }

        private string _searchProduct;

        public string searchProduct
        {
            get { return _searchProduct; }
            set
            {
                _searchProduct = value;
                OnPropertyChanged(this, "searchProduct");
            }
        }

        private DateTime _searchDate;

        public DateTime searchDate
        {
            get { return _searchDate; }
            set
            {
                _searchDate = value;
                OnPropertyChanged(this, "searchDate");
            }
        }

        private ObservableCollection<Sales> _lstSales;

        public ObservableCollection<Sales> lstSales
        {
            get { return _lstSales; }
            set
            {
                _lstSales = value;
                OnPropertyChanged(this, "lstSales");
            }
        }

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

        public DelegateCommand<object> cmdSearch { get; set; }

        public SalesViewModel(int PageMode)
        {
            try
            {
                Mode = PageMode;
                searchDate = DateTime.Now.Date;
                pxy = new DataServiceClient();
                lstSearch = new ObservableCollection<Search>
                {
                    new Search { ID = 1, Desc = "Date" },
                    new Search { ID = 2, Desc = "Product" },
                    new Search { ID = 3, Desc = "Both" }
                };
                searchMode = 1;
                lstSales = new ObservableCollection<Sales>();
                lstProduct = new ObservableCollection<ProductList>();
                lstProduct = pxy.GetProductList();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstVendor = pxy.GetVendorList();
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute);
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdSearch_Execute(object obj)
        {
            try
            {
                if (searchMode == 1) { searchProduct = string.Empty; }
                if (searchMode == 2) { searchDate = new DateTime(1900, 01, 01); }
                lstSales = pxy.GetSalesDetails(searchProduct, searchDate);
                if (lstSales.Any()) return;
                UIHelper.ShowErrorMessage("Data not found");
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