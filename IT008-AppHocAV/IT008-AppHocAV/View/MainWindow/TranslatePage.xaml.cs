using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using IT008_AppHocAV.Services;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class TranslatePage : Page
    {
        private DispatcherTimer _debounceTimer;
        private readonly Dictionary<string, string> _languages = new Dictionary<string, string>();

        public TranslatePage()
        {
            InitializeComponent();
            _languages.Add("English","en");
            _languages.Add("Vietnamese", "vi");
        }

        public TranslatePage(string text)
        {
            InitializeComponent();
            _languages.Add("English","en");
            _languages.Add("Vietnamese", "vi");
            

        }

        /// <summary>
        /// Switch source and destination language
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchLanBtn_OnClick(object sender, RoutedEventArgs e)
        {
            string temp = GtslLabel.Content.ToString();
            GtslLabel.Content =GttlLabel.Content;
            GttlLabel.Content = temp;
            TranslateRtb();
        }

        /// <summary>
        /// Calls Google translate with each text changed event every 500ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GTransSlText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel all previously expected commands (if any)
            if (_debounceTimer != null)
            {
                _debounceTimer.Stop();
            }
            // create new timer
            _debounceTimer = new DispatcherTimer();
            _debounceTimer.Interval = TimeSpan.FromMilliseconds(500); // Wait 500ms after no more changes
            _debounceTimer.Tick += async (s, args) =>
            {
                _debounceTimer.Stop();
                TranslateRtb();
            };

            // start countdown after 500ms when nothing change
            _debounceTimer.Start();
        }
        
        
        /// <summary>
        /// Get text from input rich text box and calls google translate api 
        /// </summary>
        private async void TranslateRtb()
        {
            string sltext = new TextRange(GTransSlText.Document.ContentStart, GTransSlText.Document.ContentEnd).Text;
            if (!string.IsNullOrEmpty(sltext.Trim()))
            {
                string sl = _languages[GtslLabel.Content.ToString()];
                string tl = _languages[GttlLabel.Content.ToString()];
                string tltext = await GoogleTranslateApi.GoogleTranslate(sl, tl, sltext);
                Console.WriteLine(tltext);
                GTransTlText.Document.Blocks.Clear();
                GTransTlText.Document.Blocks.Add(new Paragraph(new Run(tltext)));
            }
            else
            {
                GTransTlText.Document.Blocks.Clear();
            }
        }
    }
}