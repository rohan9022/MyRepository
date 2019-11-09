using System.Windows;
using System.Windows.Controls;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Patch.xaml
    /// </summary>
    public partial class Patch : UserControl
    {
        public Patch()
        {
            InitializeComponent();
        }

        private void cmdPatch_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Helper.Helper.WindowNavigation(System.Convert.ToInt32(btn.Tag), MainRegion);
        }
    }
}
