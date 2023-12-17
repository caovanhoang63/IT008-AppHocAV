using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using IT008_AppHocAV.Util;
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
    /// Interaction logic for EditFlashCard.xaml
    /// </summary>
    public partial class EditFlashCard : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private readonly FlashCardPage _listFlashCardPage;
        private readonly ListFlashCard _desk;
         
        private FlashCard _card;
        private List<FlashCard> _data;
        public EditFlashCard(IT008_AppHocAV.MainWindow mainWindow, FlashCardPage flashCardPage)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            this._listFlashCardPage = flashCardPage;
            
            _desk = flashCardPage.CurrentCard;
            this.DataContext = _desk;
        }
       

      

        public ListFlashCard  Card => _desk;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Text =_desk.Title;
            DescriptionTextBox.Text =_desk.Description;
            TextBlockDescription.Visibility = Visibility.Hidden;
            TextBlockTitle.Visibility = Visibility.Hidden;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to continue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.NavigateToPage("FlashCard");
                _mainWindow.PageCache.Remove("EditFlashCard");
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        { 
            _mainWindow.DbConnection.DeskQ.UpdateDeskContent(_desk.Id,_desk.Quantity,TitleTextBox.Text,DescriptionTextBox.Text);
            MessageBoxResult result = MessageBox.Show("Submit successes! Do you want to back to List FlashCard?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.PageCache.Remove("FlashCard");
                _mainWindow.PageCache.Remove("EditFlashCard");
                _mainWindow.NavigateToPage("FlashCard");
            }
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LvListCard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TermBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DefineBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
