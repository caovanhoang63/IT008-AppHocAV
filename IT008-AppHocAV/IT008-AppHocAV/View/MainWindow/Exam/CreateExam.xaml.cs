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
    /// Interaction logic for CreateExam.xaml
    /// </summary>
    public partial class CreateExam : Page
    {
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        public CreateExam()
        {
            InitializeComponent();
        }
        public CreateExam(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
        }
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure want to leave?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _mainWindow.NavigateToPage("ShowListExam");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.PageCache.Remove("Exam");
            _mainWindow.NavigateToPage("Exam");
        }
        private void LevelCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = 0;
            switch ((LevelCombobox.SelectedItem as ComboBoxItem)?.Content.ToString())
            {
                case "Easy":
                    value = 1;
                    break;
                case "Advanced":
                    value = 2;
                    break;
            }

            _mainWindow.SetLevelValue(value);
            if (LevelCombobox.SelectedItem != null && Category.SelectedItem != null)
            {
                StartExam.IsEnabled = true;
            }
            else
            {
                StartExam.IsEnabled = false;
            }
        }

        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainWindow.SetCategoryValue((Category.SelectedItem as ComboBoxItem)?.Content.ToString());

            if (LevelCombobox.SelectedItem != null && Category.SelectedItem != null)
            {
                StartExam.IsEnabled = true;
            }
            else
            {
                StartExam.IsEnabled = false;
            }


        }

    }
}
