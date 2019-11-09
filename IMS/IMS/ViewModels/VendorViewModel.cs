using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace IMS.ViewModels
{
    internal class VendorViewModel : ViewModelBase
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

        private VendorMaster _objVendorMaster;

        public VendorMaster objVendorMaster
        {
            get { return _objVendorMaster; }
            set
            {
                _objVendorMaster = value;
                VendorID = value.ID;
                Name = value.Name;
                Address = value.Address;
                GSTNo = value.GSTNo;
                PanNo = value.PanNo;
                CommissionTab = value.CommissionTab;
            }
        }

        private ObservableCollection<VendorMaster> _lstVendor;

        public ObservableCollection<VendorMaster> lstVendor
        {
            get { return _lstVendor; }
            set
            {
                _lstVendor = value;
                OnPropertyChanged(this, "lstVendor");
            }
        }

        public int VendorID
        {
            get { return _objVendorMaster.ID; }
            set
            {
                _objVendorMaster.ID = value;
                OnPropertyChanged(this, "VendorID");
            }
        }

        public string Name
        {
            get { return _objVendorMaster.Name; }
            set
            {
                _objVendorMaster.Name = value;
                OnPropertyChanged(this, "Name");
            }
        }

        public string Address
        {
            get { return _objVendorMaster.Address; }
            set
            {
                _objVendorMaster.Address = value;
                OnPropertyChanged(this, "Address");
            }
        }

        public string GSTNo
        {
            get { return _objVendorMaster.GSTNo; }
            set
            {
                _objVendorMaster.GSTNo = value;
                OnPropertyChanged(this, "GSTNo");
            }
        }

        public string PanNo
        {
            get { return _objVendorMaster.PanNo; }
            set
            {
                _objVendorMaster.PanNo = value;
                OnPropertyChanged(this, "PanNo");
            }
        }

        public bool CommissionTab
        {
            get { return _objVendorMaster.CommissionTab; }
            set
            {
                _objVendorMaster.CommissionTab = value;
                OnPropertyChanged(this, "CommissionTab");
            }
        }

        public DelegateCommand<object> cmdVendorDetails { get; set; }
        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }

        public VendorViewModel(int PageMode)
        {
            try
            {
                Mode = PageMode;
                if (pxy == null) pxy = new DataServiceClient();
                objVendorMaster = new VendorMaster();
                cmdVendorDetails = new DelegateCommand<object>(cmdVendorDetails_Execute, cmdVendorDetails_CanExecute);
                cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
                cmdClear = new DelegateCommand<object>(cmdClear_Execute);
                ModeAction();
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdVendorDetails_Execute(object obj)
        {
            try
            {
                objVendorMaster = pxy.GetVendorDetails(VendorID);
                if (Mode == PageMode.Modify) Ck = true;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdVendorDetails_CanExecute(object obj)
        {
            if (Mode == PageMode.Add) return false;
            return true;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify)
                {
                    if (pxy.InsertUpdateVendor(objVendorMaster, Global.UserID))
                    {
                        UIHelper.ShowMessage("Data Saved Successfully");
                    }
                }
                else if (Mode == PageMode.Delete && pxy.DeleteVendor(VendorID))
                {
                    UIHelper.ShowMessage("Data Deleted Successfully");
                }
                ModeAction();
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdSave_CanExecute(object obj)
        {
            if (VendorID > 0 || Mode == PageMode.Add) return true;
            return false;
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
                objVendorMaster = new VendorMaster();
                if (Mode != PageMode.Add) lstVendor = pxy.GetVendorList();
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }
    }
}