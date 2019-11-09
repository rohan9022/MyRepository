using IMS.ViewModels;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for UploadSheet.xaml
    /// </summary>
    public partial class UploadSheet : UserControl
    {
        public UploadSheet()
        {
            InitializeComponent();
            this.DataContext = new UploadSheetViewModel();
        }
    }
}
