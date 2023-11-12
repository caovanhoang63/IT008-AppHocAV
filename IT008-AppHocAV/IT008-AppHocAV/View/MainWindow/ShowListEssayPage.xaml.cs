using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ShowListEssayPage : Page
    {
        
        #region Declare Fields

            private List<Essay> _data;
            private readonly IT008_AppHocAV.MainWindow _mainWindow;
            private  Essay  _currentEssay ;

        #endregion

        #region Declare Constructors
            public ShowListEssayPage(IT008_AppHocAV.MainWindow mainWindow)
            {
                InitializeComponent();
                this._mainWindow = mainWindow;

                _data = _mainWindow.DbConnection.SelectListEssayByUserId(_mainWindow.UserId);
                _currentEssay = null;
                
                ListEssayListView.ItemsSource = _data;
            }
        #endregion

        #region Declare Properties

            public Essay CurrentEssay
            {
                get => _currentEssay;
            }

        #endregion

        #region Click Event Handlers
        
            private void NewEssayButton_OnClick(object sender, RoutedEventArgs e)
            {
                _mainWindow.NavigateToPage("CreateEssay");
            }


            private void ShowEssayButton_OnClick(object sender, RoutedEventArgs e)
            {
                var modelEssay = (Essay)((FrameworkElement)sender).DataContext;
                _currentEssay = _mainWindow.DbConnection.SelectEssayById(modelEssay.Id);
                _mainWindow.NavigateToPage("ShowEssay");
            }

            private void DeleteEssayButton_OnClick(object sender, RoutedEventArgs e)
            {
                e.Handled = true;
                MessageBoxResult result =
                    MessageBox.Show("You sure you want to delete this essay", null, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Essay modelEssay = (Essay)((FrameworkElement)sender).DataContext;
                    if (_mainWindow.DbConnection.DeleteEssayById(modelEssay.Id))
                    {

                        
                        foreach (Essay essay in _data)
                        {
                            if (essay.Id == modelEssay.Id)
                            {
                                _data.Remove(essay);
                                break;
                            }
                        }
                        
                        MessageBox.Show("Delete successes! ");
                        
                        ListEssayListView.Items.Refresh();

                    }
                    else 
                        MessageBox.Show("Delete fail! ");
                }
            }

        #endregion

    }
}