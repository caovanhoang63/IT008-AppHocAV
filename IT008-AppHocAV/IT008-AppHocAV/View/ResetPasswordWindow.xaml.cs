﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using IT008_AppHocAV.Repositories.DbConnection;
using IT008_AppHocAV.Util.SendResetPasswordMail;
using IT008_AppHocAV.View.CustomMessageBox;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ResetPasswordWindow : Window
    {
        private int _code;
        
        private LoginWindow _loginWindow = null;
        private IT008_AppHocAV.MainWindow _mainWindow = null;
        private DispatcherTimer _timer;
        private DbConnection _dbConnection;
        private int _id;
        private int _timeCount = 5 * 60;
        public ResetPasswordWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            _loginWindow = loginWindow;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Tick;
            _dbConnection = _loginWindow.DbConnection;
        }
        
        public ResetPasswordWindow(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            InputEmailContainer.Visibility = Visibility.Collapsed;
            InputPinContainer.Visibility = Visibility.Collapsed;
            ChangePasswordContainer.Visibility = Visibility.Visible;
            _mainWindow = mainWindow;
            _dbConnection = _mainWindow.DbConnection;
            _id = _mainWindow.UserId;
        }

        private void Tick(object sender, EventArgs args)
        {

            _timeCount--;
            int minute = _timeCount/60;
            int second = _timeCount - minute * 60;
            if (second < 10)
            {
                TimeLabel.Content = minute +":0" + second;
            }
            else
            {
                TimeLabel.Content = minute +":" + second;
            }
            if (_timeCount == 0)
            {
                _timer.Stop();
            }

        }
        
        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            if (_loginWindow != null)
            {
                _loginWindow.Show();
            }
            else
            {
                _mainWindow.Show();
            }
            Close();
        }

        private void BoxBorder_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Border bd)
            {
                bd.BorderBrush = Brushes.CornflowerBlue;
            }
        }

        private void BoxBorder_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Border bd)
            {
                bd.BorderBrush = Brushes.Black;
            }
        }

        private async void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            _id =  _dbConnection.UserRepository.FindIdByEmail(email);
            if (_id == -1)
            {
                
                 CTMessageBox.Show("Message", "Email does not exist!", MessageBoxType.Error);

                return;
            }
            
            InputEmailContainer.Visibility = Visibility.Collapsed;
            InputPinContainer.Visibility = Visibility.Visible;
            _timer.Start();
            
            _code = await Task.Run( () => SendEMail.Send(email));
        }

        

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void PinContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            int inputCode = int.Parse(PinTextBox.Text);
            if (inputCode != _code)
            {
                
                CTMessageBox.Show("Message", "Pin does not match!", MessageBoxType.Error);
                return;
            }

            if (_timeCount == 0)
            {
               
                CTMessageBox.Show("Message", "Pin expired!", MessageBoxType.Error);
                return;
            }

            ChangePasswordContainer.Visibility = Visibility.Visible;
            InputPinContainer.Visibility = Visibility.Collapsed;
        }

        private async void ResendButton_OnClick(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            _timeCount = 5 * 60;
            _timer.Start();
            _code = await Task.Run( () => SendEMail.Send(email));
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!Util.CheckValidate.IsValidPassword(PasswordBox.Password))
            {
                InvalidPasswordLabel.Visibility = Visibility.Visible;
            }
            else
            {
                InvalidPasswordLabel.Visibility = Visibility.Collapsed;
            }
        }


        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Util.CheckValidate.IsValidPassword(PasswordBox.Password))
                return;
            if (!PasswordBox.Password.Equals(ConfirmPassword.Password))
            {
               
                CTMessageBox.Show("Message", "Password does not match!", MessageBoxType.Error);
                return;
            }

            if (_dbConnection.UserRepository.UpdatePasswordById(_id, PasswordBox.Password))
            {
                 
                CTMessageBox.Show("Message", "Password was changed!", MessageBoxType.Error);
                if (_loginWindow != null)
                {
                    _loginWindow.Show();
                }
                else
                {
                    _mainWindow.Show();
                }
                this.Close();
            }
            else
            {
                CTMessageBox.Show("Message", "Error!", MessageBoxType.Error);
            }
        }
    }
}