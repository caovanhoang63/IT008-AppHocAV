using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Services;
using IT008_AppHocAV.Util;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class WritingContentPage : Page
    {
        #region Declare Fields
            private readonly IT008_AppHocAV.MainWindow _mainWindow;
            private Essay _essay;
            private GptWritingResponsePopUp _popUp = new GptWritingResponsePopUp();
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
            
            if (_essay.Image != null)
            {
                WritingImage.Source = _essay.Image;
            }
            else
            {
                WritingImage.Visibility = Visibility.Collapsed;
            }
            ContentRichTextBox.Document.Blocks.Clear();
            ContentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(_essay.Content)));
            
            DataContext = _essay;

            
            
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
            _mainWindow.DbConnection.EssayQ.UpdateEssayContent(_essay.Id,WordCouting.WordCount(richText),richText);
            MessageBoxResult result = MessageBox.Show("Submit successes! Do you want to back to List Essay?","",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.PageCache.Remove("ShowListEssay");
                _mainWindow.PageCache.Remove("WritingContent");
                _mainWindow.NavigateToPage("ShowListEssay");
            }
        }

        /// <summary>
        /// Count number of words in content rich Text Box and display it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentRichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string richText = new TextRange(ContentRichTextBox.Document.ContentStart, ContentRichTextBox.Document.ContentEnd).Text;
            _mainWindow.WordNum.Text = WordCouting.WordCount(richText).ToString();
        }

        /// <summary>
        /// Closes this page and navigates _main window to Show list essay page  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result =  MessageBox.Show("Your changes will be discard! \n Are you sure want to continue?", "",MessageBoxButton.OKCancel);
            
            if (result == MessageBoxResult.OK)
            {
                _mainWindow.PageCache.Remove("WritingContent");
                _mainWindow.NavigateToPage("ShowListEssay");
            }
        }

        /// <summary>
        /// Open GPT Writing help MenuContext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GPTWrite_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenu contextMenu = ((Button)sender).ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.IsEnabled = true;
                contextMenu.PlacementTarget = (sender as Button);
                contextMenu.IsOpen = true;
            }
        }

        /// <summary>
        /// Handles Close Button's click event
        /// Close the GPT popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseGPTButton_OnClick(object sender, RoutedEventArgs e)
        {
            GptResponsePopUpGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Handles ResizeGPT Button's click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeGPTButton_OnClick(object sender, RoutedEventArgs e)
        {
            GptResponsePupUpContainer.Height = 350;
        }

        
        /// <summary>
        /// Handles GPT Writing Help and Pop up the box result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GptFunc_OnClick(object sender, RoutedEventArgs e)
        {
            GptResponsePopUpGrid.Visibility = Visibility.Visible;
            
            string answer = new TextRange(ContentRichTextBox.Document.ContentStart, ContentRichTextBox.Document.ContentEnd).Text;
            
            string topic = TopicTextBlock.Text;
            
            Func func = GetGptWritingFunc(sender);

            GptResponsePupUpContent.Content = _popUp;
            
            await _popUp.LoadResult(func,topic,answer);
        }


        
        /// <summary>
        /// Get Header from GPT func MenuContext
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Func GetGptWritingFunc(object sender)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Header)
                {
                    case "Ideas!":
                        return Func.Ideas;  
                    case "Outline":
                        return Func.OutLine;
                    case "Preview and enhance":
                        return Func.Enhance;
                    case "Lexical items":
                        return Func.Lexical;
                    case "Make a sample":
                        return Func.Sample; 
                    default:
                        throw new Exception("This function does not exist");
                }
            }

            throw new Exception("This function does not exist");
        }
    }
}