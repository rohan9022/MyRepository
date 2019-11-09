using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceDetailsMaster.xaml
    /// </summary>
    public partial class InvoiceDetailsMaster : UserControl
    {
        public InvoiceDetailsMaster()
        {
            InitializeComponent();
            this.DataContext = new InvoiceDetailsViewModel();
        }
    }
}
