using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for SubCategoryMaster.xaml
    /// </summary>
    public partial class SubCategoryMaster : UserControl
    {
        public SubCategoryMaster(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new SubCategoryViewModel(PageMode);
        }
    }
}
