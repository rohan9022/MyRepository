using IMS.Helper;
using IMS.Models;
using IMS.DataServiceRef;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Controls;

namespace IMS.ViewModels
{
    internal class HomeWindowViewModel : ViewModelBase
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

        public HomeWindowViewModel()
        {
            if (pxy == null) pxy = new DataServiceClient();
            lstMenuMaster = new ObservableCollection<Models.MenuMaster>();
            lstScreen = new ObservableCollection<Lookup>(GetScreenList());
            GetMenuData();

            cmdClose = new DelegateCommand<object>(cmdClose_execute);
            cmdAddTab = new DelegateCommand<object>(cmdAddTab_execute);
            lstTab = new ObservableCollection<TabList>();

            cmdRefresh = new DelegateCommand<object>(cmdRefresh_execute);
            cmdNavigation = new DelegateCommand<object>(cmdNavigation_execute);
            cmdExit = new DelegateCommand<object>(cmdExit_execute);

            cmdPageMode = new DelegateCommand<object>(cmdPageMode_execute);
        }

        private ObservableCollection<Lookup> GetScreenList()
        {
            ObservableCollection<Lookup> lst = new ObservableCollection<Lookup>();
            try
            {
                foreach (Lookup item in pxy.GetSubLookupList(Const.Screen))
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

        #region ==TreeView==

        private ObservableCollection<IMS.Models.MenuMaster> _lstMenuMaster;

        public ObservableCollection<IMS.Models.MenuMaster> lstMenuMaster
        {
            get { return _lstMenuMaster; }
            set
            {
                _lstMenuMaster = value;
                OnPropertyChanged(this, "lstMenuMaster");
            }
        }

        private void GetMenuData()
        {
            ObservableCollection<Models.MenuMaster> lstTemp = new ObservableCollection<Models.MenuMaster>();
            foreach (DataServiceRef.MenuMaster item in pxy.GetMenuList(3))
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

        #region ==Tab==

        private ObservableCollection<TabList> _lstTab;

        public ObservableCollection<TabList> lstTab
        {
            get { return _lstTab; }
            set
            {
                _lstTab = value;
                OnPropertyChanged(this, "lstTab");
            }
        }

        private int _tabSelectedIndex;

        public int tabSelectedIndex
        {
            get { return _tabSelectedIndex; }
            set
            {
                _tabSelectedIndex = value;
                OnPropertyChanged(this, "tabSelectedIndex");
            }
        }

        private List<IMSLibrary.UI.CloseableTabItem> _lstTabItem;

        public List<IMSLibrary.UI.CloseableTabItem> lstTabItem
        {
            get { return _lstTabItem; }
            set
            {
                _lstTabItem = value;
                OnPropertyChanged(this, "lstTabItem");
            }
        }

        public DelegateCommand<object> cmdClose { get; set; }
        public DelegateCommand<object> cmdAddTab { get; set; }

        private void cmdAddTab_execute(object obj)
        {
            try
            {
                if (Convert.ToInt32(obj) > 0)
                {
                    string scrName = lstScreen.SingleOrDefault(p => p.ID == Convert.ToInt32(obj)).Name;
                    if (lstTab.Any(p => p.headerName == scrName))
                    {
                        tabSelectedIndex = lstTab.IndexOf(lstTab.First(p => p.headerName == scrName));
                        return;
                    }
                    try
                    {
                        var userControl = (UserControl)Activator.CreateInstance(Type.GetType(Const.Namespace + "." + scrName), 1);
                        lstTab.Add(new TabList { headerName = scrName, contCtrl = userControl, PageMode = 1 });
                    }
                    catch
                    {
                        var userControl = (UserControl)Activator.CreateInstance(Type.GetType(Const.Namespace + "." + scrName));
                        lstTab.Add(new TabList { headerName = scrName, contCtrl = userControl, PageMode = 1 });
                    }
                    tabSelectedIndex = lstTab.Count - 1;
                    CheckPageMode();
                }
            }
            catch
            {
                // don't do anything here
            }
        }

        private void cmdClose_execute(object obj)
        {
            if (lstTab.Count == 0) return;
            if (obj == null && tabSelectedIndex >= 0)
            {
                lstTab.RemoveAt(tabSelectedIndex);
                if (lstTab.Count == 0) modeBrowse = modeAdd = modeModify = modeDelete = false;
                return;
            }
            lstTab.Remove((TabList)obj);
            if (lstTab.Count == 0) modeBrowse = modeAdd = modeModify = modeDelete = false;
        }

        #endregion ==Tab==

        #region ==Ribbon==

        public DelegateCommand<object> cmdRefresh { get; set; }
        public DelegateCommand<object> cmdNavigation { get; set; }
        public DelegateCommand<object> cmdExit { get; set; }

        private void cmdRefresh_execute(object obj)
        {
            lstScreen = new ObservableCollection<Lookup>(GetScreenList());
            GetMenuData();
        }

        private void cmdNavigation_execute(object obj)
        {
            if (Convert.ToInt32(obj) == 1)
            {
                if (lstTab.Count() > 1 && tabSelectedIndex > 0)
                {
                    tabSelectedIndex -= 1;
                    CheckPageMode();
                }
            }
            else if (Convert.ToInt32(obj) == 2)
            {
                if (lstTab.Any() && tabSelectedIndex != lstTab.Count() - 1)
                {
                    tabSelectedIndex += 1;
                    CheckPageMode();
                }
            }
            else if (Convert.ToInt32(obj) == 3)
            {
                CheckPageMode();
            }
        }

        private void cmdExit_execute(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion ==Ribbon==

        #region ==PageMode==

        public DelegateCommand<object> cmdPageMode { get; set; }

        private bool _modeBrowse;

        public bool modeBrowse
        {
            get { return _modeBrowse; }
            set
            {
                _modeBrowse = value;
                OnPropertyChanged(this, "modeBrowse");
            }
        }

        private bool _modeAdd;

        public bool modeAdd
        {
            get { return _modeAdd; }
            set
            {
                _modeAdd = value;
                OnPropertyChanged(this, "modeAdd");
            }
        }

        private bool _modeModify;

        public bool modeModify
        {
            get { return _modeModify; }
            set
            {
                _modeModify = value;
                OnPropertyChanged(this, "modeModify");
            }
        }

        private bool _modeDelete;

        public bool modeDelete
        {
            get { return _modeDelete; }
            set
            {
                _modeDelete = value;
                OnPropertyChanged(this, "modeDelete");
            }
        }

        private void cmdPageMode_execute(object obj)
        {
            if (lstTab.Count > 0 && lstTab[tabSelectedIndex].PageMode != Convert.ToInt32(obj))
            {
                lstTab[tabSelectedIndex].PageMode = Convert.ToInt32(obj);
                CheckPageMode();
                try
                {
                    var userControl = (UserControl)Activator.CreateInstance(Type.GetType(Const.Namespace + "." + lstTab[tabSelectedIndex].headerName), PageMode);
                    lstTab[tabSelectedIndex].contCtrl = userControl;
                }
                catch
                {
                    // don't do anything here
                }
            }
        }

        private int PageMode;

        private void CheckPageMode()
        {
            if (tabSelectedIndex >= 0)
            {
                PageMode = lstTab[tabSelectedIndex].PageMode;
                modeBrowse = modeAdd = modeModify = modeDelete = false;
                if (PageMode == 1)
                {
                    modeBrowse = true;
                }
                else if (PageMode == 2)
                {
                    modeAdd = true;
                }
                else if (PageMode == 3)
                {
                    modeModify = true;
                }
                else if (PageMode == 4)
                {
                    modeDelete = true;
                }
            }
        }

        #endregion ==PageMode==
    }
}