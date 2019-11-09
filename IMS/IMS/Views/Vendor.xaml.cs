using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Vendor.xaml
    /// </summary>
    public partial class Vendor : UserControl
    {
        public Vendor(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new VendorViewModel(PageMode);
        }
    }
}
