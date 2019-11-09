using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for CreditNoteReportView.xaml
    /// </summary>
    public partial class CreditNoteReportView : UserControl
    {
        public CreditNoteReportView()
        {
            InitializeComponent();
            this.DataContext = new CreditNoteViewModel();
        }
    }
}
