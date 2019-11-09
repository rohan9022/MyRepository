using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : UserControl
    {
        public Invoice(int PageMode)
        {
            InitializeComponent();
            this.DataContext = new InvoiceViewModel(PageMode);
        }
    }
}
