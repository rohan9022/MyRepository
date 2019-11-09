using System.Windows.Controls;
using IMS.ViewModels;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceDetailsPatch.xaml
    /// </summary>
    public partial class InvoiceDetailsPatch : UserControl
    {
        public InvoiceDetailsPatch()
        {
            InitializeComponent();
            this.DataContext = new InvoiceDetailsPatchViewModel();
        }
    }
}
