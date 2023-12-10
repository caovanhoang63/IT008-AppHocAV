using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            private readonly LoginWindow _loginWindow;
        #endregion
        
        #region Declare Constructors
            public MainWindow(LoginWindow loginWindow)
            {
                InitializeComponent();
                Page defaultPage = new SearchingPage(this);
                _pageCache["Searching"] = defaultPage;
                _loginWindow = loginWindow;
                _loginWindow.InternetConnectionManager.InternetConnectionChanged += ChangedInternectConnectionStatusBar;
            }
        #endregion

        #region Declare properties

            public int UserId => _loginWindow.UserId;
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
        
            private void NavToSearching_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("Searching");
            }

            private void NavToWriting_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("ShowListEssay");
            }

            private void NavToExam_OnClick(object sender, RoutedEventArgs e)
            {
                NavigateToPage("Exam");
            }

        private void NavToFlashCard_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateToPage("FlashCard");
        }

        private void ShowTakeNote_OnClick(object sender, RoutedEventArgs e)
        {
            if (Note.Visibility != Visibility.Visible)
            {
                Note.Visibility = Visibility.Visible;
                TakeNotePage takeNotePage = new TakeNotePage();
                Note.Content = takeNotePage;
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
                (sender as Button).ContextMenu.IsEnabled = true;
                (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
                (sender as Button).ContextMenu.IsOpen = true;
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
                    while (!(Content.Content is SearchingPage))
                    {
                        await Task.Delay(10);
                    }
                    if (Content.Content is SearchingPage page)
                    {
                        await page.Search(TextBoxSearching.Text);
                    }
                }
            }

            private Page CreatePage(string pageName)
            {
                Page page = null;

                if (pageName == "Searching")
                {
                    page = new SearchingPage(this); 
                }
                else if (pageName == "CreateEssay")
                {
                    page = new CreateEssayPage(this);
                }
                else if (pageName == "Exam")
                {
                    page = new ExamPage(this);
                } 
                else if (pageName == "FlashCard")
                {
                    page = new FlashCardPage(this);
                }
                 else if (pageName == "ShowFlashCard")
                 {
                /*if (pageCache["FlashCard"] is FlashCardPage listFlashCard)
                    page = new ShowFlashCard(this, listFlashCard);*/
                     page = new ShowFlashCardPage(this);


                 }
                  else if (pageName == "MakeFlashCard")
                  {
                     page = new MakeFlashCard(this);

                  }


                  else if (pageName == "NoInternet")
                     {
                         page = new NoInternetPage();
                     }
                  else if (pageName == "WritingContent")
                  {
                    if (_pageCache.TryGetValue("CreateEssay", out var value))
                    {
                        var writingPage = (CreateEssayPage)value;
                        page = new WritingContentPage(this,writingPage.Essay );
                    }
                    else
                    {
                        var writingPage = (ShowEssayPage)_pageCache["ShowEssay"];
                        page = new WritingContentPage(this,writingPage.Essay );
                    }

                  } 
                  else if (pageName == "ShowListEssay")
                  {
                    page = new ShowListEssayPage(this);
                  } 
                  else if (pageName == "ShowEssay")
                  {
                    if (_pageCache["ShowListEssay"] is  ShowListEssayPage listEssayPage )
                        page = new ShowEssayPage(this,listEssayPage);
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
        




            private void MenuButton_OnChecked(object sender, RoutedEventArgs e)
            {
                NavBar.Width = 50;
                foreach (var child in NavBar.Children)
                {
                    if (child is Button btn)
                    {
                        btn.Width = 50;
                    }
                }
                
            }






            private void MenuButton_OnUnchecked(object sender, RoutedEventArgs e)
            {
                NavBar.Width = 156;
                foreach (var child in NavBar.Children)
                {
                    if (child is Button btn)
                    {
                        btn.Width = 156;
                    }
                }
            }

            #endregion

    }

    internal class ShowFlashCard : Page
    {
        private MainWindow mainWindow;
        private FlashCardPage listFlashCard;

        public ShowFlashCard(MainWindow mainWindow, FlashCardPage listFlashCard)
        {
            this.mainWindow=mainWindow;
            this.listFlashCard=listFlashCard;
        }
    }
}
