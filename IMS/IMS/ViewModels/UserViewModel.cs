using IMSLibrary.UI;
using IMS.DataServiceRef;
using IMS.Helper;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace IMS.ViewModels
{
    internal class UserViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly DataServiceClient pxy;
        private readonly int Mode;
        private readonly Dictionary<string, string> Errors;

        private string _modeContent;

        public string modeContent
        {
            get { return _modeContent; }
            set
            {
                _modeContent = value;
                OnPropertyChanged(this, "modeContent");
            }
        }

        private ObservableCollection<Lookup> _lstGender;

        public ObservableCollection<Lookup> lstGender
        {
            get { return _lstGender; }
            set
            {
                _lstGender = value;
                OnPropertyChanged(this, "lstGender");
            }
        }

        private ObservableCollection<Lookup> _lstUserType;

        public ObservableCollection<Lookup> lstUserType
        {
            get { return _lstUserType; }
            set
            {
                _lstUserType = value;
                OnPropertyChanged(this, "lstUserType");
            }
        }

        private UserDetails _objUserDetails;

        public UserDetails objUserDetails
        {
            get
            {
                return _objUserDetails;
            }
            set
            {
                _objUserDetails = value;
                UserID = value.UserID;
                UserName = value.UserName;
                UserPassword = value.UserPassword;
                UserType = value.UserType;
                FirstName = value.FirstName;
                MiddleName = value.MiddleName;
                LastName = value.LastName;
                DateOfBirth = value.DateOfBirth;
                Gender = value.Gender;
                ContactNo = value.ContactNo;
                Email = value.Email;
            }
        }

        public int UserID
        {
            get { return _objUserDetails.UserID; }
            set
            {
                _objUserDetails.UserID = value;
                OnPropertyChanged(this, "UserID");
            }
        }

        public string UserName
        {
            get { return _objUserDetails.UserName; }
            set
            {
                _objUserDetails.UserName = value;
                OnPropertyChanged(this, "UserName");
            }
        }

        public string UserPassword
        {
            get { return _objUserDetails.UserPassword; }
            set
            {
                _objUserDetails.UserPassword = value;
                OnPropertyChanged(this, "UserPassword");
            }
        }

        public int UserType
        {
            get { return _objUserDetails.UserType; }
            set
            {
                _objUserDetails.UserType = value;
                OnPropertyChanged(this, "UserType");
            }
        }

        public string FirstName
        {
            get { return _objUserDetails.FirstName; }
            set
            {
                _objUserDetails.FirstName = value;
                OnPropertyChanged(this, "FirstName");
            }
        }

        public string MiddleName
        {
            get { return objUserDetails.MiddleName; }
            set
            {
                objUserDetails.MiddleName = value;
                OnPropertyChanged(this, "MiddleName");
            }
        }

        public string LastName
        {
            get { return objUserDetails.LastName; }
            set
            {
                objUserDetails.LastName = value;
                OnPropertyChanged(this, "LastName");
            }
        }

        public DateTime DateOfBirth
        {
            get { return objUserDetails.DateOfBirth; }
            set
            {
                objUserDetails.DateOfBirth = value;
                OnPropertyChanged(this, "DateOfBirth");
            }
        }

        public int Gender
        {
            get { return _objUserDetails.Gender; }
            set
            {
                _objUserDetails.Gender = value;
                OnPropertyChanged(this, "Gender");
            }
        }

        public string ContactNo
        {
            get { return objUserDetails.ContactNo; }
            set
            {
                objUserDetails.ContactNo = value;
                OnPropertyChanged(this, "ContactNo");
            }
        }

        public string Email
        {
            get { return objUserDetails.Email; }
            set
            {
                objUserDetails.Email = value;
                OnPropertyChanged(this, "Email");
            }
        }

        private string _ConfirmPassword;

        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set
            {
                _ConfirmPassword = value;
                OnPropertyChanged(this, "ConfirmPassword");
            }
        }

        private bool _Pk { get; set; }

        public bool Pk
        {
            get { return _Pk; }
            set
            {
                _Pk = value;
                OnPropertyChanged(this, "Pk");
            }
        }

        private bool _Ck { get; set; }

        public bool Ck
        {
            get { return _Ck; }
            set
            {
                _Ck = value;
                OnPropertyChanged(this, "Ck");
            }
        }

        public DelegateCommand<object> cmdUser { get; set; }
        public DelegateCommand<object> cmdUserDetails { get; set; }

        private void ModeAction()
        {
            try
            {
                switch (Mode)
                {
                    case PageMode.Add:
                        {
                            Pk = false;
                            Ck = true;
                            modeContent = "Save";
                        }
                        break;

                    case PageMode.Modify:
                        {
                            lstUser = pxy.GetUserList();
                            Pk = true;
                            Ck = false;
                            modeContent = "Update";
                        }
                        break;

                    case PageMode.Delete:
                        {
                            lstUser = pxy.GetUserList();
                            Pk = true;
                            Ck = false;
                            modeContent = "Delete";
                        }
                        break;

                    case PageMode.Browse:
                        {
                            lstUser = pxy.GetUserList();
                            Pk = true;
                            Ck = false;
                            modeContent = "Ok";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        public UserViewModel(int PageMode)
        {
            Mode = PageMode;
            pxy = new DataServiceClient();
            objUserDetails = new UserDetails();
            lstUser = new ObservableCollection<UserList>();
            lstGender = new ObservableCollection<Lookup>();
            lstUserType = new ObservableCollection<Lookup>();
            cmdUser = new DelegateCommand<object>(cmdUser_Execute, cmdUser_CanExecute);
            cmdUserDetails = new DelegateCommand<object>(cmdUserDetails_Execute, cmdUserDetails_CanExecute);
            lstGender = pxy.GetSubLookupList(Const.Gender);
            lstUserType = pxy.GetSubLookupList(Const.UserType);
            ModeAction();
            Errors = new Dictionary<string, string>();
        }

        private bool cmdUserDetails_CanExecute(object obj)
        {
            if (Mode != PageMode.Add) return true;
            return false;
        }

        private void cmdUserDetails_Execute(object obj)
        {
            try
            {
                if (UserID > 0)
                {
                    objUserDetails = pxy.GetUserDetails(UserID);
                }
                if (Mode == PageMode.Modify) { Pk = false; Ck = true; }
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private ObservableCollection<UserList> _lstUser;

        public ObservableCollection<UserList> lstUser
        {
            get { return _lstUser; }
            set
            {
                _lstUser = value;
                OnPropertyChanged(this, "lstUser");
            }
        }

        private void cmdUser_Execute(object obj)
        {
            try
            {
                if (Mode == PageMode.Add || (Mode == PageMode.Modify && UserID > 0))
                {
                    pxy.InsertUpdateUserDetails(objUserDetails, Global.UserID);
                    UIHelper.ShowMessage("Data Saved Successfully");
                    lstUser = pxy.GetUserList();
                }
            }
            catch (FaultException ex)
            {
                UIHelper.ShowErrorMessage(ex.Message);
            }
        }

        private bool cmdUser_CanExecute(object obj)
        {
            if (((UserID > 0 && Mode != PageMode.Add) || (Mode == PageMode.Add)) && Errors.Count == 0) return true;
            return false;
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private readonly Regex rgxName = new Regex("^[a-zA-Z]+$");
        private readonly Regex rgxContactNo = new Regex(@"[0-9]");
        private readonly Regex rgxEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        public string this[string columnName]
        {
            get
            {
                string Result = string.Empty;
                bool loginError = false;
                Errors.Remove(columnName);
                switch (columnName)
                {
                    case "UserID": if (UserID <= 0 && Mode != PageMode.Add) Result = "Select User"; break;
                    case "UserType": if (UserType <= 0) Result = "Select UserType"; break;
                    case "UserName":
                        if (string.IsNullOrEmpty(UserName)) { Result = "Enter Username"; loginError = true; }
                        else if (pxy.CheckUserNameAvailability(UserName)) { Result = "Username Not Available"; loginError = true; }
                        break;

                    case "UserPassword": if (string.IsNullOrEmpty(UserPassword)) { Result = "Enter Password"; loginError = true; } break;
                    case "ConfirmPassword": if (ConfirmPassword != UserPassword) { Result = "Confirm Password Should be Match with Password"; loginError = true; } break;
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName)) Result = "Enter First Name";
                        else if (!rgxName.IsMatch(FirstName)) Result = "Invalid First Name"; break;
                    case "MiddleName":
                        if (string.IsNullOrEmpty(MiddleName)) Result = "Enter Middle Name";
                        else if (!rgxName.IsMatch(MiddleName)) Result = "Invalid Middle Name"; break;
                    case "LastName":
                        if (string.IsNullOrEmpty(LastName)) Result = "Enter Last Name";
                        else if (!rgxName.IsMatch(LastName)) Result = "Invalid LastName Name"; break;
                    case "ContactNo":
                        if (string.IsNullOrEmpty(ContactNo)) Result = "Enter Contact Number";
                        else if (!rgxContactNo.IsMatch(ContactNo)) Result = "Contact Number Not Valid"; break;
                    case "Email":
                        if (string.IsNullOrEmpty(Email)) Result = "Enter Email ID";
                        else if (!rgxEmail.IsMatch(Email)) Result = "Email Not Valid"; break;
                    case "Gender": if (Gender <= 0) Result = "Select Gender"; break;
                }
                if (!string.IsNullOrEmpty(Result) && !loginError) Errors.Add(columnName, Result);
                return Result;
            }
        }
    }
}