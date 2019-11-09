using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        public Product(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new ProductViewModel(PageMode);
        }
    }
}
