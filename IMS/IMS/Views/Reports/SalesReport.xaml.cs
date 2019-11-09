using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for SalesReport.xaml
    /// </summary>
    public partial class SalesReport : UserControl
    {
        public SalesReport()
        {
            InitializeComponent();
            this.DataContext = new SalesReportViewModel();
        }
    }
}
