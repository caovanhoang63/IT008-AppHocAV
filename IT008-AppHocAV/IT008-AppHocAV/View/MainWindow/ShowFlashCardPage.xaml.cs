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


        int x = 0;
        public class Data
        {
            public Data(string quest, string answ, int current, int total, BitmapImage image)
            {
                this.current=current;
                this.total=total;
                this.quest=quest;
                this.answ=answ;
            }
            public Data()
            {
                this.total=0;
                this.current=0;
                this.quest=null;
                this.answ=null;
                image=null;
            }

            public int current;
            public int total;
            public string quest;
            public string answ;
            public BitmapImage image;
            public int Total
            {
                get => total;
                set => total= value;
            }
            public int Current
            {
                get => current;
                set => current = value;
            }
            public string Question
            {
                get => quest;
                set => quest=value;
            }
            public string Answer
            {
                get => answ;
                set => answ = value;
            }
            public BitmapImage Image
            {
                get => image;
                set => image = value;
            }

        }


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
            Data datatemp = new Data();
            //  LvListCard.ItemsSource = _data.FlashCards;
            if (_data.FlashCards != null)
            {
                datatemp = new Data(_data.FlashCards[0].Question, _data.FlashCards[0].Answer, x+1, _data.Quantity, _data.FlashCards[0].Image);

            }
            this.DataContext=datatemp;



        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (x<_data.Quantity-1)
            {
                x++;

                Data datatemp1 = new Data(_data.FlashCards[x].Question, _data.FlashCards[x].Answer, x+1, _data.Quantity, _data.FlashCards[x].Image);
               
                this.DataContext=datatemp1;
            }





        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (x>0)
            {
                x--;
                Data datatemp1 = new Data(_data.FlashCards[x].Question, _data.FlashCards[x].Answer, x+1, _data.Quantity, _data.FlashCards[x].Image);

                this.DataContext=datatemp1;


            }
        } 

        private void RefeshButton_Click(object sender, RoutedEventArgs e)
        {
            x=0;
            Data datatemp1 = new Data(_data.FlashCards[x].Question, _data.FlashCards[x].Answer, x+1, _data.Quantity, _data.FlashCards[x].Image);

            this.DataContext=datatemp1;
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
