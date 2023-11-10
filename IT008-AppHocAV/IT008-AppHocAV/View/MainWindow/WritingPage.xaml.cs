using System.Windows;
using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class WritingPage : Page
    {
        private IT008_AppHocAV.MainWindow _mainWindow; 
        public WritingPage()
        {
            InitializeComponent();
        }
        
        public WritingPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
        }


        private void StartWritingButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("WritingContentPage");
        }
    }
}