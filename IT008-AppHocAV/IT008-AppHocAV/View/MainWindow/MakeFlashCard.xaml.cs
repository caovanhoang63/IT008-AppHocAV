using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
 
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class MakeFlashCard : Page
    {

        private readonly IT008_AppHocAV.MainWindow mainWindow;
        //private FlashCardPage flashCardPage;
        private ListFlashCard _data;

        // public List<FlashCard> _data;
        private ObservableCollection<FlashCard> _datatemp;
        public MakeFlashCard(IT008_AppHocAV.MainWindow mainWindow)
        {

            this.InitializeComponent();
            this.mainWindow = mainWindow;

            _data = new ListFlashCard();

            _datatemp = new ObservableCollection<FlashCard>();
            
            LvListCard.ItemsSource = _datatemp;

        }


        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            // set selectedindex để tránh nhầm card
            LvListCard.SelectedIndex =-1;
            FlashCard card = new FlashCard();
            
            card.ImagePath = null;

            _data.FlashCards.Add(card);

            _datatemp.Add(card);
            // RefreshPage();
          
          
           
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
                                var setImage = FindVisualChild<Image>(item, "SetImage");
                                if(setImage != null)
                                {
                                    setImage.Visibility = Visibility.Hidden;
                                }    
                            }
                        }
                    }

                }
                 
            }
        }
     
                    
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (LvListCard.Items.Count ==0)
            {
                MessageBox.Show("Please add card!");
            }
            else
            {

                _data.UserId= mainWindow.UserId;
                _data.Title= TitleTextBox.Text;
                _data.Description= DescriptionTextBox.Text;
                _data.CreatedAt = DateTime.Now;
                _data.UpdatedAt = DateTime.Now;
                _data.Quantity = LvListCard.Items.Count;



                _data.Id = mainWindow.DbConnection.DeskQ.CreateDesk(_data);

                foreach (FlashCard item in LvListCard.Items)
                {
                    item.Id = _data.Id;
                    mainWindow.DbConnection.DeskQ.InsertCards(item);
                }
                if (_data.Id==0)
                {
                    MessageBox.Show("Fail to create new essay!");
                    return;
                }
                MessageBox.Show("Successed!");

                mainWindow.PageCache.Remove("FlashCard");
                mainWindow.PageCache.Remove("MakeFlashCard");
                mainWindow.NavigateToPage("FlashCard");

            }

 
        }


        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            int index = -1;
            index = LvListCard.SelectedIndex;
            if( index != -1)
            {
                 
                _data.FlashCards.RemoveAt(index);
                _datatemp.RemoveAt(index);
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
                    if (defineBlock != null)
                    {
                        defineBlock.Visibility = Visibility.Hidden;
                    }
                }
            }

        }

        private T FindVisualChild<T>(DependencyObject depObj, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T && ((FrameworkElement)child).Name == name)
                {
                    return (T)child;
                }
                else
                {
                    T childItem = FindVisualChild<T>(child, name);
                    if (childItem != null)
                        return childItem;
                }
            }
            return null;
        }
        // Import File
        private void ImportFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The request is to format a file.txt with the following pattern:(Term + ' : '+ Definition).", "Attention");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "File|*.txt;...";
            if (openFileDialog.ShowDialog() != true)
                return;

            if (!openFileDialog.CheckPathExists)
                return;

            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
            {
                string line;
                while ((line =  streamReader.ReadLine()) != null)
                {
                    string[] text = line.Trim().Split(':');
                    if(text.Length !=2)
                    {
                        MessageBox.Show(" Invalid format pattern! ");
                        return;
                    }   
                    
                    FlashCard card = new FlashCard(text[0], text[1]);

                    _data.FlashCards.Add(card);
                    _datatemp.Add(card);
                }
                
               
            }


            LvListCard.SelectedIndex =-1;
            
            
        }
        // Scroll in ListView
        private void LvListCard_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
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