using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for MenuMaster.xaml
    /// </summary>
    public partial class MenuMaster : UserControl
    {
        public MenuMaster()
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel();
        }

        private void trv_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null) item.Focus();
        }
    }
}
