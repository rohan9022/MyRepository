using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for FinalInvoice.xaml
    /// </summary>
    public partial class FinalInvoice : UserControl
    {
        public FinalInvoice()
        {
            InitializeComponent();
            this.DataContext = new FinalInvoiceViewModel();
        }
    }
}
