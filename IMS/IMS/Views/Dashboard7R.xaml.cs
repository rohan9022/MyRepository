using IMS.Models;
using IMS.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for Dashboard7R.xaml
    /// </summary>
    public partial class Dashboard7R : UserControl
    {
        private Frame MainRegion;

        public Dashboard7R(Frame MainRegion)
        {
            InitializeComponent();
            this.MainRegion = MainRegion;
            Global.DashBoard = true;
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            Helper.Helper.WindowNavigation(System.Convert.ToInt32(img.Tag), MainRegion);
        }

        private void imgLogout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to Logout?", "", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            if (mresult == System.Windows.MessageBoxResult.Yes)
            {
                var login = new Login();
                System.Windows.Window parentWindow = System.Windows.Window.GetWindow(this);
                parentWindow.Close();
                var loginVM = new LoginViewModel();
                loginVM.LoginCompleted += (sender1, e1) =>
                {
                    if (loginVM.Theme == 1)
                    {
                        HomeWindow main = new HomeWindow();
                        main.Show();
                    }
                    else
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                    }
                    login.Close();
                };
                login.DataContext = loginVM;
                login.ShowDialog();
            }
        }
    }
}