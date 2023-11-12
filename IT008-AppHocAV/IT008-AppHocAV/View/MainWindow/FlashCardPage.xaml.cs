using System.Collections.Generic;
using System.Windows.Controls;
using IT008_AppHocAV;
namespace IT008_AppHocAV.View.MainWindow
{
    public partial class FlashCardPage : Page
    {
        List<string> data;
        public MakeFlashCard MakeFlashCard;
        public IT008_AppHocAV.MainWindow mainWindow;
        public FlashCardPage()
        {
            InitializeComponent();
            data= new List<string>() { "flashcard 1", "flashcarh 2" , "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3" };
            lvListFlash.ItemsSource = data;
            
        }

        public FlashCardPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            data= new List<string>() { "flashcard 1", "flashcarh 2", "flashcard 3",    };
            lvListFlash.ItemsSource = data;
            this.mainWindow = mainWindow;

        }


        private void AddDeskButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
             
        }
    }
}