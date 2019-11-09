using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.InvoiceServiceRef;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Linq;
using IMS.Models;

namespace IMS.ViewModels
{
    internal class InvoiceDetailsMasterSeparateViewModel : ViewModelBase
    {
        #region ==Properties && Members==

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

        private Customer _objCustomerDetails;

        public Customer objCustomerDetails
        {
            get { return _objCustomerDetails; }
            set
            {
                _objCustomerDetails = value;
                OnPropertyChanged(this, "objCustomerDetails");
            }
        }

        private ObservableCollection<ProductDetails> _lstProductDetails;

        public ObservableCollection<ProductDetails> lstProductDetails
        {
            get { return _lstProductDetails; }
            set
            {
                _lstProductDetails = value;
                OnPropertyChanged(this, "lstProductDetails");
            }
        }

        private ObservableCollection<SettlementDetails> _lstSettlementDetails;

        public ObservableCollection<SettlementDetails> lstSettlementDetails
        {
            get { return _lstSettlementDetails; }
            set
            {
                _lstSettlementDetails = value;
                OnPropertyChanged(this, "lstSettlementDetails");
            }
        }

        private InvoiceDetailsService _selectedInvoiceDetails;

        public InvoiceDetailsService selectedInvoiceDetails
        {
            get { return _selectedInvoiceDetails; }
            set
            {
                _selectedInvoiceDetails = value;
                OnPropertyChanged(this, "selectedInvoiceDetails");

                if (selectedInvoiceDetails != null)
                {
                    lstProductDetails = selectedInvoiceDetails.IProduct;
                    selectedProductDetails = new ProductDetails();
                    if (lstProductDetails.Count > 0)
                    {
                        selectedProductDetails = selectedInvoiceDetails.IProduct[0];
                    }
                    objCustomerDetails = selectedInvoiceDetails.ICustomer.FirstOrDefault();
                }
            }
        }

        private ProductDetails _selectedProductDetails;

