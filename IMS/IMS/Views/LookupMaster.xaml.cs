using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for LookupMaster.xaml
    /// </summary>
    public partial class LookupMaster : UserControl
    {
        public LookupMaster(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new LookupViewModel(PageMode);
        }
    }
}
