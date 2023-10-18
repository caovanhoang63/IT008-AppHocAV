using System.Windows.Controls;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class Searching_page : Page
    {
        public Searching_page()
        {
            InitializeComponent();
        }

        public void ChangeLabelText(string text)
        {
            textLabel.Content = text;
        }
    }
}