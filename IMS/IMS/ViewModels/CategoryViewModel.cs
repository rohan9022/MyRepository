using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class CategoryViewModel : ViewModelBase, IDataErrorInfo
    {
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

        private bool _Pk;

        public bool Pk
        {
            get { return _Pk; }
            set
            {
                _Pk = value;
                OnPropertyChanged(this, "Pk");
            }
        }

        private bool _Ck;

        public bool Ck
        {
            get { return _Ck; }
            set
            {
                _Ck = value;
                OnPropertyChanged(this, "Ck");
            }
        }

        private string _categoryName;

        public string categoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged(this, "categoryName");
            }
        }

        private int _categoryID;

        public int categoryID
        {
            get { return _categoryID; }
            set
            {
                _categoryID = value;
                OnPropertyChanged(this, "categoryID");
            }
        }

        private decimal _cgst;

        public decimal cgst
        {
            get { return _cgst; }
            set
            {
                _cgst = value;
                OnPropertyChanged(this, "cgst");
            }
        }

        private decimal _sgst;

        public decimal sgst
        {
            get { return _sgst; }
            set
            {
                _sgst = value;
                OnPropertyChanged(this, "sgst");
            }
        }

        private decimal _igst;

        public decimal igst
        {
            get { return _igst; }
            set
            {
                _igst = value;
                OnPropertyChanged(this, "igst");
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

        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdCategoryDetails { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }

        private readonly DataServiceClient pxy;

        public CategoryViewModel(int PageMode)
        {
            Mode = PageMode;
            pxy = new DataServiceClient();
            cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExeute);
            cmdCategoryDetails = new DelegateCommand<object>(cmdCategoryDetails_Execute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            lstCategory = new ObservableCollection<Category>();
            lstCategory = pxy.GetCategoryList();
            ModeAction();
        }

        private void cmdClear_Execute(object obj)
        {
            categoryName = string.Empty;
            cgst = 0;
            sgst = 0;
            igst = 0;
            ModeAction();
        }

        private void cmdCategoryDetails_Execute(object obj)
        {
            try
            {
                Category objCategory = lstCategory[System.Convert.ToInt32(obj)];
                categoryName = objCategory.Name;
                cgst = objCategory.CGST;
                sgst = objCategory.SGST;
                igst = objCategory.IGST;
                if (Mode == PageMode.Modify)
                {
                    Pk = false;
                    Ck = true;
                }
            }
            catch
            {
                // don't do anything here
            }
        }

        private bool cmdSave_CanExeute(object obj)
        {
            if (Mode == PageMode.Browse) return true;
            if (Mode == PageMode.Add) return true;
            if (Mode == PageMode.Modify && categoryID > 0) return true;
            if (Mode == PageMode.Delete && categoryID > 0) return true;
            return false;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify)
                {
                    if (!pxy.CheckCategoryName(categoryName, categoryID, Mode))
                    {
                        UIHelper.ShowErrorMessage("Same category name already exists");
                        return;
                    }
                    if (pxy.InsertUpdateCategory(categoryName, categoryID, cgst, sgst, igst))
                    {
                        UIHelper.ShowMessage("Data Saved Successfully");
                        categoryName = string.Empty;
                        categoryID = 0;
                        cgst = 0;
                        sgst = 0;
                        igst = 0;
                        lstCategory = pxy.GetCategoryList();
                    }
                }
                else if (Mode == PageMode.Delete)
                {
                    if (pxy.DeleteCategory(categoryID))
                    {
                        UIHelper.ShowMessage("Data Deleted Successfully");
                    }
                }
                else if (Mode == PageMode.Browse)
                {
                    categoryName = string.Empty;
                    categoryID = 0;
                }
                ModeAction();
            }
            catch (FaultException ex)
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

        public string Error
        {
            get { throw new System.NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string Result = string.Empty;
                switch (columnName)
                {
                    case "categoryID": if (categoryID <= 0 && Mode != PageMode.Add) Result = "Select Category ID"; break;
                    case "categoryName": if (string.IsNullOrEmpty(categoryName)) Result = "Enter Category Name"; break;
                }
                return Result;
            }
        }
    }
}