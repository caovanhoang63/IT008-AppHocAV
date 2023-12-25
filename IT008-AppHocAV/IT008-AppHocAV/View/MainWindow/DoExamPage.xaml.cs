using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using IT008_AppHocAV.Models;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class DoExamPage : Page
    {
        #region Declare Field
        private List<Models.Question> _question;
        private List<int> Answer;
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private DispatcherTimer _timer;
        private int TimeRemaining;
        #endregion

        #region Constructor
        public DoExamPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;

            #region Timer
            TimeRemaining = 15 * 60;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            #endregion
            _question = new List<Question>();
            _question = _mainWindow.DbConnection.ExamQ.GetRandomQuestion();
            ListQuestionListView.ItemsSource = _question;

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining--;  // Decrement the time remaining
            }
            else
            {
                // Handle timer expiration
                ((DispatcherTimer)sender).Stop(); // Stop the timer
            }
        }
        #endregion
        private void Submit_btn_Click(object sender, RoutedEventArgs e)
        {
            Answer = new List<int>();
            foreach (var item in ListQuestionListView.Items)
            {
                var listViewItem = ListQuestionListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (listViewItem != null)
                {
                    CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);
                    if (checkBox.IsChecked == true)
                    {
                        string selectedCheckBoxName = checkBox.Name;
                        switch (selectedCheckBoxName)
                        {
                            case "A":
                                Answer.Add(1);
                                break;
                            case "B":
                                Answer.Add(2);
                                break;
                            case "C":
                                Answer.Add(3);
                                break;
                            case "D":
                                Answer.Add(4);
                                break;
                            default:
                                Answer.Add(-1);
                                break;
                        }
                    }
                    else
                    {
                        Answer.Add(0);
                    }
                }
            }
            string re = string.Join(", ", Answer); 
            MessageBox.Show(re);
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkedBox = sender as CheckBox;
            if (checkedBox != null)
            {
                // Tìm Grid chứa các CheckBox
                Grid questionGrid = checkedBox.Parent as Grid;
                if (questionGrid != null)
                {
                    // Bỏ chọn các CheckBox khác trong cùng một Grid
                    foreach (CheckBox otherBox in questionGrid.Children.OfType<CheckBox>())
                    {
                        if (otherBox != checkedBox)
                        {
                            otherBox.IsChecked = false;
                        }
                    }
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }


    }

}
