using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Services;
using NAudio.Wave;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class SearchingPage : Page
    {

        #region Declare Fields
            private readonly Dictionary<string, Pair<TextBox,bool>> _definitionTextBoxList = new Dictionary<string,Pair<TextBox,bool>>();
            private int _definitionCount = 0;
            private IT008_AppHocAV.MainWindow _mainWindow;
            private bool _already = false;
        #endregion

        #region Declare Constructors
            public SearchingPage(IT008_AppHocAV.MainWindow mainWindow )
            {
                InitializeComponent();
                DicApiResultContainer.Visibility = Visibility.Hidden;
                _mainWindow = mainWindow;
            }

        #endregion

        
        //Clean UI for new Search
        private void ResetPage()
        {
            _definitionCount = 0;
            this.DicApiResultContainer.Visibility = Visibility.Collapsed;
            this.WordImage.Visibility = Visibility.Hidden;
            this.MeaningsContainer.Children.Clear();
            _definitionTextBoxList.Clear();
        }
        
        
        public async Task Search(string text)
        {
            if (text == this.Word.Text)
                return;
            if (!_already)
                ResetPage();
            try
            {
                //Call DictionaryApi
                List<DictionaryEntry> words = await DictionaryApi.SearchDictionary(text);

                if (words != null)
                {
                    //Create header of page 
                    this.Word.Text = text;
                    this.VnWord.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", text);
                    PhoneticHandler(words[0]);

                    //Generate meanings xaml
                    string meaningXaml = GenerateWordDictionaryEntry(words);
                    UIElement generatedElement = (UIElement)XamlReader.Parse(meaningXaml);

                    //Write meanings xaml to page
                    this.MeaningsContainer.Children.Add(generatedElement);
                    this.DicApiResultContainer.Visibility = Visibility.Visible;

                    //Add click event for drop down translate definition buttons
                    FindButtonsInStackPanels(MeaningsContainer);
                    var imageApi = await PexelsImageAPI.GetImages(text);
                    if (imageApi != null)
                    {
                        this.WordImage.Source = imageApi;
                        this.WordImage.Visibility = Visibility.Visible;
                    }

                    _already = true;
                }

                //if Dictionary Api response null result, show google translate instead
                else
                    await NavToTranslate(text);
                
            }
            //if Dictionary Api response null result, show google translate instead
            catch (NullReferenceException e)
            {

            }
            //if Image error, hidden WordImage
            catch (ArgumentOutOfRangeException e)
            {
                this.WordImage.Visibility = Visibility.Hidden;
            }
        }

        
        
        /// <summary>
        /// Navigate Mainwindow to translate if api return null
        /// </summary>
        /// <param name="text"></param>
        private async Task NavToTranslate(string text)
        {
            _mainWindow.PageCache.Remove("Translate");

            TranslatePage page = new TranslatePage(text);

            _mainWindow.PageCache.Add("Translate",page);
            _mainWindow.NavigateToPage("Translate");
                
            page.GoogleTranslateContainer.Visibility = Visibility.Visible;
            page.GTransSlText.Selection.Text = text;
            page.GTransTlText.Selection.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", text);

            if (!_already)
                _mainWindow.PageCache.Remove("Searching");
        }
        
        
        
        
        /// <summary>
        /// Handle Phonetic from DictionaryApi to display on Header of page
        /// </summary>
        /// <returns></returns>
        private void  PhoneticHandler(DictionaryEntry word)
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
            if (usPhonetic == string.Empty && ukPhonetic == string.Empty )
            {
                this.AuPhonetic.Visibility = Visibility.Visible;
            }
            else
            {
                this.AuPhoneticContainer.Visibility = Visibility.Collapsed;
            }
        }
        
        
        /// <summary>
        /// Generate xaml for all result from dictionaryApi by call GenerateWordMeanings
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        private string GenerateWordDictionaryEntry(List<DictionaryEntry> words)
        {
            string xaml = @"<StackPanel xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">";
            foreach (var word in words)
            {
                foreach (var meaning in word.meanings)
                {
                    xaml += GenerateWordMeaningsXaml(meaning);
                }
            }
            xaml += @"</StackPanel>";
            return xaml;
        }
        
        /// <summary>
        /// Generate xaml for each word from dictionaryApi
        /// </summary>
        /// <param name="meaning"></param>
        /// <returns></returns>
        private string GenerateWordMeaningsXaml(Meaning meaning)
        {
            string xaml = $@"
                <StackPanel xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <Label FontSize=""20"" >{meaning.partOfSpeech}</Label>
                    <Border BorderThickness=""0 0 0  1 "" BorderBrush=""Black""></Border>
                    <StackPanel Margin=""20 0 0 0 "">
            ";
            int meanCount = 1;
            foreach (var definition in meaning.definitions)
            {
                if (meanCount > 5)
                    break;
                else
                    meanCount++;
                
                string Endefinition = "Endefinition" + _definitionCount;
                string Transdefinition = "Transdefinition" + _definitionCount;
                string Vndefinition = "Vndefinition" + _definitionCount;
                
                xaml +=$@"
                    <StackPanel>
                        <TextBox Name=""{Endefinition}"" Foreground=""#3B6449"" FontSize=""16"">
                           {definition.definition}
                        </TextBox>
                        <Button Name=""{Transdefinition}"" FontSize=""14"" Width=""18"" Height=""auto"" Background=""Transparent"" BorderThickness=""0""
                                Margin=""0 0 730 0"" > 
                                <Image  Source=""../../Assets/Icon/DropDownBtnIcon.png""></Image>
                        </Button>
                        <TextBox  Name=""{Vndefinition}"" Foreground=""#3B6449"" FontSize=""16"" Visibility=""Collapsed"">
                           {(definition.definition)}
                        </TextBox>
                    </StackPanel>
                ";
                
                if (!string.IsNullOrEmpty(definition.example))
                {
                    xaml += $@"
                          <TextBox Margin=""0 5 0 0"">
                                <TextBox.Padding>         
                                    <Thickness Left=""30""/>           
                                </TextBox.Padding>
                                Example: {definition.example}
                           </TextBox>";
                }
                
                string synonyms = "";
                // for (int i = 0; i < definition.synonyms.text.Count; i++)
                // {
                //     if (i != definition.synonyms.text.Count - 1)
                //     {
                //         synonyms += definition.synonyms.text[i] + ", ";
                //     }
                //     else
                //     {
                //         synonyms += definition.synonyms.text[i] + ". ";
                //     }
                // }
                
                string antonyms = "";
                // for (int i = 0; i < definition.antonyms.text.Count; i++)
                // {
                //     if (i != definition.antonyms.text.Count - 1)
                //     {
                //         antonyms += definition.antonyms.text[i] + ", ";
                //     }
                //     else
                //     {
                //         antonyms += definition.antonyms.text[i] + ". ";
                //     }
                // }
                if (synonyms.Trim() != "")
                {
                    xaml += $@"
                    <TextBox FontStyle=""Italic"" Margin=""0 5 0 0"">
                        <TextBox.Padding>         
                            <Thickness Left=""20"" />           
                        </TextBox.Padding>
                            Synonym: {synonyms}
                    </TextBox>";
                }
                if (antonyms.Trim() != "")
                {
                    xaml += $@"<TextBox FontStyle=""Italic"" Margin=""0 5 0 0"">
                        <TextBox.Padding>         
                            <Thickness Left=""20"" />           
                        </TextBox.Padding>
                            Antonym: {antonyms}
                    </TextBox>";
                }
                xaml+=$@"
                    <Border Margin=""0 30 0 0"" BorderThickness=""0 0 0 1 "" BorderBrush=""Black"" Width=""750""></Border>
                    ";
                _definitionCount++;
            }
            xaml += @"
                    </StackPanel>
                    <Border Margin=""0 10 0 0 "" BorderThickness=""0 0 0  1 "" BorderBrush=""Black""></Border>
                </StackPanel>";
            return xaml;
        }
        
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
            using(var mf = new MediaFoundationReader(url))
            using(var wo = new WasapiOut())
            {
                wo.Init(mf);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    await Task.Delay(1000);
                }
            }
        }
        
        private async void TransDefinitionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string textBoxName = "Vndefinition" + button.Name.Substring(15);
                TextBox textBox = _definitionTextBoxList[textBoxName].First;
                if (textBox != null)
                {
                    if (textBox.Visibility == Visibility.Collapsed)
                    {
                        if (!_definitionTextBoxList[textBoxName].Second)
                        {
                            textBox.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", textBox.Text);
                            _definitionTextBoxList[textBoxName].Second = true;
                        }
                        textBox.Visibility = Visibility.Visible;    
                    }
                    else
                    {
                        textBox.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        
        private void FindButtonsInStackPanels(DependencyObject parent)
        {
            if (parent != null)
            {
                int countChild = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < countChild; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is StackPanel stackPanel)
                    {
                        foreach (UIElement stackPanelChild in stackPanel.Children)
                        {
                            if (stackPanelChild is Button button)
                            {
                                button.Click += TransDefinitionBtn_OnClick;
                            } else if (stackPanelChild is TextBox textBox)
                            {
                                _definitionTextBoxList[textBox.Name] = new Pair<TextBox, bool>(textBox, false);
                            }
                        }
                    }
                    //if this child is not a button, find buttons in this child
                    FindButtonsInStackPanels(child);
                }
                
            }
        }

      
    }
}