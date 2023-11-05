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
        }
        
        private readonly BitmapImage _showPwdIcon = new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/showpwdIcon.png"));
        private readonly BitmapImage _hidePwdIcon = new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/hidepwdIcon.png"));
        private bool _isShowPassword;
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
                if (UserNameBox.Text == "admin" && PasswordBox.Password == "admin")
                {
                    IT008_AppHocAV.MainWindow mainWindow = new IT008_AppHocAV.MainWindow(this);
                    Hide();
                    mainWindow.Show();
                }
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

      
    }
}