﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IT008_AppHocAV.Components.RadioButtonMenuItem;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.View.CustomMessageBox;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for ShowListExamPage.xaml
    /// </summary>
    public partial class ShowListExamPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private List<Models.Exam> _data;

        public ShowListExamPage(IT008_AppHocAV.MainWindow mainwindow)
        {
            InitializeComponent();
            this._mainWindow = mainwindow;
            _data = _mainWindow.DbConnection.ExamRepository.SelectListExamByUserId(_mainWindow.UserId);
            ListExamListView.ItemsSource = _data;
        }
        private void NewExamButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("CreateExam");
        }

        private void ShowExamButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
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
                    _data.Sort(SortExamOptions.Factory(orderBy));
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
        #endregion
        private void RefreshPage()
        {
            ListExamListView.Items.Refresh();
        }

        private void DeleteExamButton_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
           
            MessageBoxResult result = CTMessageBox.Show("Message", "Are you sure want to delete this exam?", MessageBoxType.ConfirmationWithYesNo);

            if (result == MessageBoxResult.Yes)
            {
                Exam modelExam = (Models.Exam)((FrameworkElement)sender).DataContext;
                if (_mainWindow.DbConnection.ExamRepository.DeleteExamById(modelExam.Id))
                {

                    foreach (Exam exam in _data)
                    {
                        if (exam.Id == modelExam.Id)
                        {
                            _data.Remove(exam);
                            break;
                        }
                    }

                     CTMessageBox.Show("Message", "Deleted successfully ", MessageBoxType.Information);


                    RefreshPage();
                }
                else
                    CTMessageBox.Show("Message", "Delete fail ", MessageBoxType.Information);
            }

        }

        private void FilterExamButton_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = ((Button)sender).ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.IsEnabled = true;
                contextMenu.PlacementTarget = (sender as Button);
                contextMenu.IsOpen = true;
            }

        }
    }
}
