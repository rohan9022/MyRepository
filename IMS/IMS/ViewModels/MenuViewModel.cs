using IMS.Helper;
using IMS.DataServiceRef;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxy;

        private ObservableCollection<Lookup> _lstScreen;

        public ObservableCollection<Lookup> lstScreen
        {
            get { return _lstScreen; }
            set
            {
                _lstScreen = value;
                OnPropertyChanged(this, "lstScreen");
            }
        }

        #region ==TreeView==

        private ObservableCollection<Models.MenuMaster> _lstMenuMaster;

        public ObservableCollection<Models.MenuMaster> lstMenuMaster
        {
            get { return _lstMenuMaster; }
            set
            {
                _lstMenuMaster = value;
                OnPropertyChanged(this, "lstMenuMaster");
            }
        }

        private int _ctxID;

        public int ctxID
        {
            get { return _ctxID; }
            set
            {
                _ctxID = value;
                OnPropertyChanged(this, "ctxID");
            }
        }

        private void GetMenuData()
        {
            ObservableCollection<Models.MenuMaster> lstTemp = new ObservableCollection<Models.MenuMaster>();
            foreach (MenuMaster item in pxy.GetMenuList(3))
            {
                lstTemp.Add(new Models.MenuMaster
                {
                    ID = item.ID,
                    IsActive = item.IsActive,
                    Name = item.Name,
                    ParentID = item.ParentID,
                    ScreenID = item.ScreenID,
                    lstChild = new ObservableCollection<Models.MenuMaster>()
                });
            }
            lstMenuMaster = new ObservableCollection<Models.MenuMaster>();
            lstMenuMaster = new ObservableCollection<Models.MenuMaster>(lstTemp.Where(p => p.ParentID == 0).ToList());
            foreach (Models.MenuMaster item in lstMenuMaster)
            {
                item.lstChild = new ObservableCollection<Models.MenuMaster>(AddChildItems(lstTemp, item));
            }
        }

        private ObservableCollection<Models.MenuMaster> AddChildItems(ObservableCollection<Models.MenuMaster> lstMenu, Models.MenuMaster menuItem)
        {
            ObservableCollection<Models.MenuMaster> lstChild = new ObservableCollection<Models.MenuMaster>(lstMenu.Where(p => p.ParentID == Convert.ToInt32(menuItem.ID)).ToList());
            foreach (Models.MenuMaster childItem in lstChild)
            {
                childItem.lstChild = new ObservableCollection<Models.MenuMaster>(AddChildItems(lstMenu, childItem));
            }
            return lstChild;
        }

        #endregion ==TreeView==

        public DelegateCommand<object> cmdContextMenu { get; set; }
        public DelegateCommand<object> cmdID { get; set; }
        public DelegateCommand<object> cmdAddUpdate { get; set; }

        public MenuViewModel()
        {
            if (pxy == null) pxy = new DataServiceClient();
            lstMenuMaster = new ObservableCollection<Models.MenuMaster>();
            lstScreen = new ObservableCollection<Lookup>(GetScreenList());
            GetMenuData();
            cmdID = new DelegateCommand<object>(cmdID_execute);
            gridRoot = System.Windows.Visibility.Collapsed;
            gridUserControl = System.Windows.Visibility.Collapsed;
            cmdAddUpdate = new DelegateCommand<object>(cmdAddUpdate_execute);
            cmdContextMenu = new DelegateCommand<object>(cmdContextMenu_execute);
        }

        private ObservableCollection<Lookup> GetScreenList()
        {
            ObservableCollection<Lookup> lst = new ObservableCollection<Lookup>();
            try
            {
                foreach (DataServiceRef.Lookup item in pxy.GetSubLookupList(1))
                {
                    lst.Add(new Lookup
                    {
                        ID = item.ID,
                        Name = item.Name
                    });
                }
                return lst;
            }
            catch (FaultException ex)
            {
                IMSLibrary.UI.UIHelper.ShowErrorMessage(ex.Message);
                return lst;
            }
        }

        private int paramVal;
        private Models.MenuMaster objMenu;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        private void cmdContextMenu_execute(object obj)
        {
            objMenu = lstMenuMaster.FirstOrDefault(p => p.ID == ctxID);
            if (objMenu == null)
            {
                foreach (Models.MenuMaster item in lstMenuMaster)
                {
                    objMenu = item.lstChild.FirstOrDefault(p => p.ID == ctxID);
                    if (objMenu != null) break;
                }
            }

            gridRoot = System.Windows.Visibility.Collapsed;
            gridUserControl = System.Windows.Visibility.Collapsed;
            if (obj != null)
            {
                paramVal = Convert.ToInt32(obj);
                if ((paramVal == 5 || paramVal == 6) && objMenu.ScreenID > 0)
                {
                    IMSLibrary.UI.UIHelper.ShowErrorMessage("Please select Parent or Child Parent Node");
                    return;
                }
                if (paramVal == 1 || paramVal == 3 || paramVal == 5)
                {
                    gridRoot = System.Windows.Visibility.Visible;
                    return;
                }
                if (paramVal == 2 || paramVal == 6)
                {
                    gridUserControl = System.Windows.Visibility.Visible;
                    return;
                }
                if (paramVal == 4)
                {
                    try
                    {
                        System.Windows.MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to Delete this Hierarchy?", "", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                        if (mresult == System.Windows.MessageBoxResult.Yes && pxy.DeleteMenuMaster(ctxID))
                        {
                            GetMenuData();
                            IMSLibrary.UI.UIHelper.ShowMessage("Menu Modified");
                        }
                    }
                    catch (FaultException ex)
                    {
                        IMSLibrary.UI.UIHelper.ShowErrorMessage(ex.Message);
                    }
                }
            }
        }

        private void cmdAddUpdate_execute(object obj)
        {
            if (paramVal == 1)
            {
                objMenu.ID = 0;
                objMenu.Name = grName;
                objMenu.ParentID = 0;
                objMenu.ScreenID = 0;
            }
            if (paramVal == 2)
            {
                objMenu.ID = 0;
                objMenu.Name = gucName;
                objMenu.ParentID = 0;
                objMenu.ScreenID = gucScreenID;
            }
            if (paramVal == 3)
            {
                objMenu.Name = grName;
            }
            if (paramVal == 5)
            {
                objMenu.ID = 0;
                objMenu.Name = grName;
                objMenu.ParentID = ctxID;
                objMenu.ScreenID = 0;
            }
            if (paramVal == 6)
            {
                objMenu.ID = 0;
                objMenu.Name = gucName;
                objMenu.ParentID = ctxID;
                objMenu.ScreenID = gucScreenID;
            }
            try
            {
                MenuMaster pxyMenu = new MenuMaster
                {
                    ID = objMenu.ID,
                    IsActive = true,
                    Name = objMenu.Name,
                    ParentID = objMenu.ParentID,
                    ScreenID = objMenu.ScreenID
                };
                if (pxy.InsertUpdateMenuMaster(pxyMenu))
                {
                    IMSLibrary.UI.UIHelper.ShowMessage("Menu Modified");
                }
            }
            catch (FaultException ex)
            {
                IMSLibrary.UI.UIHelper.ShowErrorMessage(ex.Message);
            }
            grName = string.Empty;
            gucName = string.Empty;
            gucScreenID = 0;
            gridRoot = System.Windows.Visibility.Collapsed;
            gridUserControl = System.Windows.Visibility.Collapsed;
            GetMenuData();
        }

        private void cmdID_execute(object obj)
        {
            ctxID = Convert.ToInt32(obj);
        }

        private System.Windows.Visibility _gridRoot;

        public System.Windows.Visibility gridRoot
        {
            get { return _gridRoot; }
            set
            {
                _gridRoot = value;
                OnPropertyChanged(this, "gridRoot");
            }
        }

        private System.Windows.Visibility _gridUserControl;

        public System.Windows.Visibility gridUserControl
        {
            get { return _gridUserControl; }
            set
            {
                _gridUserControl = value;
                OnPropertyChanged(this, "gridUserControl");
            }
        }

        private string _grName;

        public string grName
        {
            get { return _grName; }
            set
            {
                _grName = value;
                OnPropertyChanged(this, "grName");
            }
        }

        private string _gucName;

        public string gucName
        {
            get { return _gucName; }
            set
            {
                _gucName = value;
                OnPropertyChanged(this, "gucName");
            }
        }

        private int _gucScreenID;

        public int gucScreenID
        {
            get { return _gucScreenID; }
            set
            {
                _gucScreenID = value;
                OnPropertyChanged(this, "gucScreenID");
            }
        }
    }
}