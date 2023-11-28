using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using IT008_AppHocAV.Components.RadioButtonMenuItem;
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
                        
                        RefreshPage();
                    }
                    else 
                        MessageBox.Show("Delete fail! ");
                }
            }

        #endregion

        #region Sorting
        private string _prevOrderBy = "Date modified";
        private string _prevOrderOption = "Descending";
        private void OrderBy_OnClick(object sender, RoutedEventArgs e)
        {
            
            if (sender is RadioButtonMenuItem radioButton)
            {
                string orderBy = radioButton.Header.ToString();
                if (radioButton.Header.ToString() == _prevOrderBy)
                {
                    if (_prevOrderOption == "Descending")
                    {
                        _prevOrderOption = "Ascending";
                        Ascending.IsChecked = true;
                    }
                    else
                    {
                        _prevOrderOption = "Descending";
                        Descending.IsChecked = true;
                    }
                    _data.Reverse();
                }
                else
                {
                    _data.Sort(SortEssayOptions.Factory(orderBy));
                }
                _prevOrderBy = orderBy;
                RefreshPage();
            }
        }
        
        private void OrderOption_OnClick(object sender, RoutedEventArgs e)
        {
            _data.Reverse();
            RefreshPage();
        }
        private void SortEssayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var contextMenu = ((Button)sender).ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.IsEnabled = true;
                contextMenu.PlacementTarget = (sender as Button);
                contextMenu.IsOpen = true;
            }
        }
        #endregion

        #region Searching

        private List<Essay> _searchResult = new List<Essay>();
        private DispatcherTimer _debounceTimer;

        private void SearchEssayTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel all previously expected commands (if any)
            if (_debounceTimer != null)
            {
                _debounceTimer.Stop();
            }
            // create new timer
            _debounceTimer = new DispatcherTimer();
            _debounceTimer.Interval = TimeSpan.FromMilliseconds(500); // Wait 500ms after no more changes
            _debounceTimer.Tick += async (s, args) =>
            {
                _debounceTimer.Stop();
                if (sender is TextBox textBox)
                {
                    if (textBox.Text == String.Empty)
                    {
                        ListEssayListView.ItemsSource = _data;
                    }
                    else
                    {
                        _searchResult.Clear();
                        foreach (Essay essay in _data)
                        {
                            if (essay.Title.StartsWith(textBox.Text))
                            {
                                Console.WriteLine(essay.Title);
                                _searchResult.Add(essay);
                            }
                        }
                        ListEssayListView.ItemsSource = _searchResult;
                    }
                    RefreshPage();
                }
            };
            // start countdown after 500ms when nothing change
            _debounceTimer.Start();
        }

        #endregion
        
        
        private void SearchEssayButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            SearchEssayTextBox.Visibility = Visibility.Visible;
        }

        private void SearchEssayButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!SearchEssayTextBox.IsMouseOver)
                SearchEssayTextBox.Visibility = Visibility.Collapsed;
        }

        private void RefreshPage()
        {
            ListEssayListView.Items.Refresh();
        }


    }
}