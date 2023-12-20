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
        private ListFlashCard _data;

       // public List<FlashCard> _data;
      
        public MakeFlashCard(IT008_AppHocAV.MainWindow mainWindow)
        {

            this.InitializeComponent();
            this.mainWindow = mainWindow;

            _data = new ListFlashCard();
             
            LvListCard.ItemsSource = _data.FlashCards;

        }


        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            FlashCard card = new FlashCard();
            
             card.ImagePath = null;
            /*  card.Question ="";
             card.Answer ="";*/

            _data.FlashCards.Add(card);
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
                    //_card.ImagePath= fileName;
                    //_card.Image = image;
                    
                }
            }
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _data.UserId= mainWindow.UserId;
            _data.Title= TitleTextBox.Text;
            _data.Description= DescriptionTextBox.Text;
            _data.CreatedAt = DateTime.Now;
            _data.UpdatedAt = DateTime.Now;


            _data.Id = mainWindow.DbConnection.DeskQ.CreateDesk(_data);
            
            foreach(FlashCard item in LvListCard.Items)
            {
                item.Id = _data.Id;
                mainWindow.DbConnection.DeskQ.InsertCards(item);
            }    

            /*foreach( var item in LvListCard.Items )
            {
                _card.DeskId = _data.Id;
                _card.UpdatedAt = DateTime.Now;

                TextBox term = (TextBox)FindName("TermBox");
                TextBox define = (TextBox)FindName("DefineBox");
                _card.Question =term.Text;
                _card.Answer = define.Text;
                _card.Id = mainWindow.DbConnection.CardQ.CreateCard(_card);
            }   */ 
            if(_data.Id==0)
            {
                MessageBox.Show("Fail to create new essay!");
                return;
            }
            MessageBox.Show("Successed!");
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
            else {
                termBlock.Visibility = Visibility.Visible; 
            }




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