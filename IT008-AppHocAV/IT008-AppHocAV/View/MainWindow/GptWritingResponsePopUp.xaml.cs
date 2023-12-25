using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IT008_AppHocAV.Models;
using IT008_AppHocAV.Services;

namespace IT008_AppHocAV.View.MainWindow
{
    public partial class GptWritingResponsePopUp : Page
    {
        private List<GptWritingResponse> _data;
        public GptWritingResponsePopUp()
        {
            InitializeComponent();
            _data = new List<GptWritingResponse>();
            ContentListView.ItemsSource = _data;
               
        }

        public async Task LoadResult(Func func,string topic, string answer)
        {
            
            GptWritingResponse result = await Task.Run(() =>
                ChatGpt.WritingHelp(func, topic, answer)
            );
            
            GptWritingResponse temp = null;
            foreach (var e in _data)
            {
                if (e.Func == result.Func)
                    temp = e;
            }
            _data.Remove(temp);
            
            _data.Insert(0,result);
           
            RefeshPage();
        }

        private void RefeshPage()
        {
            ContentListView.Items.Refresh();
        }
    }
}