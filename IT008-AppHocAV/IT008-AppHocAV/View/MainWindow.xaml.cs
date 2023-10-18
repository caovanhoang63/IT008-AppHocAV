using System;
using System.Collections.Generic;
using System.IO.Compression;
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
using IT008_AppHocAV.View.MainWindow;

namespace IT008_AppHocAV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnAvatar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is avatar");
        }

        private void NavToSearching_OnClick(object sender, RoutedEventArgs e)
        {
            Content.Content = new Searching_page();
        }

        private void NavToWriting_OnClick(object sender, RoutedEventArgs e)
        {
            Content.Content = new WritingPage();
        }

        private void NavToExam_OnClick(object sender, RoutedEventArgs e)
        {
            Content.Content = new ExamPage();
        }

        private void NavToFlashCard_OnClick(object sender, RoutedEventArgs e)
        {
            Content.Content = new FlashCardPage();
        }

        private void ShowTakeNote_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void SearchTextContainer_OnGotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextContainer.BorderBrush = Brushes.CornflowerBlue;
        }

        private void SearchTextContainer_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SearchTextContainer.BorderBrush = Brushes.Transparent;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSearching.Text != string.Empty)
            {
                TextBlockPlaceHolder.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBlockPlaceHolder.Visibility = Visibility.Visible;
            }
        }

        private void TextBoxSearching_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && textBoxSearching.Text != string.Empty)
            {
                MessageBox.Show("Search " + textBoxSearching.Text);
            }
        }
    }
}
