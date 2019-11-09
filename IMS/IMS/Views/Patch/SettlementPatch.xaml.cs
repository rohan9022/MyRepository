using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for SettlementPatch.xaml
    /// </summary>
    public partial class SettlementPatch : UserControl
    {
        public SettlementPatch()
        {
            InitializeComponent();
            this.DataContext = new SettlementPatchViewModel();
        }
    }
}
