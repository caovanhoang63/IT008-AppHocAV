﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Repositories.DbConnection;
using IT008_AppHocAV.Services;
using NAudio.Wave;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class WordPage : Page
    {
        public DictionaryEntry Word { get; set; }
        private DbConnection _dbConnection;
        private IT008_AppHocAV.MainWindow _mainWindow;
        private bool _already = false;
        private int RecallId = -1;
        public List<int?> CheckedDefinitionIds { get; set; } = new List<int?>();
        private bool LoadSuccess = false;
        
        public WordPage(IT008_AppHocAV.MainWindow mainWindow)
        {
            InitializeComponent();
            _dbConnection = mainWindow.DbConnection;
            _mainWindow = mainWindow;
        }


        public async Task<bool> Search(string word)
        {
            LoadSuccess = false;
            _mainWindow.NavigateToPage("Loading");
            _mainWindow.StatusBarCurrentPage.Text = "Searching";
            await Task.Run(() => Word = _dbConnection.DictionaryRepository.GetDictionaryEntry(word));
            if (Word != null)
            {
                ResetPage();
                Container.ScrollToTop();
                CheckedDefinitionIds = _dbConnection.RecallRepository.FindDefinitionIdByWordAndUserId(Word.word,_mainWindow.UserId);
                RecallId = _dbConnection.RecallRepository.IsRecalled(_mainWindow.UserId, Word.word);
                
                if (RecallId != -1)
                    RecallCheckBox.IsChecked = true;
                else
                    RecallCheckBox.IsChecked = false;
                MeaningsListview.ItemsSource = Word.meanings;
                PhoneticHandler(Word);
                DataContext = this;
                _already = true;
                _mainWindow.NavigateToPage("Searching");
                return true;
            }

            await NavToTranslate(word);
            _mainWindow.StatusBarCurrentPage.Text = "Translate";
            return false;
        }



        /// <summary>
        /// Handles to display phonetic
        /// </summary>
        /// <param name="word"></param>
        private void PhoneticHandler(DictionaryEntry word)
        {
            string usPhonetic = "";
            string ukPhonetic = "";
            string auPhonetic = "";

            foreach (Phonetic phonetic in word.phonetics)
            {
                if (phonetic.audio != string.Empty)
                {
                    if (phonetic.audio.Contains("uk"))
                    {
                        ukPhonetic = phonetic.text;
                        UkSpeakerUri.Text = phonetic.audio;
                    }
                    else if (phonetic.audio.Contains("us"))
                    {
                        usPhonetic = phonetic.text;
                        UsSpeakerUri.Text = phonetic.audio;
                    }
                    else if (phonetic.audio.Contains("au"))
                    {
                        auPhonetic = phonetic.text;
                        AuSpeakerUri.Text = phonetic.audio;
                    }
                }
            }

            //If auPhonetic == "", auPhonetic = default phonetic
            if (auPhonetic == String.Empty)
            {
                BtnUaSpeaker.Visibility = Visibility.Collapsed;
                auPhonetic = word.phonetic;
            }
            else
            {
                BtnUaSpeaker.Visibility = Visibility.Visible;
            }

            AuPhonetic.Text = auPhonetic;
            //If ukPhonetic != "", show it
            if (ukPhonetic != string.Empty)
            {
                this.UkPhonetic.Text = ukPhonetic;
                this.UkPhoneticContainer.Visibility = Visibility.Visible;
            }
            else
            {
                this.UkPhoneticContainer.Visibility = Visibility.Collapsed;
            }

            //If usPhonetic != "", show it
            if (usPhonetic != string.Empty)
            {
                this.UsPhonetic.Text = usPhonetic;
                this.UsPhoneticContainer.Visibility = Visibility.Visible;
            }
            else
            {
                this.UsPhoneticContainer.Visibility = Visibility.Collapsed;
            }

            //if usPhonetic and ukPhonetic neither == "", show auPhonetic
            if (usPhonetic == string.Empty && ukPhonetic == string.Empty)
            {
                this.AuPhonetic.Visibility = Visibility.Visible;
            }
            else
            {
                this.AuPhoneticContainer.Visibility = Visibility.Collapsed;
            }
        }

        #region Sound

        //play Pronunciation sound
        private async void BtnUkSpeaker_OnClick(object sender, RoutedEventArgs e)
        {
            await PlaySound(UkSpeakerUri.Text.ToString());
        }

        private async void BtnUsSpeaker_OnClick(object sender, RoutedEventArgs e)
        {
            await PlaySound(UsSpeakerUri.Text.ToString());
        }

        private async void BtnUaSpeaker_OnClick(object sender, RoutedEventArgs e)
        {
            await PlaySound(AuSpeakerUri.Text.ToString());
        }

        // Play sound with https://github.com/naudio/NAudio/blob/master/Docs/PlayAudioFromUrl.md?fbclid=IwAR23AvVfary9oc8IpOIFh_en6wkbLptkKvxDNLxyJeR1VVjZ3FvZG9smGPA
        private static async Task PlaySound(string url)
        {
            using (var mf = new MediaFoundationReader(url))
            using (var wo = new WasapiOut())
            {
                wo.Init(mf);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000);
                }
            }
        }


        #endregion

        //fix scrolling issue when using a listview inside a scroll viewer
        private void instScroll_Loaded(object sender, RoutedEventArgs e)
        {
            MeaningsListview.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH), true);
        }

        private void MyMouseWheelH(object sender, RoutedEventArgs e)
        {

            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;

            double x = (double)eargs.Delta;

            double y = Container.VerticalOffset;

            Container.ScrollToVerticalOffset(y - x);
        }


        /// <summary>
        /// Navigate Mainwindow to translate if api return null
        /// </summary>
        /// <param name="text"></param>
        private async Task NavToTranslate(string text)
        {
            _mainWindow.PageCache.Remove("Translate");

            TranslatePage page = new TranslatePage(text);

            _mainWindow.PageCache.Add("Translate", page);
            _mainWindow.NavigateToPage("Translate");

            page.GoogleTranslateContainer.Visibility = Visibility.Visible;
            page.GTransSlText.Selection.Text = text;
            page.GTransTlText.Selection.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", text);

            if (!_already)
                _mainWindow.PageCache.Remove("Searching");
        }


        private async void SearchSyAntonym_OnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (sender is Button button)
            {
                if (await Search(button.Content.ToString()))
                    _mainWindow.NavigateToPage("Searching");
            }
        }

        private void MoreSyAntonym_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Content is Label label)
                {
                    if (button.Parent is StackPanel stackPanel)
                        if (stackPanel.Height != 40)
                        {
                            stackPanel.Height = 40;
                            label.Content = "+ More synonym and antonyms";

                        }
                        else
                        {
                            stackPanel.Height = Double.NaN;
                            label.Content = "- Less synonym and antonyms";
                        }
                }
                
                
            }
                
        }

        private void RecallCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            if (RecallId == -1)
                RecallId = _dbConnection.RecallRepository.AddNewWord(_mainWindow.UserId, Word.word);
        }

        private void RecallCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _dbConnection.RecallRepository.DeleteById(RecallId);
            RecallId = -1;
        }

        private void MoreDefinition_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                CheckedDefinitionIds = _dbConnection.RecallRepository.FindDefinitionIdByWordAndUserId(Word.word,_mainWindow.UserId);
                if (button.Content is Label label)
                {
                    Console.WriteLine(label.Content);
                    if (label.Content != "- Less definition")
                    {

                        Console.WriteLine("more Click");
                        if (button.Parent is StackPanel stackPanel)
                        {
                            if (stackPanel.Children[1] is ItemsControl itemsControl)
                            {
                                Meaning meaning = (Meaning)((FrameworkElement)(sender)).DataContext;

                                itemsControl.ItemsSource = meaning.definitions;

                            }
                          
                        }
                        label.Content = "- Less definition";
                    }
                    else
                    {
                        Console.WriteLine("Less Click");

                        if (button.Parent is StackPanel stackPanel)
                        {
                            if (stackPanel.Children[1] is ItemsControl itemsControl)
                            {
                                Meaning meaning = (Meaning)((FrameworkElement)(sender)).DataContext;
                                itemsControl.ItemsSource = meaning.definitions.Take(5);
                            }
                        }
                        label.Content = "+ More definition";
                    }
                }
            }
            
            
        }
        
        private void Definition_Checked(object sender, RoutedEventArgs e)
        {
            if (!LoadSuccess) return;
            
            if (RecallCheckBox.IsChecked == false)
                RecallCheckBox.IsChecked = true;
            
            if (!(sender is CheckBox checkBox)) return;
            
            int id = (int)checkBox.Tag;
            List<VocabularyRecallLog> logs = _dbConnection.RecallRepository.FindVocabularyRecallLogsByWord(Word.word);
            
            foreach (var log  in logs)
            {
                if (log.Meaning == string.Empty && log.DefinitionId == null)
                {
                    _dbConnection.RecallRepository.AddDefinitionId(log.Id,id);
                    return;
                }
            }
            
            VocabularyRecallLog recallLog = new VocabularyRecallLog();
            recallLog.UserId = _mainWindow.UserId;
            recallLog.Word = Word.word;
            recallLog.DefinitionId = id;
            _dbConnection.RecallRepository.AddNewRecallLog(recallLog);
        }

        private void Definition_UnChecked(object sender, RoutedEventArgs e)
        {
            if (!LoadSuccess) return;
            
            if (!(sender is CheckBox checkBox)) return;
            
            int id = (int)checkBox.Tag;
            
            _dbConnection.RecallRepository.DeleteByDefinitionIdAndWord(id, Word.word);
        }

     
        
        
        private void ResetPage()
        {
            DataContext = null;
            RecallId = -1;
            MeaningsListview.ItemsSource = null;
            UkPhoneticContainer.Visibility = Visibility.Collapsed;
            UsPhoneticContainer.Visibility = Visibility.Collapsed;
            AuPhoneticContainer.Visibility = Visibility.Collapsed;
            AuPhonetic.Visibility = Visibility.Collapsed;
            UkPhonetic.Text = string.Empty;
            UsPhonetic.Text = string.Empty;
            AuPhonetic.Text = string.Empty;
            UkSpeakerUri.Text = string.Empty;
            UsSpeakerUri.Text = string.Empty;
            AuSpeakerUri.Text = string.Empty;
        }


        private void DefinitionCheckBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadSuccess = true;
        }

        private void WordPage_OnLayoutUpdated(object sender, EventArgs e)
        {

        }
    }
  
    #region Converters

    public class CheckDefinitionCheckBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is WordPage wordPage && values[1] is int id)
            {
                if (wordPage.CheckedDefinitionIds.Contains(id))
                    return true;
                return false;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing };
        }
    }
    
    
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string)
                ? System.Windows.Visibility.Collapsed
                : System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<Text> list)
            {
                if (list.Count == 1 && list[0].text.Trim() == string.Empty)
                    return Visibility.Collapsed;

                return list.Any() ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MoreSyAntonymsVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is List<Text> list1 && values[1] is List<Text> list2)
            {
                if (list1.Count == 1
                    && list1[0].text.Trim() == string.Empty
                    && list2.Count == 1
                    && list2[0].text.Trim() == string.Empty)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ItemsLimiter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count;
            if (Int32.TryParse((string)parameter, out count))
            {
                return ((IEnumerable<object>)value).Take(count);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        
    }

    public class NumberDefinitionToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<Definition> definitions)
            {
                if (definitions.Count > 5)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
                
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Visibility.Visible;
        }
    }

    #endregion


  


}
