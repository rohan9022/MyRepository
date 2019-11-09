using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for InvoicingPatch.xaml
    /// </summary>
    public partial class InvoicingPatch : UserControl
    {
        public InvoicingPatch()
        {
            InitializeComponent();
            this.DataContext = new InvoicingPatchViewModel();
        }
    }
}
