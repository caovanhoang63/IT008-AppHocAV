using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.View
{
    public partial class SignUpWindow : Window
    {

        #region Declare Fields
            private readonly LoginWindow _loginWindow;
        #endregion

        #region Declare Constructors
            public SignUpWindow(LoginWindow loginWindow)
            {
                InitializeComponent();
                this._loginWindow = loginWindow;
            }
        
        #endregion

        #region Dockbar Buttons Click Event Handlers

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

        #endregion

        #region Gets data from IU And Sign up
            private void SignUpBtn_OnClick(object sender, RoutedEventArgs e)
            {
                            
                if (!CheckValidateValue())
                    return;
                
                string gender = GetRadioButonsValue();

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
        

        #endregion
        
        #region Checks Validate and Handles When has Invalid Fields
            private void GenderRadioButton_OnChecked(object sender, RoutedEventArgs e)
            {
                InvalidGenderLabel.Visibility = Visibility.Hidden;
            }
            private bool CheckValidateValue()
            {
                
                bool flag = true;
                if (GetRadioButonsValue() == String.Empty)
                {
                    InvalidGenderLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (FullName.Text == String.Empty)
                {
                    FullName.BorderBrush = Brushes.Red;
                    InvalidFullNameLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (ConfirmPasswordBox.Password == String.Empty)
                {
                    ConfirmPasswordBox.BorderBrush = Brushes.Red;
                    InvalidConfirmPasswordLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (DateOfBirth.Text == "")
                {
                    DatePickerBorder.BorderBrush = Brushes.Red;
                    InvalidDateOfBirthLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (!CheckValidate.IsEmail(Email.Text))
                {
                    Email.BorderBrush = Brushes.Red;
                    InvalidEmailLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (!CheckValidate.IsPhoneNumber(PhoneNumber.Text))
                {
                    PhoneNumber.BorderBrush = Brushes.Red;
                    InvalidPhoneNumberLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (!CheckValidate.IsValidUserName(UserName.Text))
                {
                    UserName.BorderBrush = Brushes.Red;
                    InvalidUserNameLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                if (!CheckValidate.IsValidPassword(PasswordBox.Password))
                {
                    PasswordBox.BorderBrush = Brushes.Red;
                    InvalidPasswordLabel.Visibility = Visibility.Visible;
                    flag = false;
                }
                return flag;
            }

           private void Email_OnTextChanged(object sender, TextChangedEventArgs e)
            {
                if (!CheckValidate.IsEmail(Email.Text))
                {
                    Email.BorderBrush = Brushes.Red;
                    InvalidEmailLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    Email.BorderBrush = Brushes.Black;
                    InvalidEmailLabel.Visibility = Visibility.Hidden;
                }
            }

            private void PhoneNumber_OnTextChanged(object sender, TextChangedEventArgs e)
            {
                if (!CheckValidate.IsPhoneNumber(PhoneNumber.Text))
                {
                    PhoneNumber.BorderBrush = Brushes.Red;
                    InvalidPhoneNumberLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    PhoneNumber.BorderBrush = Brushes.Black;
                    InvalidPhoneNumberLabel.Visibility = Visibility.Hidden;
                }
            }

            private void UserName_OnTextChanged(object sender, TextChangedEventArgs e)
            {
                if (!CheckValidate.IsValidUserName(UserName.Text))
                {
                    UserName.BorderBrush = Brushes.Red;
                    InvalidUserNameLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    UserName.BorderBrush = Brushes.Black;
                    InvalidUserNameLabel.Visibility = Visibility.Hidden;
                }
            }

            private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
            {
                if (!CheckValidate.IsValidPassword(PasswordBox.Password))
                {
                    PasswordBox.BorderBrush = Brushes.Red;
                    InvalidPasswordLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    PasswordBox.BorderBrush = Brushes.Black;
                    InvalidPasswordLabel.Visibility = Visibility.Hidden;
                }
            }

            private void FullName_OnTextChanged(object sender, TextChangedEventArgs e)
            {
                if (FullName.Text == String.Empty)
                {
                    FullName.BorderBrush = Brushes.Red;
                    InvalidFullNameLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    FullName.BorderBrush = Brushes.Black;
                    InvalidFullNameLabel.Visibility = Visibility.Hidden;
                }
            }


            private void DateOfBirth_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
            {
                if (DateOfBirth.Text == String.Empty)
                {
                    DatePickerBorder.BorderBrush = Brushes.Red;
                    InvalidDateOfBirthLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    DatePickerBorder.BorderBrush = Brushes.Black;
                    InvalidDateOfBirthLabel.Visibility = Visibility.Hidden;
                }
            }

            private void ConfirmPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
            {
                if (ConfirmPasswordBox.Password == String.Empty)
                {
                    ConfirmPasswordBox.BorderBrush = Brushes.Red;
                    InvalidConfirmPasswordLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    ConfirmPasswordBox.BorderBrush = Brushes.Black;
                    InvalidConfirmPasswordLabel.Visibility = Visibility.Hidden;
                }
            }
        
        #endregion

    }
}