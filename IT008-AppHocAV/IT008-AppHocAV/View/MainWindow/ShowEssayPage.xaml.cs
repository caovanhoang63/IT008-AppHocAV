using System;
using System.Windows;
using System.Windows.Controls;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ShowEssayPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private readonly ListEssayPage _listEssayPage;
        private readonly Essay _essay;
        public ShowEssayPage(IT008_AppHocAV.MainWindow mainWindow,ListEssayPage listEssayPage)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            this._listEssayPage = listEssayPage;
            Console.WriteLine(listEssayPage.CurrentEssay.Title);    
            _essay = listEssayPage.CurrentEssay;
            this.DataContext = _essay;
        }

        public Essay Essay => _essay;

        private void BackToListButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("ListEssayPage");
            _mainWindow.PageCache.Remove("ShowEssay");
        }

        private void ModifyEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("WritingContentPage");
        }
    }
}