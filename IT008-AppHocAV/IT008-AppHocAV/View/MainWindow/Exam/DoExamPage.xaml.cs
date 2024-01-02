using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.View.CustomMessageBox;

namespace IT008_AppHocAV.View.MainWindow
{
    /// <summary>
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class DoExamPage : Page
    {
        #region Declare Field
        private List<Models.Question> _question;
        private List<String> Answer;
        List<string> Result;
        private readonly IT008_AppHocAV.MainWindow _mainWindow;
        private int _remainingTimeInSeconds;
        private int _timetaken;
        private int level;
        private string category;
        private DispatcherTimer _countdownTimer;
        #endregion

        #region Constructor
        public DoExamPage(IT008_AppHocAV.MainWindow mainWindow, int lvl, string cate)
        {
            InitializeComponent();
            this._mainWindow = mainWindow;
            _timetaken = 0;
            level = lvl;
            category = cate;

            _question = new List<Question>();
            _question = _mainWindow.DbConnection.ExamRepository.GetRandomQuestion();
            Result = GetCorrectAnswer(_question);
            ListQuestionListView.ItemsSource = _question;
            InitializeCountdownTimer();

        }
        #endregion
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

        private void Submit_btn_Click(object sender, RoutedEventArgs e)
        {
            _countdownTimer.Stop();
            MessageBoxResult Submit = CTMessageBox.Show("Are you sure want to submit?", "", MessageBoxButton.YesNo);
            
            if (Submit == MessageBoxResult.Yes)
            {
                Answer = new List<String>();
                int index = 0;
                foreach (var item in ListQuestionListView.Items)
                {
                    bool flag = false;
                    var listViewItem = ListQuestionListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    if (listViewItem != null)
                    {
                        List<CheckBox> checkBoxes = new List<CheckBox>();
                        foreach (string c in new string[] { "A", "B", "C", "D" })
                        {
                            checkBoxes.Add(FindVisualChild<CheckBox>(listViewItem, c));
                        }
                        foreach (CheckBox checkBox in checkBoxes)
                        {
                            if (checkBox.IsChecked == true)
                            {
                                string selectedCheckBoxName = checkBox.Name;
                                switch (selectedCheckBoxName)
                                {
                                    case "A":
                                        Answer.Add("A");
                                        flag = true;
                                        break;
                                    case "B":
                                        Answer.Add("B");
                                        flag = true;
                                        break;
                                    case "C":
                                        Answer.Add("C");
                                        flag = true;
                                        break;
                                    case "D":
                                        Answer.Add("D");
                                        flag = true;
                                        break;
                                }
                            }
                        }
                        if (flag == false)
                        {
                            Answer.Add("None");
                        }
                    }
                    index++;
                }
                int score = Scoring_function(Answer, Result);
                
                Exam exam = new Exam(_mainWindow.UserId, level, category, score, _timetaken);
                CTMessageBox.Show("Message", "Your score is " + score.ToString(), MessageBoxType.Information);

                _mainWindow.DbConnection.ExamRepository.SaveResult(exam);
                _mainWindow.PageCache.Remove("ShowListExam");
                _mainWindow.PageCache.Remove("DoExamPage");
                _mainWindow.NavigateToPage("ShowListExam");
            }
        }

        #region Support function
        public static T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T && ((FrameworkElement)child).Name == childName)
                {
                    foundChild = (T)child;
                    break;
                }
                else
                {
                    foundChild = FindVisualChild<T>(child, childName);
                    if (foundChild != null)
                    {
                        break;
                    }
                }
            }
            return foundChild;
        }
        private List<string> GetCorrectAnswer(List<Models.Question> questions)
        {
            List<string> correct = new List<string>();
            foreach (Question question in questions)
            {
                correct.Add(question.Correct);
            }
            return correct;
        }
        private int Scoring_function(List<string> answer, List<string> result)
        {
            int score = 0;
            for (int i = 0; i < Math.Min(answer.Count, result.Count); i++)
            {
                if (answer[i] == result[i])
                {
                    score++;
                }
            }
            return score;
        }
        #endregion
        private void InitializeCountdownTimer()
        {
            _remainingTimeInSeconds = 15*60;
            _countdownTimer = new DispatcherTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_remainingTimeInSeconds > 0)
            {
                _remainingTimeInSeconds--;
                _timetaken++;
                UpdateCountdownDisplay();
            }
            else
            {
                _remainingTimeInSeconds = 15 * 60;
                _countdownTimer.Stop();
                Answer = new List<String>();
                int index = 0;
                foreach (var item in ListQuestionListView.Items)
                {
                    bool flag = false;
                    var listViewItem = ListQuestionListView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    if (listViewItem != null)
                    {
                        List<CheckBox> checkBoxes = new List<CheckBox>();
                        foreach (string c in new string[] { "A", "B", "C", "D" })
                        {
                            checkBoxes.Add(FindVisualChild<CheckBox>(listViewItem, c));
                        }
                        foreach (CheckBox checkBox in checkBoxes)
                        {
                            if (checkBox.IsChecked == true)
                            {
                                string selectedCheckBoxName = checkBox.Name;
                                switch (selectedCheckBoxName)
                                {
                                    case "A":
                                        Answer.Add("A");
                                        flag = true;
                                        break;
                                    case "B":
                                        Answer.Add("B");
                                        flag = true;
                                        break;
                                    case "C":
                                        Answer.Add("C");
                                        flag = true;
                                        break;
                                    case "D":
                                        Answer.Add("D");
                                        flag = true;
                                        break;
                                }
                            }
                        }
                        if (flag == false)
                        {
                            Answer.Add("None");
                        }
                    }
                    index++;
                }
                int score = Scoring_function(Answer, Result);
                CTMessageBox.Show("Message", "Your score is " + score.ToString(), MessageBoxType.Information);

                Exam exam = new Exam(_mainWindow.UserId, level,category, score,_timetaken);

                _mainWindow.DbConnection.ExamRepository.SaveResult(exam);
                _mainWindow.PageCache.Remove("DoExamPage");
                _mainWindow.PageCache.Remove("ShowListExam");
                _mainWindow.NavigateToPage("ShowListExam");
            }
        }

        private void UpdateCountdownDisplay()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_remainingTimeInSeconds);
            string formattedTime = timeSpan.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
            TimerText.Text = formattedTime;
        }
    }
    
}
