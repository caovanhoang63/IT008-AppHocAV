using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IT008_AppHocAV.Services;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class GptWritingResponsePopUp : Page
    {
        public GptWritingResponsePopUp()
        {
            InitializeComponent();
        }

        public async Task LoadResult(Func func,string topic, string answer)
        {
            string result = await Task.Run(() =>
                ChatGpt.WritingHelp(func, topic, answer)
            );
            
            switch (func)
            {
                case Func.Ideas:
                    IdeasContainer.Visibility = Visibility.Visible;
                    Ideas.Text = result;
                    break;
                case Func.OutLine:
                    OutlineContainer.Visibility = Visibility.Visible;
                    Outline.Text = result;
                    break;
                case Func.Lexical:
                    LexicalContainer.Visibility = Visibility.Visible;
                    Lexical.Text = result;
                    break;
                case Func.Enhance:
                    EnhanceContainer.Visibility = Visibility.Visible;
                    Enhance.Text = result;
                    break;
                case Func.Sample:
                    SampleContainer.Visibility = Visibility.Visible;
                    Sample.Text = result;
                    break;
            }
            
        }
    }
}