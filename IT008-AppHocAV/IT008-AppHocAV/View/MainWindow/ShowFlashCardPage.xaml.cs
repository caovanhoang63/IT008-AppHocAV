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
        public class Lable
        {
            public Lable(int current, int total)
            {
                this.current=current;
                this.total=total;
            }

            public int current { get; set; }
            public int total { get; set; }
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
            
            //  LvListCard.ItemsSource = _data.FlashCards;
            if(_data.FlashCards != null)
               this.DataContext=_data.FlashCards[x];
            MessageBox.Show(_data.Id.ToString());

            Lable lable = new Lable(x,_data.Quantity);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (x<_data.Quantity-1)
            {
                x++;
                this.DataContext=_data.FlashCards[x];
            }
               
          
              

         
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (x>0)
            {
                x--;
                this.DataContext=_data.FlashCards[x];
            }

            
        }

        private void RefeshButton_Click(object sender, RoutedEventArgs e)
        {
            x=0;
            this.DataContext=_data.FlashCards[x];
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
