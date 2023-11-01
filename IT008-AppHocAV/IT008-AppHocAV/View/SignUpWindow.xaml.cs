using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            this._loginWindow = loginWindow;
        }

        private readonly LoginWindow _loginWindow;
        
        private void SignUpPage_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =  MessageBox.Show("Are you sure want to continue?","",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _loginWindow.Show();
                Close();
            }
        }

        private void SignUpBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string gender = GetRadioButonsValue();

            if (HasEmptyField() || gender == "")
            {
                MessageBox.Show("Fill all field!");
                return;
            }
            
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show(
                    "Your passwords do not match", "",
                    MessageBoxButton.OK);
                return;
            }
            
            User user = new User(
                FullName.Text,
                DateOfBirth.DisplayDate,
                Email.Text,
                PhoneNumber.Text,
                gender,
                UserName.Text,
                PasswordBox.Password
            );
            
            int newUserId =  _loginWindow.DbConnection.NewUser(user);
            
            if (newUserId != 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Congratulation, you have a new account!\n" +
                    "Back to Login!","Sign up success",MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    Close();
                    _loginWindow.Show();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Fail to create new account =(( \n" +
                    "Wanna try again?","Sign up fail", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    Close();
                    _loginWindow.Show();
                } 
            }
        
        }

        private string GetRadioButonsValue()
        {
            string gender = "";
            if (MaleRadioButton.IsChecked != null && MaleRadioButton.IsChecked.Value)
            {
                gender = "Male";
            } else if (FemaleRadioButton.IsChecked != null && FemaleRadioButton.IsChecked.Value)
            {
                gender = "Female";
            } else if (OtherGenderRadioButton.IsChecked != null && OtherGenderRadioButton.IsChecked.Value)
            {
                gender = "Other";
            }
            return gender;
        }
        
        private bool HasEmptyField()
        {

            foreach (var child in InputStackPanel.Children)
            {
                if (child is TextBox textBox)
                    if (textBox.Text == String.Empty)
                        return true;
                
                if (child is PasswordBox passwordBox)
                    if (passwordBox.Password == String.Empty)
                        return true;
            }
            return false;
        }
    }
}