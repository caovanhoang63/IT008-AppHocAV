using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using IT008_AppHocAV.View.CustomMessageBox;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class UserInfoPage : Page
    {
        private IT008_AppHocAV.MainWindow _mainWindow;
        private DbConnection _dbConnection;
        public BitmapImage DefaultAvatar { get; set; } =  new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/AvatarIcon.png"));
        public string DateOfBirth { get; set; }
        public User User { get; set; } 
        public UserInfoPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _dbConnection = _mainWindow.DbConnection;
            User = _dbConnection.UserRepository.GetUserById(_mainWindow.UserId);
            DateOfBirth = User.DateOfBirth.ToString("dd/MM/yyyy");
            DataContext = this;
        }

        private void AvatarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            AddImageIcon.Visibility = Visibility.Visible;
            AvatarImage.Opacity = 0.4;
        }

        private void AvatarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            AddImageIcon.Visibility = Visibility.Hidden;
            AvatarImage.Opacity = 1;
        }

        private void AvatarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dialog.ShowDialog() == true)
            {
                var uri = new Uri(dialog.FileName);
                BitmapImage bitmap = new BitmapImage(uri);
                AvatarImage.Source = bitmap;
                _dbConnection.UserRepository.UpdateAvatar(_mainWindow.UserId, bitmap);
            } 
        }

        private void ChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow resetPasswordWindow = new ResetPasswordWindow(_mainWindow);
            resetPasswordWindow.ShowDialog();
        }

        private void EditInfo_OnClick(object sender, RoutedEventArgs e)
        {
            FullNameTb.IsReadOnly = false;
            EmailTb.IsReadOnly = false;
            PhoneNumberTb.IsReadOnly = false;
            DateOfBirthPicker.Visibility = Visibility.Visible;
            DateOfBirthTb.Visibility = Visibility.Collapsed;
            EditInfoButton.Visibility = Visibility.Collapsed;
            SaveInfoButton.Visibility = Visibility.Visible;
            
            FullNameTb.BorderThickness= new Thickness(0, 0, 0, 1);
            EmailTb.BorderThickness = new Thickness(0, 0, 0, 1);
            PhoneNumberTb.BorderThickness = new Thickness(0, 0, 0, 1);
            
        }

        private void SaveInfo_OnClick(object sender, RoutedEventArgs e)
        {
            FullNameTb.IsReadOnly = true;
            EmailTb.IsReadOnly = true;
            PhoneNumberTb.IsReadOnly = true;
            DateOfBirthPicker.Visibility = Visibility.Collapsed;
            DateOfBirthTb.Visibility = Visibility.Visible;
            EditInfoButton.Visibility = Visibility.Visible;
            SaveInfoButton.Visibility = Visibility.Collapsed;
            
            FullNameTb.BorderThickness= new Thickness(0, 0, 0, 0);
            EmailTb.BorderThickness = new Thickness(0, 0, 0, 0);
            PhoneNumberTb.BorderThickness = new Thickness(0, 0, 0, 0);

            try
            {
                _dbConnection.UserRepository.UpdateUserInfo(_mainWindow.UserId, FullNameTb.Text, EmailTb.Text,
                    PhoneNumberTb.Text, DateOfBirthPicker.SelectedDate.Value);
               
                 CTMessageBox.Show("Message", "Saved successfully!", MessageBoxType.Information);

            }
            catch
            {
                CTMessageBox.Show("Message", "Saved fail!", MessageBoxType.Error);

            }   
            
        }
    }
}