        public ProductDetails selectedProductDetails
        {
            get { return _selectedProductDetails; }
            set
            {
                _selectedProductDetails = value;
                OnPropertyChanged(this, "selectedProductDetails");

                if (selectedProductDetails != null)
                {
                    lstSettlementDetails = selectedProductDetails.ISettlement;
                }
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
        public DelegateCommand<object> cmdUpdate { get; set; }
        public DelegateCommand<object> cmdDelete { get; set; }
        public DelegateCommand<object> cmdBlankDate { get; set; }

        private string _orderID = string.Empty;

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

        #endregion ==Properties && Members==

        public InvoiceDetailsMasterSeparateViewModel()
        {
            try
            {
                if (pxyDataService == null) pxyDataService = new DataServiceClient();
                if (pxyInvoice == null) pxyInvoice = new InvoiceServiceClient();
                fromInvoiceDate = toInvoiceDate = new DateTime(1900, 01, 01);
                lstInvoiceDetails = new ObservableCollection<InvoiceDetailsService>();
                lstStatus = new ObservableCollection<Lookup>();
                lstVendor = new ObservableCollection<VendorMaster>();
                lstProduct = new ObservableCollection<ProductList>();
                lstVendor = pxyDataService.GetVendorList();
                lstProduct = pxyDataService.GetProductList();
                lstStatus = pxyDataService.GetSubLookupList(Const.InvoiceStatus);
                cmdSearch = new DelegateCommand<object>(cmdSearch_Execute);
                cmdPrint = new DelegateCommand<object>(cmdPrint_Execute);
                cmdUpdate = new DelegateCommand<object>(cmdUpdate_Execute);
                cmdDelete = new DelegateCommand<object>(cmdDelete_Execute);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        private void cmdDelete_Execute(object obj)
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to Delete it", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.No);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    if (obj is InvoiceDetailsService)
                    {
                        InvoiceDetailsService invoiceDetails = obj as InvoiceDetailsService;
                        if (pxyInvoice.DeleteInvoiceDetails(invoiceDetails, Global.UserID))
                        {
                            UIHelper.ShowMessage("Data successfully deleted!");
                        }
                    }
                    else if (obj is ProductDetails)
                    {
                        ProductDetails invoiceProduct = obj as ProductDetails;
                        if (pxyInvoice.DeleteInvoiceProduct(invoiceProduct, Global.UserID))
                        {
                            UIHelper.ShowMessage("Data successfully deleted!");
                        }
                    }
                    else if (obj is SettlementDetails)
                    {
                        SettlementDetails invoiceSettlement = obj as SettlementDetails;
                        if (pxyInvoice.DeleteInvoiceSettlement(invoiceSettlement, Global.UserID))
                        {
                            UIHelper.ShowMessage("Data successfully deleted!");
                        }
                    }
                }
                catch (FaultException ex)
                {
                    UIHelper.ShowErrorMessage(ex.Message);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        private void cmdUpdate_Execute(object obj)
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to Update it", "Confirmation", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, System.Windows.MessageBoxResult.No);
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    if (obj is InvoiceDetailsService)
                    {
                        InvoiceDetailsService invoiceDetails = obj as InvoiceDetailsService;
                        if (pxyInvoice.UpdateInvoiceDetails(invoiceDetails, objCustomerDetails, Global.UserID))
                        {
                            selectedInvoiceDetails = invoiceDetails;
                            UIHelper.ShowMessage("Data successfully updated!");
                        }
                    }
                    else if (obj is ProductDetails)
                    {
                        ProductDetails invoiceProduct = obj as ProductDetails;
                        if (pxyInvoice.UpdateInvoiceProduct(invoiceProduct, Global.UserID))
                        {
                            selectedProductDetails = invoiceProduct;
                            UIHelper.ShowMessage("Data successfully updated!");
                        }
                    }
                    else if (obj is SettlementDetails)
                    {
                        SettlementDetails invoiceSettlement = obj as SettlementDetails;
                        if (pxyInvoice.UpdateInvoiceSettlement(invoiceSettlement, Global.UserID))
                        {
                            UIHelper.ShowMessage("Data successfully updated!");
                        }
                    }
                }
                catch (FaultException ex)
                {
                    UIHelper.ShowErrorMessage(ex.Message);
                }
            }
        }

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
                if (lstProductDetails != null)
                {
                    lstProductDetails.Clear();
                }
                objCustomerDetails = new Customer();
                if (lstSettlementDetails != null)
                {
                    lstSettlementDetails.Clear();
                }
                if (lstInvoiceDetails == null || lstInvoiceDetails.Count == 0)
                {
                    UIHelper.ShowMessage("Invoice details not found");
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

        private void cmdPrint_Execute(object obj)
        {
            if (obj is InvoiceDetailsService)
            {
                InvoiceDetailsService invoiceProduct = obj as InvoiceDetailsService;
                if (invoiceProduct == null || invoiceProduct.FinalStatus == 8)
                {
                    UIHelper.ShowErrorMessage("Duplicate Invoice!");
                    return;
                }
                IMS.Views.GenReports win = new Views.GenReports(invoiceProduct.InvoiceNo, invoiceProduct.InvoiceDate);
                win.ShowDialog();
            }
            else if (obj is ProductDetails)
            {
                ProductDetails invoiceProduct = obj as ProductDetails;
                if (invoiceProduct == null || invoiceProduct.Status == 8)
                {
                    UIHelper.ShowErrorMessage("Duplicate Invoice!");
                    return;
                }
                IMS.Views.GenReports win = new Views.GenReports(invoiceProduct.InvoiceNo, invoiceProduct.InvoiceDate);
                win.ShowDialog();
            }
            else if (obj is SettlementDetails)
            {
                SettlementDetails invoiceProduct = obj as SettlementDetails;
                if (invoiceProduct == null || invoiceProduct.Status == 8)
                {
                    UIHelper.ShowErrorMessage("Duplicate Invoice!");
                    return;
                }
                IMS.Views.GenReports win = new Views.GenReports(invoiceProduct.InvoiceNo, invoiceProduct.InvoiceDate);
                win.ShowDialog();
            }
        }
    }
}