using IT008_AppHocAV.Models;
using IT008_AppHocAV.View.CustomMessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        int flag = 0;
        int IndexOfCurrentCard = 0;
        List<int> MergeList = new List<int>();
        


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
            _data.FlashCards = _mainWindow.DbConnection.CardRepository.SelectCardByID(_data.Id);
            CurrentCard.Content = (IndexOfCurrentCard + 1).ToString();

            
            CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];
            this.DataContext= _data;
            
            
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                if (IndexOfCurrentCard < _data.Quantity-1)
                {
                    IndexOfCurrentCard++;
                    CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];

                    CurrentCard.Content = (IndexOfCurrentCard + 1).ToString();

                }
                ToggleButton flipCardToggleButton = Flip_Card;

                if (flipCardToggleButton.IsChecked==true)
                {
                    Flip_Card.IsChecked = false;
                }
            }
            else
            {

                if (IndexOfCurrentCard < _data.Quantity-1)
                {
                    IndexOfCurrentCard++;
                    
                    CardViewport.DataContext = _data.FlashCards[MergeList[IndexOfCurrentCard]];

                    CurrentCard.Content = (MergeList[IndexOfCurrentCard]+ 1).ToString();

                }
                ToggleButton flipCardToggleButton = Flip_Card;

                if (flipCardToggleButton.IsChecked==true)
                {
                    Flip_Card.IsChecked = false;
                }







            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        { 
            if(flag == 0)
            {
                if (IndexOfCurrentCard > 0)
                {
                    IndexOfCurrentCard--;
                    CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];

                    CurrentCard.Content = (IndexOfCurrentCard+1).ToString();

                }
                ToggleButton flipCardToggleButton = Flip_Card;

                if (flipCardToggleButton.IsChecked==true)
                {
                    Flip_Card.IsChecked = false;
                }
            }
            else
            {
                if (IndexOfCurrentCard>0)
                {
                    IndexOfCurrentCard--;

                    CardViewport.DataContext = _data.FlashCards[MergeList[IndexOfCurrentCard]];

                    CurrentCard.Content = (MergeList[IndexOfCurrentCard]+ 1).ToString();

                }
                ToggleButton flipCardToggleButton = Flip_Card;

                if (flipCardToggleButton.IsChecked==true)
                {
                    Flip_Card.IsChecked = false;
                }
            } 
                
            
        }

        private void RefeshButton_Click(object sender, RoutedEventArgs e)
        {
            IndexOfCurrentCard =0;
            CardViewport.DataContext = _data.FlashCards[IndexOfCurrentCard];
            CurrentCard.Content = (IndexOfCurrentCard+1).ToString();
            ToggleButton flipCardToggleButton = Flip_Card;

            if (flipCardToggleButton.IsChecked==true)
            {
                Flip_Card.IsChecked = false;
            }
            MergeList = ShuffleArray(_data.Quantity);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CTMessageBox.Show("Message", "Are you sure want to continue?", MessageBoxType.ConfirmationWithYesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.NavigateToPage("FlashCard");
                _mainWindow.PageCache.Remove("ShowFlashCard");
            }
        }
        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            if(flag==0)
            {
                Shuffle.Source = new BitmapImage(new Uri("/Assets/Icon/ArrowsIcon.png", UriKind.Relative));
                flag=1;
            }    
            else
            {
                Shuffle.Source = new BitmapImage(new Uri("/Assets/Icon/ShuffleIcon.png", UriKind.Relative));
                flag=0;
            }

            MergeList = ShuffleArray(_data.Quantity);
            IndexOfCurrentCard=0;

        }

       
       public List<int> ShuffleArray(int quantity)
        {
            List<int> TempList = new List<int>();
            //
            for (int i = 0; i < quantity; i++)
            {
                TempList.Add(i);
            }

            
            Random random = new Random();
            int n = TempList.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                // Hoán đổi giá trị tại vị trí i và j
                int temp = TempList[i];
                TempList[i] = TempList[j];
                TempList[j] = temp;
            }
            
            return TempList;
        }

       
    }
}
