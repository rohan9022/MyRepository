using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceStatus.xaml
    /// </summary>
    public partial class InvoiceStatus : UserControl
    {
        public InvoiceStatus()
        {
            InitializeComponent();
            this.DataContext = new InvoiceStatusViewModel();
        }
    }
}
