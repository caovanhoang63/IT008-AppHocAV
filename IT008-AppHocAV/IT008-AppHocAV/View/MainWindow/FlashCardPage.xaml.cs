using System.Collections.Generic;
using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class FlashCardPage : Page
    {
        List<string> data;
        public FlashCardPage()
        {
            InitializeComponent();
            data= new List<string>() { "flashcard 1", "flashcarh 2" , "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3", "flashcard 3" };
            lvListFlash.ItemsSource = data;
            
        }
    }
}