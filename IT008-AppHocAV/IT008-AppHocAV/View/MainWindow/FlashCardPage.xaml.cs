using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using IT008_AppHocAV;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class FlashCardPage : Page
    {
        List<string> data;
        public MakeFlashCard MakeFlashCard;
        public IT008_AppHocAV.MainWindow mainWindow;
        public FlashCardPage()
        {
            InitializeComponent();
            data= new List<string>() { "flashcard 1", "flashcarh 2", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3" };
            lvListFlash.ItemsSource = data;

        }

        public FlashCardPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            data= new List<string>() { "flashcard 1", "flashcarh 2", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", };
            lvListFlash.ItemsSource = data;
            this.mainWindow = mainWindow;

        }


        private void AddDeskButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mainWindow.NavigateToPage("MakeFlashCard");
        }

        private void ItemFlashCard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mainWindow.NavigateToPage("ShowFlashCard");
        }

        private void EditFlashcard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true;
            mainWindow.NavigateToPage("MakeFlashCard");// chua chạy đoạn này
        }
        private void DeleteFlashCard_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            // khúc này ép data vô rồi làm tiếp 

            e.Handled = true;
            MessageBoxResult result =
                    MessageBox.Show("You sure you want to delete this essay", null, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                data.Remove("flashcard 1");

            }
        }

        private void NewFlashCardButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private DispatcherTimer _debounceTimer;
        private void SearchFlashCardTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
                        //ListEssayListView.ItemsSource = _data;
                    }
                    else
                    {
                        /*_searchResult.Clear();
                        foreach (Essay essay in _data)
                        {
                            if (essay.Title.StartsWith(textBox.Text))
                            {
                                Console.WriteLine(essay.Title);
                                _searchResult.Add(essay);
                            }
                        }
                        ListEssayListView.ItemsSource = _searchResult;*/
                    }
                    RefreshPage();
                }
            };
            // start countdown after 500ms when nothing change
            _debounceTimer.Start();
        }

        

        private void RefreshPage()
        {
            lvListFlash.Items.Refresh();
        }

        private void SearchEssayButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SearchFlashCardTextBox.Visibility=  Visibility.Visible;
        }

        private void SearchEssayButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!SearchFlashCardTextBox.IsMouseOver)
                SearchFlashCardTextBox.Visibility=Visibility.Collapsed;
        }
    }
}