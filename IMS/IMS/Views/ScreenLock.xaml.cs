using IMSLibrary.UI;
using IMS.Models;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;

namespace IMS.Views
{
    /// <summary>
    /// Interaction logic for ScreenLock.xaml
    /// </summary>
    public partial class ScreenLock : Window
    {
        bool Result;
        public ScreenLock()
        {
            InitializeComponent();
            this.Closing += ScreenLock_Closing;
            pwdBox.Focus();
            Result = false;
        }

        void ScreenLock_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Result)
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Do want to Exit ", "Seven Rays", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    System.Environment.Exit(0);
                }
                e.Cancel = true;
            }
        }
    
        private void SceenUnlock()
        {
            try
            {
                if (string.IsNullOrEmpty(pwdBox.Password))
                {
                    UIHelper.ShowErrorMessage("Please enter Password");
                    return;
                }
                if (CheckLogin(pwdBox.Password))
                {
                    Result = true;
                    this.Close();
                }
                else
                {
                    UIHelper.ShowErrorMessage("Invalid Login Details");
                    pwdBox.Password = string.Empty;
                    pwdBox.Focus();
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool CheckLogin(string userpassword)
        {
            try
            {
                IMS.DataServiceRef.DataServiceClient pxy = new DataServiceRef.DataServiceClient();
                IMS.DataServiceRef.UserList objUser = pxy.IsValidUser(Global.UserName, userpassword);
                if (objUser != null)
                {
                    pxy.Close();
                    return true;
                }
                pxy.Close();
                return false;
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
                return false;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SceenUnlock();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SceenUnlock();
            }
        }
    }
}
