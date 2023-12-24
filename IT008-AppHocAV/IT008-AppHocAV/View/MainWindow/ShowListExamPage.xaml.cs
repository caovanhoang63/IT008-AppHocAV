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
    /// Interaction logic for ShowListExamPage.xaml
    /// </summary>
    public partial class ShowListExamPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainwindow;
        public ShowListExamPage(IT008_AppHocAV.MainWindow mainwindow)
        {
            InitializeComponent();
            this._mainwindow = mainwindow;
        }
        private void NewExamButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainwindow.NavigateToPage("CreateExam");
        }

        private void SearchEssayButton_OnMouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void SearchEssayButton_OnMouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
