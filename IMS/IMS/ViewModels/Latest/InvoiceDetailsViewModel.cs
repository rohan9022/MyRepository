using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.InvoiceServiceRef;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Linq;

namespace IMS.ViewModels
{
    internal class InvoiceDetailsViewModel : ViewModelBase
    {
        private readonly DataServiceClient pxyDataService;
        private readonly InvoiceServiceClient pxyInvoice;

        private ObservableCollection<InvoiceDetailsService> _lstInvoiceDetails;

        public ObservableCollection<InvoiceDetailsService> lstInvoiceDetails
        {
            get { return _lstInvoiceDetails; }
            set
            {
                _lstInvoiceDetails = value;
                OnPropertyChanged(this, "lstInvoiceDetails");
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

        public DelegateCommand<object> cmdSearch { get; set; }
        public DelegateCommand<object> cmdPrint { get; set; }
        public DelegateCommand<object> cmdBlankDate { get; set; }
        private string _orderID;

        public string orderID
        {
            get { return _orderID; }
            set
            {
                _orderID = value;
                OnPropertyChanged(this, "orderID");
            }
        }

        private DateTime _fromInvoiceDate;

        public DateTime fromInvoiceDate
        {
            get { return _fromInvoiceDate; }
            set
            {
                _fromInvoiceDate = value;
                OnPropertyChanged(this, "fromInvoiceDate");
            }
        }

        private DateTime _toInvoiceDate;

        public DateTime toInvoiceDate
        {
            get { return _toInvoiceDate; }
            set
            {
                _toInvoiceDate = value;
                OnPropertyChanged(this, "toInvoiceDate");
            }
        }

        private int _fromInvoiceNo;

        public int fromInvoiceNo
        {
            get { return _fromInvoiceNo; }
            set
            {
                _fromInvoiceNo = value;
                OnPropertyChanged(this, "fromInvoiceNo");
            }
        }

        private int _toInvoiceNo;

        public int toInvoiceNo
        {
            get { return _toInvoiceNo; }
            set
            {
                _toInvoiceNo = value;
                OnPropertyChanged(this, "toInvoiceNo");
            }
        }

        public InvoiceDetailsViewModel()
        {
            try
            {
                if (pxyDataService == null) pxyDataService = new DataServiceClient();
                if (pxyInvoice == null) pxyInvoice = new InvoiceServiceClient();

                lstInvoiceDetails = new ObservableCollection<InvoiceDetailsService>();
                lstStatus = new ObservableCollection<Lookup>();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstProduct = new ObservableCollection<ProductList>();
                lstVendor = pxyDataService.GetVendorList();
                lstProduct = pxyDataService.GetProductList();
                lstStatus = pxyDataService.GetSubLookupList(Const.InvoiceStatus);
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute);
                cmdPrint = new DelegateCommand<object>(cmdPrint_Execute);
                cmdBlankDate = new DelegateCommand<object>(cmdBlankDate_Execute);
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        #region ==Clear Values==

        private void cmdBlankDate_Execute(object obj)
        {
            fromInvoiceDate = toInvoiceDate = new DateTime(1900, 01, 01);
        }

        #endregion ==Clear Values==

        private void cmdSearch_Execute(object obj)
        {
            try
            {
                if (fromInvoiceNo > toInvoiceNo)
                {
                    UIHelper.ShowErrorMessage("From Invoice No. cannot not be greater than To Invoice No.");
                    return;
                }
                if (fromInvoiceDate > toInvoiceDate)
                {
                    UIHelper.ShowErrorMessage("From Date cannot not be greater than To Date");
                    return;
                }
                lstInvoiceDetails = pxyInvoice.GetInvoiceDetails(fromInvoiceNo, toInvoiceNo, fromInvoiceDate, toInvoiceDate, orderID);
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

        private void cmdPrint_Execute(object obj)
        {
            var InvoiceProduct = lstInvoiceDetails.FirstOrDefault(p => p.FinalStatus != 2);
            if (InvoiceProduct == null)
            {
                UIHelper.ShowErrorMessage("Returned Invoice!");
                return;
            }
            Views.GenReports win = new Views.GenReports(InvoiceProduct.InvoiceNo, InvoiceProduct.InvoiceDate);
            win.ShowDialog();
        }
    }
}