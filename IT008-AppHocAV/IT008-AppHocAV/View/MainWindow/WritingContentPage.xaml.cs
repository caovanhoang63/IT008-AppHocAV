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
        #region Declare Fields
            private readonly IT008_AppHocAV.MainWindow _mainWindow;
            private Essay _essay;
        #endregion

        #region Declare Constructors

            public WritingContentPage(IT008_AppHocAV.MainWindow mainWindow, Essay essay)
            {
                InitializeComponent();
                this._mainWindow = mainWindow;
                this._essay = essay;
            }

        #endregion

        
        /// <summary>
        /// Handles Loaded Event of this page
        /// and bindings data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            ContentRichTextBox.Selection.Text  = _essay.Content;
        }

        /// <summary>
        /// Handles Click Event for SubmitButton
        /// This update content of the essay, creates a new ShowListEssayPage and navigates to it. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            string richText = new TextRange(ContentRichTextBox.Document.ContentStart, ContentRichTextBox.Document.ContentEnd).Text;
            _mainWindow.DbConnection.UpdateEssayContent(_essay.Id,richText);
            MessageBoxResult result = MessageBox.Show("Submit successes! Do you want to back to List Essay?","",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.PageCache.Remove("ShowListEssay");
                _mainWindow.PageCache.Remove("WritingContent");
                _mainWindow.NavigateToPage("ShowListEssay");
            }
        }
    }
}