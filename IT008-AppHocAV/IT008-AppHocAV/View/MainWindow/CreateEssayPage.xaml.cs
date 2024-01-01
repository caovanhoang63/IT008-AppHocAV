using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using Microsoft.Win32;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class CreateEssayPage : Page
    {
        #region Declare Fields
        
            private readonly IT008_AppHocAV.MainWindow _mainWindow;
            private Essay _essay;
            
        #endregion

        #region Delare Constructors
            public CreateEssayPage()
            {
                InitializeComponent();
            }
            
            public CreateEssayPage(IT008_AppHocAV.MainWindow mainWindow)
            {
                InitializeComponent();
                this._mainWindow = mainWindow;
                _essay = new Essay();
                _essay.ImagePath = null;
                _essay.ImagePath = null;
            }
            
        #endregion

        #region Declare Properties

            public Essay Essay
            {
                get => _essay;
            }

        #endregion

        #region Define Click Event Handlers of UI Buttons

            private void StartWritingButton_OnClick(object sender, RoutedEventArgs e)
            {
                _essay.UserId = _mainWindow.UserId;
                _essay.Title = TitleTextBox.Text;
                _essay.Topic = TopicTextBox.Text;
                _essay.CreatedAt =  DateTime.Now; 
                _essay.UpdatedAt =  DateTime.Now;
                
                _essay.Id =  _mainWindow.DbConnection.EssayRepository.CreateEssay(_essay);
                if (_essay.Id == 0)
                {
                    MessageBox.Show("Fail to create new essay!");
                    return;
                }
                _mainWindow.NavigateToPage("WritingContent");
                _mainWindow.PageCache.Remove("CreateEssay");
                _mainWindow.PageCache.Remove("Writing");
            }


            private void AddImageButton_OnClick(object sender, RoutedEventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png;...";
                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    if (fileName != String.Empty)
                    {
                        ImagePath.Text = fileName;
                        BitmapImage image = new BitmapImage(new Uri(fileName));
                        WritingImage.Source = image;
                        ImagePath.Visibility = Visibility.Visible;
                        WritingImage.Visibility = Visibility.Visible;

                        _essay.ImagePath = ImagePath.Text;
                        _essay.Image = image;

                    }
                }
            }

            private void CloseButton_OnClick(object sender, RoutedEventArgs e)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure want to continue?","",MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _mainWindow.PageCache.Remove("Writing");
                    _mainWindow.PageCache.Remove("CreateEssay");
                    _mainWindow.NavigateToPage("ShowListEssay");
                }
            }

        #endregion
    }
}