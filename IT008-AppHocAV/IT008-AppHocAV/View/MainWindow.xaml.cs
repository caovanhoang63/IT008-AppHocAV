using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
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


        
        #region Declare Fields
            private Dictionary<string, Page> _pageCache = new Dictionary<string, Page>();
            public string NoteCache;
            private readonly LoginWindow _loginWindow;
            private LoadingPage _loadingPage = new LoadingPage();
        #endregion
        
        #region Declare Constructors
            public MainWindow(LoginWindow loginWindow)
            {
                InitializeComponent();
                Page defaultPage = new HomePage(this);
                _pageCache["Home"] = defaultPage;
                _loginWindow = loginWindow;
                _loginWindow.InternetConnectionManager.InternetConnectionChanged += ChangedInternectConnectionStatusBar;
                Content.Navigate(defaultPage);
                User = DbConnection.UserRepository.GetUserById(UserId);
                DataContext = this;
            }
        #endregion

        #region Declare properties
    
        public BitmapImage DefaultAvatar { get; set; } =  new BitmapImage(new Uri("pack://application:,,,/Assets/Icon/AvatarIcon.png"));
            public int UserId => _loginWindow.UserId;
            public User User { get; set; }
            public DbConnection DbConnection => _loginWindow.DbConnection;

            public Dictionary<string, Page> PageCache
            {
                get => _pageCache;
                set => _pageCache = value;
            }

            public bool ContentLoaded
            {
                get => _contentLoaded;
                set => _contentLoaded = value;
            }

        #endregion

        #region Define CLick Event of DockBar Buttons

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

        #endregion        
        
        #region Define Click Event of Left Navigator 
        
        public void NavToSearching_OnClick(object sender, RoutedEventArgs e)
        {
            if (_pageCache.TryGetValue("Searching", out var value))
            {
                Content.Navigate(value);
            }
            else
            {
                TextBoxSearching.Focus();
            }
        }

        private void NavToWriting_OnClick(object sender, RoutedEventArgs e)
        {
            if (_pageCache.TryGetValue("ShowEssay", out var sEValue))
            {
                NavigateToPage("ShowEssay");
                return;
            }
            
            if (_pageCache.TryGetValue("WritingContent", out var wCValue))
            {
                NavigateToPage("WritingContent");
                return;
            }
            NavigateToPage("ShowListEssay");
        }
        
        private void NavToTranslate_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Translate");
        }

        private void NavToExam_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("ShowListExam");
        }
        private void NavToHome_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Home");
        }

        private void NavToFlashCard_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("FlashCard");
        }
        
        
        private void NavToRecall_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Recall");
        }

        private void ShowTakeNote_OnClick(object sender, RoutedEventArgs e)
        {
            if (Note.Visibility != Visibility.Visible)
            {
                Note.Visibility = Visibility.Visible;
                if (NoteCache != null)
                {
                    TakeNotePage takeNotePage = new TakeNotePage(NoteCache);
                    takeNotePage.NoteValueChanged += UpdateNoteCache;
                    Note.Content = takeNotePage;
                }
                else 
                {
                    TakeNotePage takeNotePage = new TakeNotePage();
                    takeNotePage.NoteValueChanged += UpdateNoteCache;
                    Note.Content = takeNotePage;
                }
            }
            else
            {
                Note.Visibility = Visibility.Collapsed;
            }
        }


        #endregion

        #region Avatar, Setting and Notify Button Handlers
            private void btnAvatar_Click(object sender, RoutedEventArgs e)
            {
                var contextMenu = ((Button)sender).ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.IsEnabled = true;
                    contextMenu.PlacementTarget = (sender as Button);
                    contextMenu.IsOpen = true;
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
        

        #endregion
        
        #region Navigate handlers
        
            private async Task DisplaySearchPage()
            {
                if (!InternetAvailability.IsInternetAvailable())
                    NavigateToPage("NoInternet");
                else
                {
                    NavigateToPage("Searching");
                    while (!(Content.Content is WordPage))
                    {
                        await Task.Delay(10);
                    }
                    if (Content.Content is WordPage page)
                    {
                        await page.Search(TextBoxSearching.Text);
                    }
                }
            }

            private Page CreatePage(string pageName)
            {
                Page page = null;
                
                switch (pageName)
                {
                    case "Searching":
                        page = new WordPage(this);
                        break;
                    case "Loading":
                        page = new LoadingPage();
                        break;
                    
                    case "Recall":
                        page = new VocabularyRecallPage(this);
                        break;
                    case "UserInfo":
                        page = new UserInfoPage(this);
                        break;

                    case "EditFlashCard":
                    {
                        if (_pageCache["FlashCard"] is FlashCardPage listcard)
                            page = new EditFlashCard(this, listcard);
                        break; 
                    }
                    case "ShowFlashCard":
                    {
                        if (_pageCache["FlashCard"] is FlashCardPage listcard)
                            page = new ShowFlashCardPage(this, listcard);
                        break;
                    }
                    case "CreateEssay":
                        page = new CreateEssayPage(this);
                        break;

                    case "FlashCard":
                        page = new FlashCardPage(this);
                        break;

                    case "MakeFlashCard":
                        page = new MakeFlashCard(this);
                        break;

                    case "NoInternet":
                        page = new NoInternetPage();
                        break;

                    case "WritingContent":
                        WordsStatusBarItem.Visibility = Visibility.Visible;
                        if (_pageCache.TryGetValue("CreateEssay", out var value))
                        {
                            var writingPage = (CreateEssayPage)value;
                            page = new WritingContentPage(this, writingPage.Essay);
                        }
                        else
                        {
                            var writingPage = (ShowEssayPage)_pageCache["ShowEssay"];
                            page = new WritingContentPage(this, writingPage.Essay);
                        }
                        break;

                    case "ShowListEssay":
                        page = new ShowListEssayPage(this);
                        break;

                    case "ShowEssay":
                        if (_pageCache["ShowListEssay"] is ShowListEssayPage listEssayPage)
                            page = new ShowEssayPage(this, listEssayPage);
                        break;

                    case "Translate":
                        page = new TranslatePage();
                        break;

                    case "ShowListExam":
                        page = new ShowListExamPage(this);
                        break;

                    case "CreateExam":
                        page = new CreateExam(this);
                        break;

                    case "Exam":
                        page = new DoExamPage(this);
                        break;

                    // Add more cases as needed...

             
                }

                

            return page;
            }
            
            public void NavigateToPage(string pageName)
            {
                Content.Navigate(_loadingPage);
                StatusBarCurrentPage.Text = pageName;
                WordsStatusBarItem.Visibility = Visibility.Collapsed;
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

        #endregion

        private async void TextBoxSearching_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await DisplaySearchPage();
            }
        }
        
        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
                await DisplaySearchPage();
        }

        #region Statusbar Handlers

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

        #endregion
        
        #region  UI Event Handler

            private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
            {
                if (TextBoxSearching.Text != string.Empty)
                {
                    TextBlockPlaceHolder.Visibility = Visibility.Hidden;
                }
                else
                {
                    TextBlockPlaceHolder.Visibility = Visibility.Visible;
                }
            }
        
            private void SearchTextContainer_OnGotFocus(object sender, RoutedEventArgs e)
            {
                SearchTextContainer.BorderBrush = Brushes.CornflowerBlue;
            }

            private void SearchTextContainer_OnLostFocus(object sender, RoutedEventArgs e)
            {
                SearchTextContainer.BorderBrush = Brushes.Black;
            }
        
            private void MenuButton_OnClick(object sender, RoutedEventArgs e)
            {
                if (NavBar.Width == 52)
                {
                    NavBar.Width = 156;
                    foreach (var child in  NavBar.Children)
                    {
                        if (child is Button button)
                            button.Width = 156;
                    }
                }
                else
                {
                    NavBar.Width = 52;
                    foreach (var child in  NavBar.Children)
                    {
                        if (child is Button button)
                            button.Width = 50;
                    }
                }
            }

        #endregion

        private void UpdateNoteCache(object sender, string newNoteValue)
        {
            NoteCache = newNoteValue;
        }

        private void UserInfo_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("UserInfo");
        }
    }
}
