using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for SubLookupMaster.xaml
    /// </summary>
    public partial class SubLookupMaster : UserControl
    {
        public SubLookupMaster(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new SubLookupViewModel(PageMode);
        }
    }
}
