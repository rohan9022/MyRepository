using IMS.Helper;
using IMSLibrary.UI;
using System.Collections.ObjectModel;
using IMS.DataServiceRef;
using System;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class SubCategoryViewModel : ViewModelBase
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

        private int _subCategoryID;

        public int subCategoryID
        {
            get { return _subCategoryID; }
            set
            {
                _subCategoryID = value;
                OnPropertyChanged(this, "subCategoryID");
            }
        }

        private string _SubCategoryName;

        public string SubCategoryName
        {
            get { return _SubCategoryName; }
            set
            {
                _SubCategoryName = value;
                OnPropertyChanged(this, "SubCategoryName");
            }
        }

        private int _dgSelectedIndex;

        public int dgSelectedIndex
        {
            get { return _dgSelectedIndex; }
            set
            {
                _dgSelectedIndex = value;
                OnPropertyChanged(this, "dgSelectedIndex");
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

        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        public DelegateCommand<object> cmdCategory { get; set; }
        public DelegateCommand<object> cmdSubCategory { get; set; }

        public SubCategoryViewModel(int PageMode)
        {
            Mode = PageMode;
            if (pxy == null) pxy = new DataServiceClient();
            cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            cmdCategory = new DelegateCommand<object>(cmdCategory_Execute, cmdCategory_CanExecute);
            cmdSubCategory = new DelegateCommand<object>(cmdSubCategory_Execute, cmdSubCategory_CanExecute);
            lstCategory = new ObservableCollection<Category>();
            lstSubCategory = new ObservableCollection<SubCategory>();
            lstCategory = pxy.GetCategoryList();
            ModeAction();
        }

        private void cmdCategory_Execute(object obj)
        {
            try
            {
                lstSubCategory = pxy.GetSubCategoryList(categoryID);
                if (lstSubCategory == null || lstSubCategory.Count == 0)
                {
                    SubCategoryName = string.Empty;
                    UIHelper.ShowMessage("Data not found");
                }
                dgSelectedIndex = 0;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdCategory_CanExecute(object obj)
        {
            return true;
        }

        private void cmdSubCategory_Execute(object obj)
        {
            try
            {
                if (Mode != PageMode.Add)
                {
                    if (dgSelectedIndex < 0)
                    {
                        SubCategoryName = string.Empty;
                        return;
                    }
                    SubCategoryName = lstSubCategory[dgSelectedIndex].Name;
                    if (Mode == PageMode.Modify) Ck = true;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdSubCategory_CanExecute(object obj)
        {
            return true;
        }

        private void cmdClear_Execute(object obj)
        {
            lstSubCategory = new ObservableCollection<SubCategory>();
            categoryID = 0;
            subCategoryID = 0;
            SubCategoryName = string.Empty;
        }

        private bool cmdSave_CanExecute(object obj)
        {
            if (Mode == PageMode.Add && !string.IsNullOrEmpty(SubCategoryName)) return true;
            if (Mode == PageMode.Modify && dgSelectedIndex >= 0 && !string.IsNullOrEmpty(SubCategoryName)) return true;
            if (Mode == PageMode.Delete && dgSelectedIndex >= 0 && lstSubCategory.Count > 0) return true;
            if (Mode == PageMode.Browse && dgSelectedIndex >= 0 && lstSubCategory.Count > 0) return true;
            return false;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify)
                {
                    if (!pxy.CheckSubCategoryName(SubCategoryName, categoryID))
                    {
                        UIHelper.ShowErrorMessage("Same sub-category already exists");
                        return;
                    }
                    if (Mode == PageMode.Add) subCategoryID = 0;
                    if (pxy.InsertUpdateSubCategory(SubCategoryName, categoryID, subCategoryID))
                    {
                        UIHelper.ShowMessage("Data Saved Successfully");
                        lstSubCategory = pxy.GetSubCategoryList(categoryID);
                    }
                }
                else if (Mode == PageMode.Delete)
                {
                    if (pxy.DeleteSubCategory(categoryID, subCategoryID))
                    {
                        UIHelper.ShowMessage("Data Deleted Successfully");
                        lstSubCategory = pxy.GetSubCategoryList(categoryID);
                    }
                }
                else if (Mode == PageMode.Browse)
                {
                    SubCategoryName = string.Empty;
                    subCategoryID = 0;
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
    }
}