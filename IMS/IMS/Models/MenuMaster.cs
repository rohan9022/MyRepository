using IMS.Helper;
using System.Collections.ObjectModel;
namespace IMS.Models
{
    class MenuMaster : ViewModelBase
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ScreenID { get; set; }
        private ObservableCollection<MenuMaster> _lstChild;
        public ObservableCollection<MenuMaster> lstChild
        {
            get { return _lstChild; }
            set
            {
                _lstChild = value;
                OnPropertyChanged(this, "lstChild");
            }
        }
    }
}
