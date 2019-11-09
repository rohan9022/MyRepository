using System.Windows;
using System.Windows.Input;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            txtUserName.Focus();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to exit?", "Login", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mresult == MessageBoxResult.Yes)
                {
                    System.Environment.Exit(0);
                }
            }
        }
    }
}