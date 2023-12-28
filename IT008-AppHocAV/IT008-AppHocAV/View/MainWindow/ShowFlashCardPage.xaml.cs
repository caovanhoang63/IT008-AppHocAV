using IT008_AppHocAV.Models;
using System;
using System.Collections.Generic;
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

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for ShowFlashCardPage.xaml
    /// </summary>
    public partial class ShowFlashCardPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private ListFlashCard _data;
        private FlashCardPage FlashCardPage;

        int IndexOfCurrentCard = 0;
        


        public ShowFlashCardPage()
        {
            this.InitializeComponent();
        }

        public ShowFlashCardPage(IT008_AppHocAV.MainWindow mainWindow, FlashCardPage flashCardPage)
        {
            InitializeComponent();

            this._mainWindow = mainWindow;
            this.FlashCardPage = flashCardPage;
            this._data= FlashCardPage.CurrentCard;
            _data.FlashCards = _mainWindow.DbConnection.CardQ.SelectCardByID(_data.Id);
            CurrentCard.Content = (IndexOfCurrentCard + 1).ToString();

            
            CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];
            this.DataContext= _data;
            
            
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        { 
            if(IndexOfCurrentCard < _data.Quantity-1)
            {
                IndexOfCurrentCard++;
                CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];

                CurrentCard.Content = (IndexOfCurrentCard + 1).ToString();

            }


        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        { 
            if(IndexOfCurrentCard > 0)
            {
                IndexOfCurrentCard--;
                CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];

                CurrentCard.Content = (IndexOfCurrentCard+1).ToString();

            }    

        }

        private void RefeshButton_Click(object sender, RoutedEventArgs e)
        {
            IndexOfCurrentCard =0;
            CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];
            CurrentCard.Content = (IndexOfCurrentCard+1).ToString();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to continue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.NavigateToPage("FlashCard");
                _mainWindow.PageCache.Remove("ShowFlashCard");
            }
        }
    }
}
