using System;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Models
{
    public class FlashCard
    {
        private int _id;
        private int _deskId;
        private string _question;
        private string _answer;
        private string _imagePath;
        private BitmapImage _image;
        private DateTime _updatedAt;
        private DateTime _createdAt;
        public FlashCard()
        {
            
            this._question="";
            this._answer ="";
            this._imagePath="";
            this._image=null;

        }
        public FlashCard( string question, string answer, string imagePath, BitmapImage image,  DateTime updatedAt, DateTime createdAt)
        {
            
            _question=question;
            _answer=answer;
            _imagePath=imagePath;
            Image=image;
            UpdatedAt=updatedAt;
            CreatedAt=createdAt;
           
        }
        public FlashCard(int id, int deskId, string question, string answer,  BitmapImage image, DateTime updatedAt, DateTime createdAt )
        {
            Id=id;
            DeskId=deskId;
            _question=question;
            _answer=answer;
            
            Image=image;
            UpdatedAt=updatedAt;
            CreatedAt=createdAt;
    
        }
        public FlashCard(int id, int deskId, string question, string answer,   DateTime updatedAt, DateTime createdAt)
        {
            Id=id;
            DeskId=deskId;
            _question=question;
            _answer=answer;
            
            UpdatedAt=updatedAt;
            CreatedAt=createdAt;

        }
        public FlashCard(string question, string answer)
        {
            _question=question;
            _answer=answer;
            this._imagePath="";
            this._image=null;
        }


        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int DeskId
        {
            get => _deskId;
            set => _deskId = value;
        }
        public string Question
        {
            get => _question;
            set => _question = value;
        }
        public string Answer
        {
            get => _answer;
            set => _answer = value; 
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