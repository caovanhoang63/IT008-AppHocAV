using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using IT008_AppHocAV.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for EditFlashCard.xaml
    /// </summary>
    public partial class EditFlashCard : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private ListFlashCard _data;
        private FlashCardPage FlashCardPage;
        private ObservableCollection<FlashCard> _datatemp;
        public EditFlashCard(IT008_AppHocAV.MainWindow mainWindow, FlashCardPage flashCardPage)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            this.FlashCardPage = flashCardPage;
            this._data= FlashCardPage.CurrentCard;
            _datatemp = new ObservableCollection<FlashCard>();

            _data.FlashCards = _mainWindow.DbConnection.CardRepository.SelectCardByID(_data.Id);

            // Chuyển đổi List<FlashCard> thành ObservableCollection<FlashCard>
            ObservableCollection<FlashCard> flashCardCollection = new ObservableCollection<FlashCard>(_mainWindow.DbConnection.CardRepository.SelectCardByID(_data.Id));

            // Gán vào _datatemp
            _datatemp = flashCardCollection;

            LvListCard.ItemsSource = _datatemp;
        }
       

      
 
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Text =_data.Title;
            DescriptionTextBox.Text =_data.Description;

          
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
        {   //update desk
            _mainWindow.DbConnection.DeskRepository.UpdateDeskContent(_data.Id,_data.Quantity,TitleTextBox.Text,DescriptionTextBox.Text);
             // update card
            foreach( FlashCard item in LvListCard.ItemsSource )
            {
                
                _mainWindow.DbConnection.CardRepository.UpdateCardContent(item.Id, item.Question, item.Answer,item.Image);
            }    
            MessageBoxResult result = MessageBox.Show("Submit successes! Do you want to back to List FlashCard?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.PageCache.Remove("FlashCard");
                _mainWindow.PageCache.Remove("EditFlashCard");
                _mainWindow.NavigateToPage("FlashCard");
            }
        }
        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            // set selectedindex để tránh nhầm card
            LvListCard.SelectedIndex =-1;
            FlashCard card = new FlashCard();

            card.ImagePath = null;
            _data.Quantity++;
            card.DeskId = _data.Id;
            card.Id = _mainWindow.DbConnection.CardRepository.SelectTopId() +1;

            if (_mainWindow.DbConnection.CardRepository.CreateCard(card))
            {
                _data.FlashCards.Add(card);
                _datatemp.Add(card);
            }
            else
                MessageBox.Show("Fail!");
          
           // RefreshPage();

        }
        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            int index = -1;
            index = LvListCard.SelectedIndex;
            
            if (index != -1)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Files|*.jpg;*.jpeg;*.png;...";
                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    if (fileName != String.Empty)
                    {

                        {

                            _data.FlashCards[index].ImagePath = fileName;
                            BitmapImage image = new BitmapImage(new Uri(fileName));
                            _data.FlashCards[index].Image = image;
                            _datatemp[index].ImagePath = fileName;
                            _datatemp[index].Image = image;

                            // Hiển thị hình ảnh ra 
                            DependencyObject dep = (DependencyObject)e.OriginalSource;
                            while ((dep != null) && !(dep is ListViewItem))
                            {
                                dep = VisualTreeHelper.GetParent(dep);
                            }

                            if (dep is ListViewItem item)
                            {
                                // Tìm Image trong ListViewItem và đặt thuộc tính Visibility
                                
                                Image cardImage = FindVisualChild<Image>(item, "CardImage");
                                if (cardImage != null)
                                {
                                    cardImage.Source=image;
                                    cardImage.Visibility = Visibility.Visible;
                                }
                             
                            }
                        }

                    }
                }
               
            }
        }
        private void DeleteImageButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            int index = -1;
            index = LvListCard.SelectedIndex;
           
            if (index != -1)
            {
               
                _data.FlashCards[index].ImagePath = null;
                //  BitmapImage image = new BitmapImage(new Uri(fileName));
                _data.FlashCards[index].Image = null;
                _datatemp[index].ImagePath = null;
                _datatemp[index].Image = null;
                DependencyObject dep = (DependencyObject)e.OriginalSource;
                while ((dep != null) && !(dep is ListViewItem))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep is ListViewItem item)
                {
                    // Tìm Image trong ListViewItem và đặt thuộc tính Visibility

                    var cardImage = FindVisualChild<Image>(item, "CardImage");
                    if (cardImage != null)
                    {
                        // cardImage.Source=image;
                        cardImage.Visibility = Visibility.Hidden;
                    }
                    var Button = FindVisualChild<Button>(item, "DeleteImageButton");
                    if (Button != null)
                    {
                        Button.Visibility = Visibility.Hidden;
                    }
                }

           
         
                    


            }
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

        private void LvListCard_Loaded(object sender, RoutedEventArgs e)
        {
            ListView listView = (ListView)sender;

            foreach (var item in listView.Items)
            {
                ListViewItem listViewItem = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

                if (listViewItem != null)
                {
                    TextBlock termBlock = FindVisualChild<TextBlock>(listViewItem, "TermBlock");
                    TextBlock defineBlock = FindVisualChild<TextBlock>(listViewItem, "DefineBlock");
                    if (termBlock != null)
                    {
                        termBlock.Visibility = Visibility.Hidden;
                    }
                    if(defineBlock != null)
                    {
                        defineBlock.Visibility = Visibility.Hidden;
                    }    
                }
            }
        }

        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            FlashCard modelCard = (FlashCard)((FrameworkElement)sender).DataContext;


            if (_mainWindow.DbConnection.CardRepository.DeleteCardById(modelCard.Id))
            {


                foreach (FlashCard card in _datatemp)
                {
                    if (card.Id == modelCard.Id)
                    {
                        _data.FlashCards.Remove(card);
                        _datatemp.Remove(card);
                        break;
                    }
                }
             
            }
            else
                MessageBox.Show("Delete fail! ");

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
            else
            {
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

        private void LvListCard_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
                return;

            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };
            LvListCard.RaiseEvent(eventArg);
        }

        
    }
}
