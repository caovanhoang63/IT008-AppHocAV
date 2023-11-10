using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Models
{
    public class Essay
    {
        public Essay()
        {
            this._title = "";
            this._topic = "";
            this._description = "";
            this._imagePath = null;
            this._image = null;
            this._content = "";
        }
        public Essay(string title, string topic, string description, string imagePath, BitmapImage image, DateTime updatedAt, DateTime createdAt)
        {
            this._title = title;
            this._topic = topic;
            this._description = description;
            this._imagePath = imagePath;
            this._image = image;
        }


        private int _id;
        private int _userId;
        private string _title;
        private string _topic;
        private string _description;
        private string _imagePath;
        private BitmapImage _image;
        private string _content;
        private DateTime _updatedAt;
        private DateTime _createdAt;
        
        
        
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        
        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }
        
        
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string Topic
        {
            get => _topic;
            set => _topic = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public string ImagePath
        {
            get => _imagePath;
            set => _imagePath = value;
        }

        public BitmapImage Image
        {
            get => _image;
            set => _image = value;
        }

        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

    }
}