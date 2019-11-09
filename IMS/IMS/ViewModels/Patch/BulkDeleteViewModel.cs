using IMS.DataServiceRef;
using IMS.Helper;

namespace IMS.ViewModels
{
    public class BulkDeleteViewModel : ViewModelBase
    {
        public DelegateCommand<object> cmdDelete { get; set; }
        public DelegateCommand<object> cmdClear { get; set; }
        private readonly DataServiceClient pxy;

        private int _frmInvoiceNo;
        private int _toInvoiceNo;

        public int frmInvoiceNo
        {
            get { return _frmInvoiceNo; }
            set { _frmInvoiceNo = value; OnPropertyChanged(this, "frmInvoiceNo"); }
        }

        public int toInvoiceNo
        {
            get { return _toInvoiceNo; }
            set { _toInvoiceNo = value; OnPropertyChanged(this, "toInvoiceNo"); }
        }

        public BulkDeleteViewModel()
        {
            pxy = new DataServiceClient();
            cmdDelete = new DelegateCommand<object>(cmdDelete_Execute, cmdDelete_CanExecute);
            cmdClear = new DelegateCommand<object>(cmdClear_Execute);
        }

        private void cmdClear_Execute(object obj)
        {
            frmInvoiceNo = 0;
            toInvoiceNo = 0;
        }

        private bool cmdDelete_CanExecute(object obj)
        {
            if (frmInvoiceNo == 0 || toInvoiceNo == 0) return false;
            return true;
        }

        private void cmdDelete_Execute(object obj)
        {
            if (pxy.BulkDeleteInvoice(frmInvoiceNo, toInvoiceNo))
            {
                IMSLibrary.UI.UIHelper.ShowMessage("Data Deleted Successfully!!!");
            }
        }
    }
}