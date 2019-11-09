using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Stock : UserControl
    {
        public Stock()
        {
            InitializeComponent();
            this.DataContext = new StockViewModel();
        }
    }
}
