using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Repositories;

namespace IT008_AppHocAV.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            _isShowPassword = false;
            _internetConnectionManager = new InternetConnectionManager();
            _internetConnectionManager.CheckInternetConnection();
            DbConnection = new DbConnection();
            UserId = 0;
        }
        
        private readonly BitmapImage _showPwdIcon = new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/showpwdIcon.png"));
        private readonly BitmapImage _hidePwdIcon = new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/hidepwdIcon.png"));
        private bool _isShowPassword;
        public DbConnection DbConnection;
        public int UserId;
        public InternetConnectionManager _internetConnectionManager;
        
        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DragMove();
        }

        private void ShowPassWordBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_isShowPassword)
            {
                VisiblePasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                ShowPassWordIcon.Source = _showPwdIcon;
                VisiblePasswordBox.Text = PasswordBox.Password;
                _isShowPassword = true;
            }
            else
            {
                VisiblePasswordBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                ShowPassWordIcon.Source = _hidePwdIcon;
                PasswordBox.Password = VisiblePasswordBox.Text;
                _isShowPassword = false;
            }
        }
        
        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
           Login();
        }

        private void Login()
        {
            if (VisiblePasswordBox.Visibility == Visibility.Visible)
            {
                PasswordBox.Password = VisiblePasswordBox.Text;
            }
            if (UserNameBox.Text != String.Empty && PasswordBox.Password != String.Empty )
            {
                UserId = DbConnection.Authentication(UserNameBox.Text, PasswordBox.Password);
                if (UserId != 0)
                {
                    IT008_AppHocAV.MainWindow mainWindow = new IT008_AppHocAV.MainWindow(this,UserId);
                    if (RememberMeCheckBox.IsChecked != null && RememberMeCheckBox.IsChecked.Value)
                    {
                        Properties.Settings.Default.UserName = UserNameBox.Text;
                        Properties.Settings.Default.Password = PasswordBox.Password;
                        Properties.Settings.Default.RememberMe = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.UserName = "";
                        Properties.Settings.Default.Password = "";
                        Properties.Settings.Default.RememberMe = false;
                        Properties.Settings.Default.Save();
                    }
                    Hide();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalide user name or password! \n" +
                                    "Try again!","Fail to login",MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Please enter your user name and password!","",MessageBoxButton.OK);
            }
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
        
        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SignUpWindow a = new SignUpWindow(this);
            a.Show();
            Hide();
        }

        private void LoginWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }


        private void LoginWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.RememberMe == true)
            {
                RememberMeCheckBox.IsChecked = true;
                UserNameBox.Text = Properties.Settings.Default.UserName;
                PasswordBox.Password = Properties.Settings.Default.Password;
            }
        }
    }
}