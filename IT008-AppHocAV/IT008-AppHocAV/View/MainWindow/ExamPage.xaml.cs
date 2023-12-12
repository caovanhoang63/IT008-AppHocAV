using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class ExamPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        public ExamPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
        }
    }
}