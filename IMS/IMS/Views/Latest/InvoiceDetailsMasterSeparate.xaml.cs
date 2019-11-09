using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceDetailsMasterSeparate.xaml
    /// </summary>
    public partial class InvoiceDetailsMasterSeparate : UserControl
    {
        public InvoiceDetailsMasterSeparate()
        {
            InitializeComponent();
            this.DataContext = new InvoiceDetailsMasterSeparateViewModel();
        }
    }
}
