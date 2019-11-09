using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for BulkDeleteInvoice.xaml
    /// </summary>
    public partial class BulkDeleteInvoice : UserControl
    {
        public BulkDeleteInvoice()
        {
            InitializeComponent();
            this.DataContext = new BulkDeleteViewModel();
        }
    }
}
