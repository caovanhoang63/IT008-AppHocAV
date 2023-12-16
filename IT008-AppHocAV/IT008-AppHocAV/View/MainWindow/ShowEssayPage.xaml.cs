using System;
using System.Windows;
using System.Windows.Controls;
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Click");
        }

        private void EditTitleButton_OnClick(object sender, RoutedEventArgs e)
        {
            TitleTextBox.IsReadOnly = false;
            TitleTextBox.BorderThickness = new Thickness(0,0,0,1);
            TitleTextBox.Focus();
        }
    }
}