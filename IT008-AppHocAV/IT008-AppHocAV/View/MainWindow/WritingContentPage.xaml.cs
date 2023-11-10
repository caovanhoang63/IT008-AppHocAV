using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class WritingContentPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private readonly WritingPage _writingPage;
        public WritingContentPage(IT008_AppHocAV.MainWindow mainWindow, WritingPage writingPage)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            this._writingPage = writingPage;
        }

        private void WritingContentPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Text = _writingPage.Essay.Title;
            TopicTextBlock.Text = _writingPage.Essay.Topic;
            
            if (_writingPage.Essay.Image != null)
            {
                WritingImage.Source = _writingPage.Essay.Image;
            }
            else
            {
                WritingImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}