using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ResetPasswordWindow : Window
    {
        private LoginWindow _loginWindow;
        public ResetPasswordWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            _loginWindow = loginWindow;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            _loginWindow.Show();
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

        private void LoginBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _loginWindow.Show();
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }




    }
}