using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Services;
using NAudio.Wave;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class SearchingPage : Page
    {
        public SearchingPage()
        {
            InitializeComponent();
            DicApiResultContainer.Visibility = Visibility.Hidden;
            GoogleTranslateContainer.Visibility = Visibility.Hidden;
        }
        
        public async Task Search(string text)
        {
            this.DicApiResultContainer.Visibility = Visibility.Collapsed;
            this.WordImage.Visibility = Visibility.Hidden;
            this.GoogleTranslateContainer.Visibility = Visibility.Hidden;
            this.MeaningsContainer.Children.Clear();
            try
            {
                List<DictionaryEntry> words = await DictionaryApi.SearchDictionary(text);
                this.VnWord.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", text);
                this.Word.Text = text;
                PhoneticHandler(words[0]);

                string meaningXaml = GenerateWordDictionaryEntry(words);
                
                
                UIElement generatedElement = (UIElement)XamlReader.Parse(meaningXaml);

                this.MeaningsContainer.Children.Add(generatedElement);
                
                this.DicApiResultContainer.Visibility = Visibility.Visible;
                var imageApi = await PexelsImageAPI.GetImages(text);
                if (imageApi != null)
                {
                    this.WordImage.Source = imageApi;
                    this.WordImage.Visibility = Visibility.Visible;
                }
            }
            //if Dictionary Api response null result, show google translate instead
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                this.GoogleTranslateContainer.Visibility = Visibility.Visible;
                this.GTransSlText.Selection.Text = text;
                this.GTransTlText.Selection.Text = await GoogleTranslateApi.GoogleTranslate("en", "vi", text);
            }
            catch (ArgumentOutOfRangeException e)
            {
                this.WordImage.Visibility = Visibility.Hidden;
            }
        }


        public void  PhoneticHandler(DictionaryEntry word)
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
                {
                    break;
                }
                else
                {
                    meanCount++;
                }
                xaml +=$@"
                    <TextBox Foreground=""#3B6449"" FontSize=""16"">
                        {definition.definition}
                    </TextBox>
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
                for (int i = 0; i < definition.synonyms.Length; i++)
                {
                    if (i != definition.synonyms.Length - 1)
                    {
                        synonyms += definition.synonyms[i] + ", ";
                    }
                    else
                    {
                        synonyms += definition.synonyms[i] + ". ";
                    }
                }

                
                string antonyms = "";
                for (int i = 0; i < definition.antonyms.Length; i++)
                {
                    if (i != definition.antonyms.Length - 1)
                    {
                        antonyms += definition.antonyms[i] + ", ";
                    }
                    else
                    {
                        antonyms += definition.antonyms[i] + ". ";
                    }
                }

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
    }
}