using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;

namespace IMS.ViewModels
{
    internal class LoginViewModel : ViewModelBase, IDataErrorInfo
    {
        public DelegateCommand<object> cmdLogin { get; set; }
        public DelegateCommand<object> cmdCancel { get; set; }
        private readonly DataServiceClient pxy;

        private string _userName;

        public string userName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(this, "userName");
            }
        }

        private string _password;

        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(this, "password");
            }
        }

        public LoginViewModel()
        {
            if (pxy == null) pxy = new DataServiceClient();
            if (pxy.InnerChannel.State != CommunicationState.Faulted)
            {
                cmdLogin = new DelegateCommand<object>(cmdLogin_execute, cmdLogin_canExecute);
                cmdCancel = new DelegateCommand<object>(cmdCancel_execute);
            }
        }

        private void cmdCancel_execute(object obj)
        {
            MessageBoxResult mresult = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to exit?", "Login", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mresult == MessageBoxResult.Yes)
            {
                System.Environment.Exit(0);
            }
        }

        private bool cmdLogin_canExecute(object obj)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return false;
            return true;
        }

        public int Theme;

        private void cmdLogin_execute(object obj)
        {
            try
            {
                Theme = Convert.ToInt32(((System.Windows.Controls.ComboBoxItem)obj).Tag);
                UserList objUserList = pxy.IsValidUser(userName, password);
                Global.UserID = objUserList.UserID;
                Global.UserName = objUserList.UserName;
                Global.UserType = objUserList.UserType;
                RaiseLoginCompletedEvent();
            }
            catch (FaultException ex)
            {
                IMSLibrary.UI.UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        public string Error
        {
            get { throw new System.NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string Result = string.Empty;
                switch (columnName)
                {
                    case "userName": if (string.IsNullOrEmpty(userName)) Result = "UserName is Required"; break;
                    case "password": if (string.IsNullOrEmpty(password)) Result = "Please Enter Password"; break;
                }
                return Result;
            }
        }

        public event EventHandler LoginCompleted;

        private void RaiseLoginCompletedEvent()
        {
            LoginCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}