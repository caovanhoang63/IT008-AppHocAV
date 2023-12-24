using IT008_AppHocAV.Services;

namespace IT008_AppHocAV.Models
{
    public class GptWritingResponse
    {
        private Func _func;
        private string _header;
        private string _content;

        public Func Func
        {
            get => _func;
            set => _func = value;
        }

        public string Header
        {
            get => _header;
            set => _header = value;
        }

        public string Content
        {
            get => _content;
            set => _content = value;
        }
    }
}