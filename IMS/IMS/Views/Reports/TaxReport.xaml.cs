using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for TaxReport.xaml
    /// </summary>
    public partial class TaxReport : UserControl
    {
        public TaxReport()
        {
            InitializeComponent();
            this.DataContext = new TaxReportViewModel();
        }
    }
}
