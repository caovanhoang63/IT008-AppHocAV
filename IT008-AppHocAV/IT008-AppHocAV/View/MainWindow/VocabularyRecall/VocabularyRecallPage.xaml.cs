using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using IT008_AppHocAV.Models;
using DbConnection = IT008_AppHocAV.Repositories.DbConnection.DbConnection;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class VocabularyRecallPage : Page
    {
        private IT008_AppHocAV.MainWindow _mainWindow;
        private DbConnection _dbConnection;
        private bool _isAlready = false;
        private DispatcherTimer _debounceTimer;
        public List<VocabularyRecallLog> _data { get; set; }
        public List<VocabularyRecallLog> _dataMoc { get; set; }
        private bool IsSeeAll = false;
        public VocabularyRecallPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            
            _mainWindow = mainWindow;
            _dbConnection = mainWindow.DbConnection;
            _dataMoc = new List<VocabularyRecallLog>();
            _data = _dbConnection.RecallRepository.FindAllRecallLogsByDateAndUserId(_mainWindow.UserId, DateTime.Now);
            
            DataGrid.ItemsSource = _data;
        }

        private void VocabularyRecallPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddNewRecallLog_OnClick(object sender, RoutedEventArgs e)
        {
            VocabularyRecallLog vocabularyRecallLog = new VocabularyRecallLog(_mainWindow.UserId);
            _data.Add(vocabularyRecallLog);
            DataGrid.Items.Refresh();
        }

        //fix scrolling issue when using a listview inside a scroll viewer
        private void instScroll_Loaded(object sender, RoutedEventArgs e)
        {
            ContentScrollViewer.AddHandler(MouseWheelEvent,new RoutedEventHandler(MyMouseWheelH),true);
        }
        private void MyMouseWheelH(object sender, RoutedEventArgs e)
        {
          
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;

            double x = (double)eargs.Delta;

            double y = ContentScrollViewer.VerticalOffset;

            ContentScrollViewer.ScrollToVerticalOffset(y - x);
        }

        private void RecallDate_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (RecallDate.SelectedDate != null)
                {
                    _data = _dbConnection.RecallRepository.FindAllRecallLogsByDateAndUserId(_mainWindow.UserId,RecallDate.SelectedDate.Value);
                    DataGrid.ItemsSource = _data;
                    DataGrid.Items.Refresh();
                    _isAlready = true;
                    IsSeeAll = false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            
        }

        private void RecallRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            
            if (textBox != null)
            {
                VocabularyRecallLog rowData = textBox.DataContext as VocabularyRecallLog;
                if (_debounceTimer != null)
                {
                    _debounceTimer.Stop();
                }
                // create new timer
                _debounceTimer = new DispatcherTimer();
                _debounceTimer.Interval = TimeSpan.FromMilliseconds(1000); // Wait 500ms after no more changes
                _debounceTimer.Tick += async (s, args) =>
                {
                    _debounceTimer.Stop();
                    _dbConnection.RecallRepository.UpdateRecallLog(rowData);
                };
                // start countdown after 500ms when nothing change
                _debounceTimer.Start();
            }
        }

        private void CheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            
            VocabularyRecallLog rowData = checkBox.DataContext as VocabularyRecallLog;
            if(rowData != null)
            {
                Debug.Assert(checkBox.IsChecked != null, "checkBox.IsChecked != null");
                rowData.IsSuccessful = checkBox.IsChecked.Value;
                _dbConnection.RecallRepository.UpdateRecallLog(rowData);
            }

        }

        private void SeeAllRecallLog_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _data = _dbConnection.RecallRepository.FindAllRecallLogsByDateAndUserId(_mainWindow.UserId);
                DataGrid.ItemsSource = _data;
                DataGrid.Items.Refresh();
                _isAlready = true;
                IsSeeAll = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void MakeNewFlashCards_OnClick(object sender, RoutedEventArgs e)
        {

            ListFlashCard listFlashCard = new ListFlashCard();

            listFlashCard.UserId= _mainWindow.UserId;
            
            // listFlashCard.Title= TitleTextBox.Text;
            // listFlashCard.Description= DescriptionTextBox.Text;
            
            listFlashCard.CreatedAt = DateTime.Now;
            listFlashCard.UpdatedAt = DateTime.Now;
            listFlashCard.Quantity = DataGrid.Items.Count;
            listFlashCard.FlashCards = new List<FlashCard>();
            MakeFlashCard makeFlashCard = new MakeFlashCard(_mainWindow);

            foreach (var item in _data)
            {
                FlashCard flashCard = new FlashCard();
                flashCard.Answer = item.Word;
                flashCard.Question = item.Meaning;
                makeFlashCard.Datatemp.Add(flashCard);
                makeFlashCard.Data.FlashCards.Add(flashCard);
            }

            if (IsSeeAll)
            {
                makeFlashCard.TitleTextBox.Text = "All recall logs";
            }
            else
            {
                if (RecallDate.SelectedDate != null)
                    makeFlashCard.TitleTextBox.Text = RecallDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                else 
                    return;
            }

            _mainWindow.PageCache.Remove("MakeFlashCard");
            
            
            _mainWindow.PageCache.Add("MakeFlashCard",makeFlashCard);
            
            _mainWindow.Content.Navigate(makeFlashCard);

        }
    }
}