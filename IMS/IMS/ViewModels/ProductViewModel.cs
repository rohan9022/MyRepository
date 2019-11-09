using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class ProductViewModel : ViewModelBase
    {
        private DataServiceClient pxy;
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
                }
                else
                {
                    lstSubCategory = new ObservableCollection<SubCategory>();
                    lstSubCategory = pxy.GetSubCategoryList(CategoryID);
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

        private Product _objProduct;

        public Product objProduct
        {
            get { return _objProduct; }
            set
            {
                _objProduct = value;
                OnPropertyChanged(this, "objProduct");
            }
        }

        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        public DelegateCommand<object> cmdProductDetails { get; set; }
        public DelegateCommand<object> cmdCategory { get; set; }

        public ProductViewModel(int PageMode)
        {
            try
            {
                Mode = PageMode;
                ModeAction();
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdProductDetails_Execute(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(objProduct.ProductID)) return;
                lstCategory = new ObservableCollection<Category>();
                lstCategory = pxy.GetCategoryList();
                lstSubCategory = new ObservableCollection<SubCategory>();
                objProduct = pxy.GetProductDetails(objProduct.ProductID);
                if (objProduct == null) objProduct = new Product();
                CategoryID = objProduct.Category;
                SubCategoryID = objProduct.SubCategory;
                if (Mode == PageMode.Modify) Ck = true;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdClear_Execute(object obj)
        {
            objProduct = new Product();
        }

        private void ModeAction()
        {
            try
            {
                pxy = new DataServiceClient();
                cmdProductDetails = new DelegateCommand<object>(cmdProductDetails_Execute);
                cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
                cmdClear = new DelegateCommand<object>(cmdClear_Execute);
                lstProduct = new ObservableCollection<ProductList>();
                lstProduct = pxy.GetProductList();
                lstGroup = new ObservableCollection<Lookup>();
                lstGroup = pxy.GetSubLookupList(Const.ProductGroup);
                lstCategory = new ObservableCollection<Category>();
                lstCategory = pxy.GetCategoryList();
                objProduct = new Product();
                switch (Mode)
                {
                    case PageMode.Add:
                        {
                            Pk = false;
                            Ck = true;
                            modeContent = "Save";
                        }
                        break;

                    case PageMode.Modify:
                        {
                            Pk = true;
                            Ck = false;
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
            return true;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(objProduct.ProductID) && Mode != PageMode.Add)
                {
                    UIHelper.ShowErrorMessage("Select Existing Product");
                    return;
                }
                if (Mode == PageMode.Add || Mode == PageMode.Modify)
                {
                    objProduct.Category = CategoryID;
                    objProduct.SubCategory = SubCategoryID;
                    if (pxy.InsertUpdateProductMaster(objProduct, Global.UserID))
                    {
                        UIHelper.ShowMessage("Data Saved Successfully");
                    }
                }
                if (Mode == PageMode.Delete && pxy.DeleteProduct(objProduct.ProductID))
                {
                    UIHelper.ShowMessage("Data Saved Successfully");
                }
                ModeAction();
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