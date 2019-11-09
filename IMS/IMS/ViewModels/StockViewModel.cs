using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class StockViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;
        public DelegateCommand<object> cmdSearch { get; set; }
        private string _ProductName;

        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                _ProductName = value;
                OnPropertyChanged(this, "ProductName");
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

        public ObservableCollection<ProductList> _lstProduct;

        public ObservableCollection<ProductList> lstProduct
        {
            get { return _lstProduct; }
            set
            {
                _lstProduct = value;
                OnPropertyChanged(this, "lstProduct");
            }
        }

        private ObservableCollection<Product> _lstStock;

        public ObservableCollection<Product> lstStock
        {
            get { return _lstStock; }
            set
            {
                _lstStock = value;
                OnPropertyChanged(this, "lstStock");
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

        public StockViewModel()
        {
            pxy = new DataServiceClient();
            cmdSearch = new DelegateCommand<object>(cmdSearch_Execute);
            lstProduct = new ObservableCollection<ProductList>();
            lstProduct = pxy.GetProductList();
            lstStock = new ObservableCollection<Product>();
            lstSearch = new ObservableCollection<Search>
            {
                new Search { ID = 1, Val = "<" },
                new Search { ID = 2, Val = ">" },
                new Search { ID = 3, Val = "=" },
                new Search { ID = 4, Val = ">=" },
                new Search { ID = 5, Val = "<=" }
            };
            searchMode = 3;
        }

        private void cmdSearch_Execute(object obj)
        {
            try
            {
                lstStock = pxy.CheckStock(Quantity, ProductID, searchMode);
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }
    }
}