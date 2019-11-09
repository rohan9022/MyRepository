using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceSettlementPatch.xaml
    /// </summary>
    public partial class InvoiceSettlementPatch : UserControl
    {
        public InvoiceSettlementPatch()
        {
            InitializeComponent();
            this.DataContext = new InvoiceSettlementPatchViewModel();
        }
    }
}
