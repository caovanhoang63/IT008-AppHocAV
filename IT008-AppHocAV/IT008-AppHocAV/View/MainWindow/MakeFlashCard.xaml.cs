using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class MakeFlashCard : Page
    {
        private FlashCardPage flashCardPage;
        private readonly IT008_AppHocAV.MainWindow mainWindow;
        public List<string> data;
        public MakeFlashCard()
        {
            InitializeComponent();
            data = new List<string>() { "item1", "item2" };
            LvListCard.ItemsSource = data;
        }
        public MakeFlashCard(FlashCardPage flashCardPage)
        {
            InitializeComponent();
            data = new List<string>() { "TERM" , "TERM" ,"TERM", "TERM" };
            this.flashCardPage = flashCardPage;
            LvListCard.ItemsSource = data;

        }
        public MakeFlashCard(IT008_AppHocAV.MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.mainWindow = mainWindow;
            data = new List<string>() { "TERM1", "TERM1", "TERM1", "TERM1", "TERM1" };
             
            LvListCard.ItemsSource = data;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to continue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                mainWindow.NavigateToPage("FlashCard");
                mainWindow.PageCache.Remove("Writing");
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NavigateToPage("FlashCard");
            mainWindow.PageCache.Remove("MakeFlashCard");
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

        private void DeleteCardButton_Click(object sender, RoutedEventArgs e)
        {

            data.RemoveAt(1);
            RefreshPage();
        }

        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            data.Add("TERM");
            RefreshPage();
        }
    }
}