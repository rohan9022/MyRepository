using IMSLibrary.Class;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for AboutBox.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            GetRunningVersion();
        }

        private void hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            string uri = e.Uri.AbsoluteUri;
            Process.Start(new ProcessStartInfo(uri));
            e.Handled = true;
        }

        private void GetRunningVersion()
        {
            try
            {
                AssemblyInfo objAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
                version.Content = objAssemblyInfo.Version;
                copyright.Content = objAssemblyInfo.Copyright + " - " + objAssemblyInfo.Company;
                company.Content = objAssemblyInfo.Company;
                productName.Content = objAssemblyInfo.Product;
                description.Text = objAssemblyInfo.Description;
            }
            catch
            {
                // DDA
            }
        }
    }
}