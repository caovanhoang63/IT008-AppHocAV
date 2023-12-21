using System.Windows;
using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class HomePage : Page
    {
        private IT008_AppHocAV.MainWindow _mainWindow;
        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void NavigateToExamPage_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("Exam");
        }

        private void NavigateToWriting_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("CreateEssay");
        }

        private void NavigateToFlashCard_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("MakeFlashCard");
        }

        private void NavigateToSearchPage(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavToSearching_OnClick(sender,e);
        }

        private void NavigateToTranslate_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("Translate");
        }
    }
}