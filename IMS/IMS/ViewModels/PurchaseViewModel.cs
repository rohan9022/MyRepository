using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class PurchaseViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;
        private readonly int Mode;
        private string _modeContent;

        public string modeContent
        {
            get { return _modeContent; }
            set
            {
                _modeContent = value;
                OnPropertyChanged(this, "modeContent");
            }
        }

        private bool _Pk { get; set; }

        public bool Pk
        {
            get { return _Pk; }
            set
            {
                _Pk = value;
                OnPropertyChanged(this, "Pk");
            }
        }

        private bool _Ck { get; set; }

        public bool Ck
        {
            get { return _Ck; }
            set
            {
                _Ck = value;
                OnPropertyChanged(this, "Ck");
            }
        }

        private bool _prd;

        public bool prd
        {
            get { return _prd; }
            set
            {
                _prd = value;
                OnPropertyChanged(this, "prd");
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

        public string ProductID
        {
            get { return _objPurchase.ProductID; }
            set
            {
                _objPurchase.ProductID = value;
                OnPropertyChanged(this, "ProductID");
            }
        }

        public DateTime PurchaseDate
        {
            get { return _objPurchase.PurchaseDate; }
            set
            {
                _objPurchase.PurchaseDate = value;
                OnPropertyChanged(this, "PurchaseDate");
            }
        }

        public int Quantity
        {
            get { return _objPurchase.Quantity; }
            set
            {
                _objPurchase.Quantity = value;
                OnPropertyChanged(this, "Quantity");
            }
        }

        public decimal MRP
        {
            get { return _objPurchase.MRP; }
            set
            {
                _objPurchase.MRP = value;
                OnPropertyChanged(this, "MRP");
            }
        }

        public decimal PP
        {
            get { return _objPurchase.PP; }
            set
            {
                _objPurchase.PP = value;
                OnPropertyChanged(this, "PP");
            }
        }

        public decimal SP
        {
            get { return _objPurchase.SP; }
            set
            {
                _objPurchase.SP = value;
                OnPropertyChanged(this, "SP");
            }
        }

        private Purchase _objPurchase;

        public Purchase objPurchase
        {
            get { return _objPurchase; }
            set
            {
                _objPurchase = value;
                ProductID = value.ProductID;
                PurchaseDate = value.PurchaseDate;
                Quantity = value.Quantity;
                MRP = value.MRP;
                PP = value.PP;
                SP = value.SP;
            }
        }

        private ObservableCollection<Purchase> _lstPurchase;

        public ObservableCollection<Purchase> lstPurchase
        {
            get { return _lstPurchase; }
            set
            {
                _lstPurchase = value;
                OnPropertyChanged(this, "lstPurchase");
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

        public int _dgSelectedIndex;

        public int dgSelectedIndex
        {
            get { return _dgSelectedIndex; }
            set
            {
                _dgSelectedIndex = value;
                OnPropertyChanged(this, "dgSelectedIndex");
            }
        }

        public DelegateCommand<object> cmdSearch { get; set; }
        public DelegateCommand<object> cmdPurchaseDetails { get; set; }
        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        private bool btn;

        public PurchaseViewModel(int PageMode)
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
                objPurchase = new Purchase();
                lstPurchase = new ObservableCollection<Purchase>();
                lstProduct = new ObservableCollection<ProductList>();
                lstProduct = pxy.GetProductList();
                ModeAction();
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute, cmdSearch_CanExecute);
                cmdPurchaseDetails = new DelegateCommand<object>(cmdPurchaseDetails_Execute, cmdPurchaseDetails_CanExecute);
                cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
                cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdClear_Execute(object obj)
        {
            objPurchase = new Purchase();
            if (Mode == PageMode.Add) btn = true;
        }

        private bool cmdPurchaseDetails_CanExecute(object obj)
        {
            if (Mode == PageMode.Add) return false;
            return true;
        }

        private void cmdPurchaseDetails_Execute(object obj)
        {
            try
            {
                if (dgSelectedIndex < 0) return;
                objPurchase = lstPurchase[dgSelectedIndex];
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void ModeAction()
        {
            try
            {
                switch (Mode)
                {
                    case PageMode.Add:
                        {
                            Pk = false;
                            Ck = true;
                            PurchaseDate = DateTime.Now.Date;
                            prd = true;
                            btn = true;
                            modeContent = "Save";
                        }
                        break;

                    case PageMode.Modify:
                        {
                            Pk = true;
                            Ck = false;
                            prd = false;
                            modeContent = "Update";
                        }
                        break;

                    case PageMode.Delete:
                        {
                            Pk = true;
                            Ck = false;
                            modeContent = "Delete";
                        }
                        break;

                    case PageMode.Browse:
                        {
                            Pk = true;
                            Ck = false;
                            modeContent = "Ok";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdSave_CanExecute(object obj)
        {
            if (Mode == PageMode.Browse) return false;
            return btn;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(ProductID))
                {
                    UIHelper.ShowErrorMessage("Select Existing Product");
                    return;
                }
                if (pxy.InsertUpdateDeletePurchase(Mode, objPurchase, Global.UserID))
                {
                    UIHelper.ShowMessage("Data Saved Successfully");
                }
                objPurchase = new Purchase();
                btn = false;
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

        private bool cmdSearch_CanExecute(object obj)
        {
            if (Mode != PageMode.Add) return true;
            return false;
        }

        private void cmdSearch_Execute(object obj)
        {
            try
            {
                if (searchMode == 1) { searchProduct = string.Empty; }
                if (searchMode == 2) { searchDate = new DateTime(1900, 01, 01); }
                lstPurchase = pxy.GetPurchaseDetails(searchProduct, searchDate);
                if (lstPurchase.Any())
                {
                    if (Mode == PageMode.Modify)
                    {
                        Ck = true;
                        prd = false;
                    }
                    btn = true;
                    return;
                }
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