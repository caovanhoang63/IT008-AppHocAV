using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class MakeFlashCard : Page
    {
       
        private readonly IT008_AppHocAV.MainWindow mainWindow;
        //private FlashCardPage flashCardPage;
        private ListFlashCard _listflashcard;
        private FlashCard _card;
        private List<FlashCard> _data;

       // public List<FlashCard> _data;
      
        public MakeFlashCard(IT008_AppHocAV.MainWindow mainWindow)
        {

            this.InitializeComponent();
            this.mainWindow = mainWindow;
            _card = new FlashCard();
            _card.ImagePath= null;
            _listflashcard = new ListFlashCard();  
            _data = new List<FlashCard>();
             
            LvListCard.ItemsSource = _data;
             

        }
        public FlashCard Card
        {
            get => _card;
        }
        public ListFlashCard ListFlashcards
        {
            get => _listflashcard;
        }
        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            FlashCard card = new FlashCard();
            TextBox term = FindVisualChild<TextBox>(LvListCard, "TermBox");
            TextBox define = FindVisualChild<TextBox>(LvListCard, "DefineBox");
            card.ImagePath = null;
          
           
            card.QuestionId =term.Text;
            card.AnswerId = define.Text;
            
            _data.Add(_card);



             RefreshPage();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to continue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                mainWindow.NavigateToPage("FlashCard");
                mainWindow.PageCache.Remove("MakeFlashCard");
            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png;...";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                if (fileName != String.Empty)
                {
                    Image cardImage = (Image)FindName("CardImage");
                    BitmapImage image = new BitmapImage(new Uri(fileName));
                    cardImage.Source = image;
                    cardImage.Visibility = Visibility.Visible;
                    _card.ImagePath= fileName;
                    _card.Image = image;
                    
                }
            }
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _listflashcard.UserId= mainWindow.UserId;
            _listflashcard.Title= TitleTextBox.Text;
            _listflashcard.Description= DescriptionTextBox.Text;
            _listflashcard.CreatedAt = DateTime.Now;
            _listflashcard.UpdatedAt = DateTime.Now;
            _listflashcard.Id = mainWindow.DbConnection.DeskQ.CreateDesk(_listflashcard);
            /*foreach( var item in LvListCard.Items )
            {
                _card.DeskId = _listflashcard.Id;
                _card.UpdatedAt = DateTime.Now;

                TextBox term = (TextBox)FindName("TermBox");
                TextBox define = (TextBox)FindName("DefineBox");
                _card.QuestionId =term.Text;
                _card.AnswerId = define.Text;
                _card.Id = mainWindow.DbConnection.CardQ.CreateCard(_card);
            }   */ 
            if(_listflashcard.Id==0)
            {
                MessageBox.Show("Fail to create new essay!");
                return;
            }
            /*  if(_card.Id==0)
              {
                  MessageBox.Show("Add card !");
                  return;
              }    */
            mainWindow.PageCache.Remove("FlashCard");
            mainWindow.PageCache.Remove("MakeFlashCard");
            mainWindow.NavigateToPage("FlashCard");
           



            /*  _essay.UserId = _mainWindow.UserId;
              _essay.Title = TitleTextBox.Text;
              _essay.Topic = TopicTextBox.Text;
              _essay.Description = DescriptionTextBox.Text;
              _essay.CreatedAt =  DateTime.Now;
              _essay.UpdatedAt =  DateTime.Now;

              _essay.Id =  _mainWindow.DbConnection.EssayQ.CreateEssay(_essay);
              if (_essay.Id == 0)
              {
                  MessageBox.Show("Fail to create new essay!");
                  return;
              }
              _mainWindow.NavigateToPage("WritingContent");
              _mainWindow.PageCache.Remove("Writing");*/
        }


        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {

            //data.RemoveAt(1);
            RefreshPage();
        }

       


        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


            if (TitleTextBox.Text != string.Empty)
            {
                TextBlockTitle.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBlockTitle.Visibility = Visibility.Visible;
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionTextBox.Text != string.Empty)
            {
                TextBlockDescription.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBlockDescription.Visibility = Visibility.Visible;
            }
        }

      

        private void TermBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string termContent = textBox.Text;
            TextBlock termBlock = (TextBlock)textBox.FindName("TermBlock");

            if (termContent != string.Empty)
            {
                termBlock.Visibility = Visibility.Hidden;
            }
            else { termBlock.Visibility = Visibility.Visible; }
        }

        private void DefineBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string defineContent = textBox.Text;
            TextBlock termBlock = (TextBlock)textBox.FindName("DefineBlock");

            if (defineContent != string.Empty)
            {
                termBlock.Visibility = Visibility.Hidden;
            }
            else { termBlock.Visibility = Visibility.Visible; }

        }

        private void RefreshPage()
        {
            LvListCard.Items.Refresh();

             
        }
        private T FindVisualChild<T>(DependencyObject depObj, string childName) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T && (child as FrameworkElement).Name == childName)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child, childName);

                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        private void LvListCard_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}