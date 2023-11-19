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
            lvMakeFlashCard.ItemsSource = data;
        }
        public MakeFlashCard(FlashCardPage flashCardPage)
        {
            InitializeComponent();
            data = new List<string>() { "item1" , "item2" };
            this.flashCardPage = flashCardPage;
            lvMakeFlashCard.ItemsSource = data;

        }
        public MakeFlashCard(IT008_AppHocAV.MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to continue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                mainWindow.NavigateToPage("FlashCard");
                mainWindow.pageCache.Remove("Writing");
            }
        }
    }
}