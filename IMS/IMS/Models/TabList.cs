using IMS.Helper;
namespace IMS.Models
{
    class TabList : ViewModelBase
    {
        public string headerName { get; set; }
        public System.Windows.Controls.ContentControl _contCtrl;
        public System.Windows.Controls.ContentControl contCtrl
        {
            get { return _contCtrl; }
            set { _contCtrl = value; OnPropertyChanged(this, "contCtrl"); }
        }
        public int PageMode { get; set; }
    }
}
