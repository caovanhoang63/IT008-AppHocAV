using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ShowEssayPage : Page
    {
        #region Declare Atributes
            private readonly IT008_AppHocAV.MainWindow _mainWindow;
            private readonly ShowListEssayPage _listEssayPage;
            private readonly Essay _essay;
        #endregion

        #region Declare Constructor
            public ShowEssayPage(IT008_AppHocAV.MainWindow mainWindow,ShowListEssayPage listEssayPage)
            {
                InitializeComponent();
                this._mainWindow = mainWindow;
                this._listEssayPage = listEssayPage;
                _essay = listEssayPage.CurrentEssay;
                this.DataContext = _essay;
            }
        
        #endregion

        #region Declare Properties

        public Essay Essay => _essay;

        #endregion

        #region Click Event Handler

        private void BackToListButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("ShowListEssay");
            _mainWindow.PageCache.Remove("ShowEssay");
        }

        private void ModifyEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("WritingContent");
            _mainWindow.PageCache.Remove("ShowEssay");
        }

        #endregion

        private void EditTopicButton_OnClick(object sender, RoutedEventArgs e)
        {
            TopicTextBox.IsReadOnly = false;
            TopicTextBox.SelectAll();
            TopicTextBox.Focus();
        }

        private void EditTitleButton_OnClick(object sender, RoutedEventArgs e)
        {
            TitleTextBox.IsReadOnly = false;
            TitleTextBox.BorderThickness = new Thickness(0,0,0,1);
            TitleTextBox.Focus();
        }

        private void DeleteEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this essay?","",MessageBoxButton.OKCancel);
            
            if (result == MessageBoxResult.Cancel)
                return;
            
            _mainWindow.DbConnection.EssayQ.DeleteEssayById(_essay.Id);
            _mainWindow.NavigateToPage("ShowListEssay");
            _mainWindow.PageCache.Remove("ShowEssay");
        }

        private void ShowEssayPage_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(TopicTextBox), null);
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(TitleTextBox), null);
                TitleTextBox.IsReadOnly = true;
                TopicTextBox.IsReadOnly = true;
                TitleTextBox.BorderThickness = new Thickness(0);
                Console.WriteLine(TopicTextBox.IsFocused);
                Console.WriteLine(TitleTextBox.IsFocused);
                _mainWindow.DbConnection.EssayQ.UpdateTitleAndTopic(_essay);
            }
        }
    }
}