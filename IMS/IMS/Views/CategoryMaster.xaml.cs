using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for CategoryMaster.xaml
    /// </summary>
    public partial class CategoryMaster : UserControl
    {
        public CategoryMaster(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new CategoryViewModel(PageMode);
        }
    }
}
