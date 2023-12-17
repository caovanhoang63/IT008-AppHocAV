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

        private List<ListFlashCard> _data;
        private  ListFlashCard _currentCard;  // cái này đề làm gì chưa biết  : giờ thì biết rồi nó để trỏ đển cái thẻ mà mình đang chọn dữ liệu 
        public MakeFlashCard MakeFlashCard;
        public IT008_AppHocAV.MainWindow _mainWindow;
     

        public FlashCardPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            _data= _mainWindow.DbConnection.DeskQ.SelectListDeskByUserID(_mainWindow.UserId);
              
            _currentCard= null;
            lvListFlash.ItemsSource = _data;

         

        }
        public  ListFlashCard CurrentCard
        {
            get => _currentCard;    
        }
        private void NewFlashCard_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            _mainWindow.NavigateToPage("MakeFlashCard");
        }

        private void ItemFlashCard_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            // cái đoạn này là cho nút edit chứ k phải là của toàn bộ panel
            /* var modelCard=  (ListFlashCard)((FrameworkElement)sender).DataContext;
          _currentCard = _mainWindow.DbConnection.DeskQ.SelectDeskById(modelCard.Id);
          _mainWindow.NavigateToPage("EditFlashCard");*/
            _mainWindow.NavigateToPage("ShowFlashCard");
 
        }
        

       
        // chưa
        private void EditFlashcard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = true;
            var modelCard=  (ListFlashCard)((FrameworkElement)sender).DataContext;
           _currentCard = _mainWindow.DbConnection.DeskQ.SelectDeskById(modelCard.Id);
            _mainWindow.NavigateToPage("EditFlashCard");

        }


    
        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {

            e.Handled = true;
            MessageBoxResult result =
                    MessageBox.Show("You sure you want to delete this essay", null, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                ListFlashCard modelCard = (ListFlashCard)((FrameworkElement)sender).DataContext;
                if (_mainWindow.DbConnection.DeskQ.DeleteDeskById(modelCard.Id))
                {


                    foreach (ListFlashCard desk in _data)
                    {
                        if (desk.Id == modelCard.Id)
                        {
                            _data.Remove(desk);
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