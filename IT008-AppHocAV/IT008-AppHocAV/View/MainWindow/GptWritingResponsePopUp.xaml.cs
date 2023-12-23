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

        public async void LoadResult(Func func,string topic, string answer)
        {
            string result =  await Task.Run(
                function: () => 
                    ChatGpt.WritingHelp(
                        func: func,
                        topic: topic,
                        answer: answer)
            );

            switch (func)
            {
                case Func.Ideas:
                    IdeasContainer.Visibility = Visibility.Visible;
                    Ideas.Text = result;
                    break;
                case Func.OutLine:
                    Outline.Visibility = Visibility.Visible;
                    Outline.Text = result;

                    break;
                case Func.Lexical:
                    Lexical.Visibility = Visibility.Visible;
                    Lexical.Text = result;

                    break;
                case Func.Enhance:
                    Enhance.Visibility = Visibility.Visible;
                    Enhance.Text = result;
                    break;
            }
        }
    }
}