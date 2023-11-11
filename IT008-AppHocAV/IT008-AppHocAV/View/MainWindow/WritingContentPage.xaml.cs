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
        private Essay _essay;
        public WritingContentPage(IT008_AppHocAV.MainWindow mainWindow, Essay essay)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            this._essay = essay;
        }

        private void WritingContentPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Text = _essay.Title;
            TopicTextBlock.Text = _essay.Topic;
            
            if (_essay.Image != null)
            {
                WritingImage.Source = _essay.Image;
            }
            else
            {
                WritingImage.Visibility = Visibility.Collapsed;
            }

            ContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(_essay.Content)));

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(ContentRichTextBox.Document.ContentStart, ContentRichTextBox.Document.ContentEnd).Text;
            
            _mainWindow.DbConnection.UpdateEssayContent(_essay.Id,richText);
            
        }
    }
}