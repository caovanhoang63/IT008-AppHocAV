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
    /// Interaction logic for ShowFlashCardPage.xaml
    /// </summary>
    public partial class ShowFlashCardPage : Page
    {
        private FlashCardPage flashCardPage;
        private readonly IT008_AppHocAV.MainWindow mainWindow;
        public ShowFlashCardPage()
        {
            this.InitializeComponent();
        }
        public ShowFlashCardPage( FlashCardPage flashcard)
        {
            InitializeComponent();
            this.flashCardPage = flashcard;
        }
        public ShowFlashCardPage(IT008_AppHocAV.MainWindow main)
        {
            InitializeComponent();
            this.mainWindow = main;
        }
        

    }
}
