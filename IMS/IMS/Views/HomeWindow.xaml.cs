using IMS.ViewModels;
using System.Windows;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            this.DataContext = new HomeWindowViewModel();
        }
    }
}
