using System;
using System.Windows;
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
                this.Close();
            }
        }

        private void SignUpBtn_OnClick(object sender, RoutedEventArgs e)
        {
            UserInfo user = new UserInfo(
                FullName.Text,
                DateOfBirth.DisplayDate,
                Email.Text,
                PhoneNumber.Text,
                "",
                UserName.Text,
                Password.Password
                );
            Console.WriteLine(user);
        }
    }
}