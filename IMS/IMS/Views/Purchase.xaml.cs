using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Purchase.xaml
    /// </summary>
    public partial class Purchase : UserControl
    {
        public Purchase(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new PurchaseViewModel(PageMode);
        }
    }
}
