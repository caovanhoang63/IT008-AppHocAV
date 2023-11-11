using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ListEssayPage : Page
    {
        private List<Essay> _data;
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private  Essay  _currentEssay ;
        
        public ListEssayPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;

            _data = _mainWindow.DbConnection.SelectListEssayByUserId(_mainWindow.UserId);
            _currentEssay = null;
            
            ListEssayListView.ItemsSource = _data;
        }

        public Essay CurrentEssay
        {
            get => _currentEssay;
        }

        private void NewEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("Writing");
        }


        private void ShowEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var modelEssay = (Essay)((FrameworkElement)sender).DataContext;
            _currentEssay = _mainWindow.DbConnection.SelectEssayById(modelEssay.Id);
            _mainWindow.NavigateToPage("ShowEssay");
        }

        private void DeleteEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("You sure you want to delete this essay", null, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var modelEssay = (Essay)((FrameworkElement)sender).DataContext;
                if (_mainWindow.DbConnection.DeleteEssayById(modelEssay.Id))
                    MessageBox.Show("Delete successes! ");
                else 
                    MessageBox.Show("Delete fail! ");
            }

        }
    }
}