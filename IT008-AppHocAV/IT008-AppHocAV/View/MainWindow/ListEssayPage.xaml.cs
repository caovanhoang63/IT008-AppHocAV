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
        
        public ListEssayPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;

            _data = _mainWindow.DbConnection.SelectListEssayByUserId(_mainWindow.UserId);

            ListEssayListView.ItemsSource = _data;

            
        }

        private void NewEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("Writing");
        }
    }
}