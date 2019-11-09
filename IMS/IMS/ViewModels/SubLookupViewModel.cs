using IMS.Helper;
using IMSLibrary.UI;
using System.Collections.ObjectModel;
using IMS.DataServiceRef;
using System;
using System.ServiceModel;
using IMS.Models;

namespace IMS.ViewModels
{
    internal class SubLookupViewModel : ViewModelBase
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

        private int _LookupID;

        public int LookupID
        {
            get { return _LookupID; }
            set
            {
                _LookupID = value;
                OnPropertyChanged(this, "LookupID");
            }
        }

        private int _SubLookupID;

        public int SubLookupID
        {
            get { return _SubLookupID; }
            set
            {
                _SubLookupID = value;
                OnPropertyChanged(this, "SubLookupID");
            }
        }

        private string _SubLookupName;

        public string SubLookupName
        {
            get { return _SubLookupName; }
            set
            {
                _SubLookupName = value;
                OnPropertyChanged(this, "SubLookupName");
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

        private ObservableCollection<Lookup> _lstLookup;

        public ObservableCollection<Lookup> lstLookup
        {
            get { return _lstLookup; }
            set
            {
                _lstLookup = value;
                OnPropertyChanged(this, "lstLookup");
            }
        }

        private ObservableCollection<Lookup> _lstSubLookup;

        public ObservableCollection<Lookup> lstSubLookup
        {
            get { return _lstSubLookup; }
            set
            {
                _lstSubLookup = value;
                OnPropertyChanged(this, "lstSubLookup");
            }
        }

        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        public DelegateCommand<object> cmdLookup { get; set; }
        public DelegateCommand<object> cmdSubLookup { get; set; }

        public SubLookupViewModel(int PageMode)
        {
            Mode = PageMode;
            if (pxy == null) pxy = new DataServiceClient();
            cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            cmdLookup = new DelegateCommand<object>(cmdLookup_Execute, cmdLookup_CanExecute);
            cmdSubLookup = new DelegateCommand<object>(cmdSubLookup_Execute, cmdSubLookup_CanExecute);
            lstLookup = new ObservableCollection<Lookup>();
            lstSubLookup = new ObservableCollection<Lookup>();
            lstLookup = pxy.GetLookupList();
            ModeAction();
        }

        private void cmdLookup_Execute(object obj)
        {
            try
            {
                lstSubLookup = pxy.GetSubLookupList(LookupID);
                if (lstSubLookup == null || lstSubLookup.Count == 0)
                {
                    SubLookupName = string.Empty;
                    UIHelper.ShowMessage("Data not found");
                }
                dgSelectedIndex = 0;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdLookup_CanExecute(object obj)
        {
            return true;
        }

        private void cmdSubLookup_Execute(object obj)
        {
            try
            {
                if (dgSelectedIndex < 0)
                {
                    SubLookupName = string.Empty;
                    return;
                }
                SubLookupName = lstSubLookup[dgSelectedIndex].Name;
                if (Mode == PageMode.Modify) Ck = true;
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdSubLookup_CanExecute(object obj)
        {
            return true;
        }

        private void cmdClear_Execute(object obj)
        {
            lstSubLookup = new ObservableCollection<Lookup>();
            LookupID = 0;
            SubLookupID = 0;
            SubLookupName = string.Empty;
        }

        private bool cmdSave_CanExecute(object obj)
        {
            if (Mode == PageMode.Add && !string.IsNullOrEmpty(SubLookupName)) return true;
            if (Mode == PageMode.Modify && dgSelectedIndex >= 0 && !string.IsNullOrEmpty(SubLookupName)) return true;
            if (Mode == PageMode.Delete && dgSelectedIndex >= 0 && lstSubLookup.Count > 0) return true;
            if (Mode == PageMode.Browse && dgSelectedIndex >= 0 && lstSubLookup.Count > 0) return true;
            return false;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify || Mode == PageMode.Delete)
                {
                    if ((Mode == PageMode.Add || Mode == PageMode.Modify) && !pxy.CheckSubLookupName(SubLookupName, LookupID))
                    {
                        UIHelper.ShowErrorMessage("Lookup Name Not Available");
                        return;
                    }
                    if (pxy.InsertUpdateDeleteSubLookup(SubLookupName, LookupID, SubLookupID, Global.UserID, Mode))
                    {
                        if (Mode == PageMode.Delete) UIHelper.ShowMessage("Data Deleted Successfully");
                        else UIHelper.ShowMessage("Data Saved Successfully");
                        lstSubLookup = pxy.GetSubLookupList(LookupID);
                    }
                }
                else if (Mode == PageMode.Browse)
                {
                    SubLookupName = string.Empty;
                    SubLookupID = 0;
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