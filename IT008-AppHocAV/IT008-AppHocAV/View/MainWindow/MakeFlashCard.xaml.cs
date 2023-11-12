using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class MakeFlashCard : Page
    {
        private FlashCardPage flashCardPage;
        public MakeFlashCard()
        {
            InitializeComponent();
        }
        public MakeFlashCard(FlashCardPage flashCardPage)
        {
            InitializeComponent();
            this.flashCardPage = flashCardPage;
        }

    }
}