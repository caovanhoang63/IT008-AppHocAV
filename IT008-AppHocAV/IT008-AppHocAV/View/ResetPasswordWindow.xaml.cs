using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using IT008_AppHocAV.Repositories.DbConnection;

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
            _id =  _dbConnection.UserQ.FindIdByEmail(email);
            if (_id == -1)
            {
                MessageBox.Show("Email does not exist!");
                return;
            }
            
            InputEmailContainer.Visibility = Visibility.Collapsed;
            InputPinContainer.Visibility = Visibility.Visible;
            _timer.Start();
            
            _code = await Task.Run( () => Util.SendEMail.Send(email));
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
                MessageBox.Show("Pin does not match!");
                return;
            }

            if (_timeCount == 0)
            {
                MessageBox.Show("Pin expired!");
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
            _code = await Task.Run( () => Util.SendEMail.Send(email));
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
                MessageBox.Show("Password does not match!");
                return;
            }

            if (_dbConnection.UserQ.UpdatePasswordById(_id, PasswordBox.Password))
            {
                MessageBox.Show("Password was changed!");
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
                MessageBox.Show("Error!");
            }
        }
    }
}