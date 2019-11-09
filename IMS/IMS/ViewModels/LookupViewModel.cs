using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class LookupViewModel : ViewModelBase, IDataErrorInfo
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

        private string _lookupName;

        public string lookupName
        {
            get { return _lookupName; }
            set
            {
                _lookupName = value;
                OnPropertyChanged(this, "lookupName");
            }
        }

        private int _lookupID;

        public int lookupID
        {
            get { return _lookupID; }
            set
            {
                _lookupID = value;
                OnPropertyChanged(this, "lookupID");
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

        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdLookupDetails { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }

        private readonly DataServiceClient pxy;

        public LookupViewModel(int PageMode)
        {
            Mode = PageMode;
            pxy = new DataServiceClient();
            cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExeute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
            cmdLookupDetails = new DelegateCommand<object>(cmdLookupDetails_Execute);
            lstLookup = new ObservableCollection<Lookup>();
            lstLookup = pxy.GetLookupList();
            ModeAction();
        }

        private void cmdLookupDetails_Execute(object obj)
        {
            try
            {
                lookupName = lstLookup[System.Convert.ToInt32(obj)].Name;
                if (Mode == PageMode.Modify)
                {
                    Pk = false;
                    Ck = true;
                }
            }
            catch
            {
                // DDA
            }
        }

        private bool cmdSave_CanExeute(object obj)
        {
            if (Mode == PageMode.Browse) return true;
            if (Mode == PageMode.Add) return true;
            if (Mode == PageMode.Modify && lookupID > 0) return true;
            if (Mode == PageMode.Delete && lookupID > 0) return true;
            return false;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify || Mode == PageMode.Delete)
                {
                    if ((Mode == PageMode.Add || Mode == PageMode.Modify) && !pxy.CheckLookupName(lookupName))
                    {
                        UIHelper.ShowErrorMessage("Lookup Name Not Available");
                        return;
                    }
                    if (pxy.InsertUpdateDeleteLookup(lookupName, lookupID, Global.UserID, Mode))
                    {
                        if (Mode == PageMode.Delete) UIHelper.ShowMessage("Data Deleted Successfully");
                        else UIHelper.ShowMessage("Data Saved Successfully");
                        lookupName = string.Empty;
                        lookupID = 0;
                        lstLookup = pxy.GetLookupList();
                    }
                }
                else if (Mode == PageMode.Browse)
                {
                    lookupName = string.Empty;
                    lookupID = 0;
                }
                ModeAction();
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdClear_Execute(object obj)
        {
            ModeAction();
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
                    case "lookupID": if (lookupID <= 0 && Mode != PageMode.Add) Result = "Select Lookup Code"; break;
                    case "lookupName": if (string.IsNullOrEmpty(lookupName)) Result = "Enter Lookup Name"; break;
                }
                return Result;
            }
        }
    }
}