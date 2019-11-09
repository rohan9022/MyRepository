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
    internal class DamageViewModel : ViewModelBase
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
            get { return _objDamaged.ProductID; }
            set
            {
                _objDamaged.ProductID = value;
                OnPropertyChanged(this, "ProductID");
            }
        }

        public DateTime DamagedDate
        {
            get { return _objDamaged.DamageDate; }
            set
            {
                _objDamaged.DamageDate = value;
                OnPropertyChanged(this, "DamagedDate");
            }
        }

        public int Quantity
        {
            get { return _objDamaged.Quantity; }
            set
            {
                _objDamaged.Quantity = value;
                OnPropertyChanged(this, "Quantity");
            }
        }

        public decimal Price
        {
            get { return _objDamaged.Price; }
            set
            {
                _objDamaged.Price = value;
                OnPropertyChanged(this, "Price");
            }
        }

        public string Comment
        {
            get { return _objDamaged.Comment; }
            set
            {
                _objDamaged.Comment = value;
                OnPropertyChanged(this, "Comment");
            }
        }

        private Damage _objDamaged;

        public Damage objDamaged
        {
            get { return _objDamaged; }
            set
            {
                _objDamaged = value;
                ProductID = value.ProductID;
                DamagedDate = value.DamageDate;
                Quantity = value.Quantity;
                Price = value.Price;
                Comment = value.Comment;
            }
        }

        private ObservableCollection<Damage> _lstDamaged;

        public ObservableCollection<Damage> lstDamaged
        {
            get { return _lstDamaged; }
            set
            {
                _lstDamaged = value;
                OnPropertyChanged(this, "lstDamaged");
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
        public DelegateCommand<object> cmdDamagedDetails { get; set; }
        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        private bool btn;

        public DamageViewModel(int PageMode)
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
                objDamaged = new Damage();
                lstDamaged = new ObservableCollection<Damage>();
                lstProduct = new ObservableCollection<ProductList>();
                lstProduct = pxy.GetProductList();
                ModeAction();
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute, cmdSearch_CanExecute);
                cmdDamagedDetails = new DelegateCommand<object>(cmdDamagedDetails_Execute, cmdDamagedDetails_CanExecute);
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
            objDamaged = new Damage();
            if (Mode == PageMode.Add) btn = true;
        }

        private bool cmdDamagedDetails_CanExecute(object obj)
        {
            if (Mode == PageMode.Add) return false;
            return true;
        }

        private void cmdDamagedDetails_Execute(object obj)
        {
            try
            {
                if (dgSelectedIndex < 0) return;
                objDamaged = lstDamaged[dgSelectedIndex];
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
                            DamagedDate = DateTime.Now.Date;
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
                if (pxy.InsertUpdateDeleteDamage(Mode, objDamaged, Global.UserID))
                {
                    UIHelper.ShowMessage("Data Saved Successfully");
                }
                objDamaged = new Damage();
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
                lstDamaged = pxy.GetDamagedGoodsDetails(searchProduct, searchDate);
                if (lstDamaged.Any())
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