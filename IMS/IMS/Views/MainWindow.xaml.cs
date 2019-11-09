using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using IMS.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataServiceClient pxy;

        public MainWindow()
        {
            InitializeComponent();
            LayoutRoot.Loaded += LayoutRoot_Loaded;
            LayoutRoot.Unloaded += LayoutRoot_Unloaded;
            Global.GlobalPageMode = PageMode.Browse;
            btnBrowse.Background = new SolidColorBrush(Colors.Blue);
        }

        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            if (pxy != null) pxy.Close();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            if (pxy == null) pxy = new DataServiceClient();
            txtUserName.Text = "Welcome, " + Global.UserName + "  ";

            if (Global.UserType == Const.User)
            {
                Global.DashBoard = false;
                MainRegion.Navigate(new Product(Global.GlobalPageMode));
                return;
            }
            MainRegion.Navigate(new Dashboard(MainRegion));
        }

        private void PART_TITLEBAR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            Helper.Helper.WindowNavigation(System.Convert.ToInt32(img.Tag), MainRegion);
        }

        private void imgLogout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to Logout?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mresult == MessageBoxResult.Yes)
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
                this.Close();
                login.ShowDialog();
            }
        }

        private void Change_Mode(object sender, RoutedEventArgs e)
        {
            try
            {
                Button obj = sender as Button;
                if (Global.GlobalPageMode == System.Convert.ToInt32(obj.Tag)) return;
                Global.GlobalPageMode = System.Convert.ToInt32(obj.Tag);
                btnBrowse.Background = btnAdd.Background = btnModify.Background = btnDelete.Background = new SolidColorBrush(Colors.White);
                if (Global.GlobalPageMode == PageMode.Browse) btnBrowse.Background = new SolidColorBrush(Colors.Blue);
                if (Global.GlobalPageMode == PageMode.Add) btnAdd.Background = new SolidColorBrush(Colors.Blue);
                if (Global.GlobalPageMode == PageMode.Modify) btnModify.Background = new SolidColorBrush(Colors.Blue);
                if (Global.GlobalPageMode == PageMode.Delete) btnDelete.Background = new SolidColorBrush(Colors.Blue);
                Helper.Helper.WindowNavigation(Global.GlobalNavigationValue, MainRegion);
            }
            catch
            {
                // DDA
            }
        }

        private void btnDbBackup_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
            browse.ShowDialog();
            string folderPath = browse.SelectedPath;
            if (string.IsNullOrEmpty(folderPath.Trim()))
            {
                UIHelper.ShowErrorMessage("Select Path to save backup");
                return;
            }
            try
            {
                DatabaseBackupService objDbBackup = new DatabaseBackupService(Const.DatabaseServer, folderPath);
                objDbBackup.BackupDatabase(Const.DatabaseName);
                UIHelper.ShowMessage("Backup created!");
            }
            catch
            {
                UIHelper.ShowErrorMessage("ERROR!");
            }
        }
    }
}