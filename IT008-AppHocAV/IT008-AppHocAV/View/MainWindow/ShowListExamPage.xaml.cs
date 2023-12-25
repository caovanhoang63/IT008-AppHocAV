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
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for ShowListExamPage.xaml
    /// </summary>
    public partial class ShowListExamPage : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private List<Models.Exam> _data;

        public ShowListExamPage(IT008_AppHocAV.MainWindow mainwindow)
        {
            InitializeComponent();
            this._mainWindow = mainwindow;
            _data = _mainWindow.DbConnection.ExamQ.SelectListExamByUserId(_mainWindow.UserId);
            ListExamListView.ItemsSource = _data;
        }
        private void NewExamButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToPage("CreateExam");
        }

        private void ShowExamButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
