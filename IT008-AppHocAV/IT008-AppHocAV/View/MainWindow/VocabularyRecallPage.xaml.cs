using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IT008_AppHocAV.Models;
using DbConnection = IT008_AppHocAV.Repositories.DbConnection.DbConnection;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class VocabularyRecallPage : Page
    {
        private IT008_AppHocAV.MainWindow _mainWindow;
        private DbConnection _dbConnection;
        private bool _isAlready = false;
        public List<VocabularyRecallLog> _data { get; set; }
        public List<VocabularyRecallLog> _dataMoc { get; set; }
        public VocabularyRecallPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            
            _mainWindow = mainWindow;
            _dbConnection = mainWindow.DbConnection;
            _dataMoc = new List<VocabularyRecallLog>();
            _data = _dbConnection.RecallRepository.GetAllRecallLogsByDateAndUserId(_mainWindow.UserId, DateTime.Now);
            
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
                if (_isAlready && RecallDate.SelectedDate != null)
                {
                    _data = _dbConnection.RecallRepository.GetAllRecallLogsByDateAndUserId(_mainWindow.UserId,RecallDate.SelectedDate.Value);
                    DataGrid.ItemsSource = _data;
                    DataGrid.Items.Refresh();
                    _isAlready = true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            
        }
    }
}