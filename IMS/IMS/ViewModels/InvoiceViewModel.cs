using IMS.Helper;
using IMS.DataServiceRef;
using System.Collections.ObjectModel;
using System.ServiceModel;
using IMSLibrary.UI;
using System;
using System.Linq;
using IMS.Models;

namespace IMS.ViewModels
{
    internal class InvoiceViewModel : ViewModelBase
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

        private InvoiceMaster _objInvoiceMaster;

        public InvoiceMaster objInvoiceMaster
        {
            get { return _objInvoiceMaster; }
            set
            {
                _objInvoiceMaster = value;
                InvoiceNo = value.InvoiceNo;
                InvoiceDate = value.InvoiceDate;
                PartyName = value.PartyName;
                OrderID = value.OrderID;
                OrderDate = value.OrderDate;
                Address = value.Address;
                EmailID = value.EmailID;
                ContactNo = value.ContactNo;
                Vendor = value.Vendor;
                lstInvoiceProduct = value.lstInvoiceProduct;
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

        private ObservableCollection<Lookup> _lstStatus;

        public ObservableCollection<Lookup> lstStatus
        {
            get { return _lstStatus; }
            set
            {
                _lstStatus = value;
                OnPropertyChanged(this, "lstStatus");
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

        public int InvoiceNo
        {
            get { return _objInvoiceMaster.InvoiceNo; }
            set
            {
                _objInvoiceMaster.InvoiceNo = value;
                OnPropertyChanged(this, "InvoiceNo");
            }
        }

        public DateTime InvoiceDate
        {
            get { return _objInvoiceMaster.InvoiceDate; }
            set
            {
                _objInvoiceMaster.InvoiceDate = value;
                OnPropertyChanged(this, "InvoiceDate");
            }
        }

        public string PartyName
        {
            get { return _objInvoiceMaster.PartyName; }
            set
            {
                _objInvoiceMaster.PartyName = value;
                OnPropertyChanged(this, "PartyName");
            }
        }

        public string OrderID
        {
            get { return _objInvoiceMaster.OrderID; }
            set
            {
                _objInvoiceMaster.OrderID = value;
                OnPropertyChanged(this, "OrderID");
            }
        }

        public DateTime OrderDate
        {
            get { return _objInvoiceMaster.OrderDate; }
            set
            {
                _objInvoiceMaster.OrderDate = value;
                OnPropertyChanged(this, "OrderDate");
            }
        }

        public string Address
        {
            get { return _objInvoiceMaster.Address; }
            set
            {
                _objInvoiceMaster.Address = value;
                OnPropertyChanged(this, "Address");
            }
        }

        public string EmailID
        {
            get { return _objInvoiceMaster.EmailID; }
            set
            {
                _objInvoiceMaster.EmailID = value;
                OnPropertyChanged(this, "EmailID");
            }
        }

        public string ContactNo
        {
            get { return _objInvoiceMaster.ContactNo; }
            set
            {
                _objInvoiceMaster.ContactNo = value;
                OnPropertyChanged(this, "ContactNo");
            }
        }

        public int Vendor
        {
            get { return _objInvoiceMaster.Vendor; }
            set
            {
                _objInvoiceMaster.Vendor = value;
                OnPropertyChanged(this, "Vendor");
            }
        }

        public ObservableCollection<InvoiceProduct> lstInvoiceProduct
        {
            get { return _objInvoiceMaster.lstInvoiceProduct; }
            set
            {
                _objInvoiceMaster.lstInvoiceProduct = value;
                OnPropertyChanged(this, "lstInvoiceProduct");
            }
        }

        private decimal _InvoiceAmount;

        public decimal InvoiceAmount
        {
            get { return _InvoiceAmount; }
            set
            {
                _InvoiceAmount = value;
                OnPropertyChanged(this, "InvoiceAmount");
            }
        }

        public DelegateCommand<object> cmdSearch { get; set; }
        public DelegateCommand<object> cmdSave { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }

        public DelegateCommand<object> cmdAdd { get; set; }
        public DelegateCommand<object> cmdDelete { get; set; }
        public DelegateCommand<object> cmdGST { get; set; }

        private bool _IsCSGST;

        public bool IsCSGST
        {
            get { return _IsCSGST; }
            set
            {
                _IsCSGST = value;
                OnPropertyChanged(this, "IsCSGST");
            }
        }

        private bool _IsIGST;

        public bool IsIGST
        {
            get { return _IsIGST; }
            set
            {
                _IsIGST = value;
                OnPropertyChanged(this, "IsIGST");
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

        public InvoiceViewModel(int PageMode)
        {
            try
            {
                Mode = PageMode;
                if (pxy == null) pxy = new DataServiceClient();
                objInvoiceMaster = new InvoiceMaster();
                lstStatus = new ObservableCollection<Lookup>();
                lstInvoiceProduct = new ObservableCollection<InvoiceProduct>();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstProduct = new ObservableCollection<ProductList>();
                lstSearch = new ObservableCollection<Search>
                {
                    new Search { ID = 1, Desc = "InvoiceNo" },
                    new Search { ID = 2, Desc = "OrderID" }
                };
                searchMode = 2;
                lstVendor = pxy.GetVendorList();
                lstProduct = pxy.GetProductList();
                lstStatus = pxy.GetSubLookupList(Const.InvoiceStatus);
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute, cmdSearch_CanExecute);
                cmdSave = new DelegateCommand<object>(cmdSave_Execute, cmdSave_CanExecute);
                cmdClear = new DelegateCommand<object>(cmdClear_Execute);
                cmdAdd = new DelegateCommand<object>(cmdAdd_Execute, canExecute);
                cmdDelete = new DelegateCommand<object>(cmdDelete_Execute, canExecute);
                cmdGST = new DelegateCommand<object>(cmdGST_Execute);
                ModeAction();
                IsCSGST = false;
                IsIGST = false;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool canExecute(object obj)
        {
            if (Mode == PageMode.Browse || Mode == PageMode.Delete) return false;
            return true;
        }

        private void cmdGST_Execute(object obj)
        {
            if (IsCSGST)
            {
                IsIGST = false;
                for (int i = 0; i < lstInvoiceProduct.Count; i++)
                {
                    lstInvoiceProduct[i].CGSTPerc = pxy.FindGstRate(lstInvoiceProduct[i].ProductID, "CGST");
                    lstInvoiceProduct[i].SGSTPerc = pxy.FindGstRate(lstInvoiceProduct[i].ProductID, "SGST");
                    lstInvoiceProduct[i].SGSTPerc = 0;
                }
            }

            if (IsIGST)
            {
                IsCSGST = false;
                for (int i = 0; i < lstInvoiceProduct.Count; i++)
                {
                    lstInvoiceProduct[i].SGSTPerc = pxy.FindGstRate(lstInvoiceProduct[i].ProductID, "IGST");
                    lstInvoiceProduct[i].CGSTPerc = 0;
                    lstInvoiceProduct[i].SGSTPerc = 0;
                }
            }

            UpdateValues();
        }

        private void cmdAdd_Execute(object obj)
        {
            try { lstInvoiceProduct.Add(new InvoiceProduct { }); }
            catch (Exception ex) { IMSLibrary.UI.UIHelper.ShowErrorMessage(ex.Message); }
        }

        private void cmdDelete_Execute(object obj)
        {
            try
            {
                if (dgSelectedIndex < 0)
                {
                    UIHelper.ShowErrorMessage("Select Item for Delete");
                    return;
                }
                lstInvoiceProduct.RemoveAt(dgSelectedIndex);
            }
            catch (Exception ex) { UIHelper.ShowErrorMessage(ex.Message); }
        }

        private bool cmdSave_CanExecute(object obj)
        {
            return true;
        }

        private void cmdSave_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || Mode == PageMode.Modify)
                {
                    UpdateValues();

                    if (pxy.InsertUpdateInvoice(objInvoiceMaster, Global.UserID, Mode))
                    {
                        UIHelper.ShowMessage("Data Saved Successfully");
                        objInvoiceMaster = new InvoiceMaster();
                    }
                }
                else if (Mode == PageMode.Delete)
                {
                    if (pxy.DeleteInvoice(InvoiceNo))
                    {
                        UIHelper.ShowMessage("Data Deleted Successfully");
                        objInvoiceMaster = new InvoiceMaster();
                    }
                }
                else if (Mode == PageMode.Browse)
                {
                    var InvoiceProduct = objInvoiceMaster.lstInvoiceProduct.FirstOrDefault(p => p.Status != 2);
                    if (InvoiceProduct == null)
                    {
                        UIHelper.ShowErrorMessage("Returned Invoice!");
                        return;
                    }
                    Views.GenReports win = new Views.GenReports(InvoiceNo, InvoiceProduct.InvoiceDate);
                    win.ShowDialog();
                }
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void cmdClear_Execute(object obj)
        {
            objInvoiceMaster = new InvoiceMaster();
            searchVal = string.Empty;
        }

        private bool cmdSearch_CanExecute(object obj)
        {
            if (!string.IsNullOrEmpty(searchVal) && Mode != PageMode.Add) return true;
            return false;
        }

        private void cmdSearch_Execute(object obj)
        {
            try
            {
                if (searchMode == 1)
                {
                    objInvoiceMaster = pxy.GetInvoiceDetails(Convert.ToInt32(searchVal), string.Empty, searchMode);
                }
                else if (searchMode == 2)
                {
                    objInvoiceMaster = pxy.GetInvoiceDetails(0, searchVal.Trim(), searchMode);
                }
                InvoiceAmount = lstInvoiceProduct.Sum(p => p.Total);
                if (lstInvoiceProduct.Sum(p => p.CGST + p.SGST) > 0) IsCSGST = true;
                if (lstInvoiceProduct.Sum(p => p.IGST) > 0) IsIGST = true;
                if (Global.GlobalPageMode == PageMode.Modify)
                {
                    Pk = false;
                    Ck = true;
                }
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

        private void ModeAction()
        {
            try
            {
                switch (Mode)
                {
                    case PageMode.Add:
                        {
                            Pk = true;
                            Ck = true;
                            modeContent = "Save";
                        }
                        break;

                    case PageMode.Modify:
                        {
                            Pk = false;
                            Ck = false;
                            modeContent = "Update";
                        }
                        break;

                    case PageMode.Delete:
                        {
                            Pk = false;
                            Ck = false;
                            modeContent = "Delete";
                        }
                        break;

                    case PageMode.Browse:
                        {
                            Pk = false;
                            Ck = false;
                            modeContent = "Print";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private void UpdateValues()
        {
            for (int i = 0; i < lstInvoiceProduct.Count; i++)
            {
                if (IsCSGST)
                {
                    lstInvoiceProduct[i].UnitPrice = Math.Round(((lstInvoiceProduct[i].Rate / (lstInvoiceProduct[i].CGSTPerc + 100)) * 100), 2);
                    lstInvoiceProduct[i].CGST = (lstInvoiceProduct[i].Rate - lstInvoiceProduct[i].UnitPrice) * lstInvoiceProduct[i].Quantity;

                    lstInvoiceProduct[i].UnitPrice = Math.Round(((lstInvoiceProduct[i].Rate / (lstInvoiceProduct[i].SGSTPerc + 100)) * 100), 2);
                    lstInvoiceProduct[i].SGST = (lstInvoiceProduct[i].Rate - lstInvoiceProduct[i].UnitPrice) * lstInvoiceProduct[i].Quantity;

                    lstInvoiceProduct[i].UnitPrice = (lstInvoiceProduct[i].Rate * lstInvoiceProduct[i].Quantity) - lstInvoiceProduct[i].CGST - lstInvoiceProduct[i].SGST;

                    if (lstInvoiceProduct[i].Shipping_Total > 0.00M)
                    {
                        lstInvoiceProduct[i].Shipping_UnitPrice = Math.Round(((lstInvoiceProduct[i].Shipping_Total / (lstInvoiceProduct[i].CGSTPerc + 100)) * 100), 2);
                        lstInvoiceProduct[i].Shipping_CGST = lstInvoiceProduct[i].Shipping_Total - lstInvoiceProduct[i].Shipping_UnitPrice;

                        lstInvoiceProduct[i].Shipping_UnitPrice = Math.Round(((lstInvoiceProduct[i].Shipping_Total / (lstInvoiceProduct[i].SGSTPerc + 100)) * 100), 2);
                        lstInvoiceProduct[i].Shipping_SGST = lstInvoiceProduct[i].Shipping_Total - lstInvoiceProduct[i].Shipping_UnitPrice;

                        lstInvoiceProduct[i].Shipping_UnitPrice = (lstInvoiceProduct[i].Shipping_Total - lstInvoiceProduct[i].Shipping_CGST - lstInvoiceProduct[i].Shipping_SGST) / lstInvoiceProduct[i].Quantity;
                        lstInvoiceProduct[i].Shipping_NetSale = lstInvoiceProduct[i].Shipping_UnitPrice * lstInvoiceProduct[i].Quantity;
                    }

                    lstInvoiceProduct[i].IGST = 0;
                    lstInvoiceProduct[i].IGSTPerc = 0;
                    lstInvoiceProduct[i].Shipping_IGST = 0;
                }

                if (IsIGST)
                {
                    lstInvoiceProduct[i].UnitPrice = Math.Round(((lstInvoiceProduct[i].Rate / (lstInvoiceProduct[i].CGSTPerc + 100)) * 100), 2);
                    lstInvoiceProduct[i].CGST = (lstInvoiceProduct[i].Rate - lstInvoiceProduct[i].UnitPrice) * lstInvoiceProduct[i].Quantity;

                    if (lstInvoiceProduct[i].Shipping_Total > 0.00M)
                    {
                        lstInvoiceProduct[i].Shipping_UnitPrice = Math.Round(((lstInvoiceProduct[i].Shipping_Total / (lstInvoiceProduct[i].IGSTPerc + 100)) * 100), 2);
                        lstInvoiceProduct[i].Shipping_IGST = lstInvoiceProduct[i].Shipping_Total - lstInvoiceProduct[i].Shipping_UnitPrice;

                        lstInvoiceProduct[i].Shipping_UnitPrice = (lstInvoiceProduct[i].Shipping_Total - lstInvoiceProduct[i].Shipping_IGST) / lstInvoiceProduct[i].Quantity;
                        lstInvoiceProduct[i].Shipping_NetSale = lstInvoiceProduct[i].Shipping_UnitPrice * lstInvoiceProduct[i].Quantity;
                    }

                    lstInvoiceProduct[i].CGST = 0;
                    lstInvoiceProduct[i].CGSTPerc = 0;
                    lstInvoiceProduct[i].Shipping_CGST = 0;
                    lstInvoiceProduct[i].SGST = 0;
                    lstInvoiceProduct[i].SGSTPerc = 0;
                    lstInvoiceProduct[i].Shipping_SGST = 0;
                }

                lstInvoiceProduct[i].Total = (lstInvoiceProduct[i].Rate * lstInvoiceProduct[i].Quantity) + lstInvoiceProduct[i].Shipping_Total;
            }

            InvoiceAmount = lstInvoiceProduct.Sum(p => p.Total);
        }
    }
}