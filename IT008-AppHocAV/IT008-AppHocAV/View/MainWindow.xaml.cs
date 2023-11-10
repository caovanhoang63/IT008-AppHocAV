using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IT008_AppHocAV.Util;
using IT008_AppHocAV.View;
using IT008_AppHocAV.View.MainWindow;

namespace IT008_AppHocAV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            Page defaultPage = new SearchingPage(this);
            _pageCache["Searching"] = defaultPage;
            _loginWindow = loginWindow;
            _loginWindow.InternetConnectionManager.InternetConnectionChanged += ChangedInternectConnectionStatusBar;
        }
        
        //Declare attributes
        private readonly Dictionary<string, Page> _pageCache = new Dictionary<string, Page>();
        private readonly LoginWindow _loginWindow;

        //Declare properties
        public int UserId => _loginWindow.UserId;

        
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

        }

        #region Define Navigate Buttons
        
            private void NavToSearching_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("Searching");
            }

            private void NavToWriting_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("Writing");
            }

            private void NavToExam_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("Exam");
            }

            private void NavToFlashCard_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("FlashCard");
            }

        #endregion


        
        
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

        private Page CreatePage(string pageName)
        {
            Page page = null;
            if (pageName == "Searching")
            {
                page = new SearchingPage(this); 
            }
            else if (pageName == "Writing")
            {
                page = new WritingPage(this);
            }
            else if (pageName == "Exam")
            {
                page = new ExamPage(this);
            } else if (pageName == "FlashCard")
            {
                page = new FlashCardPage();
            } else if (pageName == "NoInternet")
            {
                page = new NoInternetPage();
            } else if (pageName == "WritingContentPage")
            {
                if (_pageCache["Writing"] is  WritingPage writingPage )
                    page = new WritingContentPage(this,writingPage);
            }
            return page;
        }
        
        public void NavigateToPage(string pageName)
        {
            StatusBarCurrentPage.Text = pageName; 
            if (_pageCache.TryGetValue(pageName, out var value))
            {
                Content.Navigate(value);
            }
            else
            {
                Page newPage = CreatePage(pageName); 
                _pageCache[pageName] = newPage; 
                Content.Navigate(newPage); 
            }
        }

        
        private async void TextBoxSearching_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await DisplaySearchPage();
            }
        }
        private async void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
                await DisplaySearchPage();
        }

        private async Task DisplaySearchPage()
        {
            if (!InternetAvailability.IsInternetAvailable())
                NavigateToPage("NoInternet");
            else
            {
                NavigateToPage("Searching");
                while (!(Content.Content is SearchingPage))
                {
                    await Task.Delay(10);
                }
                if (Content.Content is SearchingPage page)
                {
                    await page.Search(textBoxSearching.Text);
                }
            }
        }

        private void LogOutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out!?","",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _loginWindow.Show();
                Close();
            }
        }

        private void ChangedInternectConnectionStatusBar(bool isConnected)
        {
            if (!isConnected)
            {
                InternetConnectionStatusBarItem.Visibility = Visibility.Visible;
            }
            else
            {
                InternetConnectionStatusBarItem.Visibility = Visibility.Collapsed;
            }
        }

        private void NavButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
                btn.Width = 130;
        }

        private void NavButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
                if (MenuButton.IsChecked != null && !MenuButton.IsChecked.Value)
                    btn.Width = 50;
        }

        private void MenuButton_OnChecked(object sender, RoutedEventArgs e)
        {
            foreach (var child in NavBar.Children)
                if (child is Button btn)
                    btn.Width = 130;
        }

        private void MenuButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            foreach (var child in NavBar.Children)
                if (child is Button btn)
                    btn.Width = 50;
        }
    }
}
