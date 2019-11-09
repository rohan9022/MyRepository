using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Sale.xaml
    /// </summary>
    public partial class Sales : UserControl
    {
        public Sales(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new SalesViewModel(PageMode);
        }
    }
}
