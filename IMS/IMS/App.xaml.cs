using IMS.ViewModels;
using IMS.Views;
using System.Windows;

namespace IMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            var login = new Login();
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
