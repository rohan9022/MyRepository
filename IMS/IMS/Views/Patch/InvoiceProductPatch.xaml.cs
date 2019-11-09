using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoiceProductPatch.xaml
    /// </summary>
    public partial class InvoiceProductPatch : UserControl
    {
        public InvoiceProductPatch()
        {
            InitializeComponent();
            this.DataContext = new InvoiceProductPatchViewModel();
        }
    }
}
