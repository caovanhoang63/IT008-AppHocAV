using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Utilities.Collections;

namespace IT008_AppHocAV.Models
{
    public class Exam
    {
        public Exam()
        {
            this._userid = 0;
            this._id = 0;
            this._level = "";
            this._score = 0;
        }
        public Exam(int id, int userid, int level, string cate, float score, DateTime createtime, int timetaken)
        {
            this._id = id;
            this._userid = userid;
            this._level = ConvertLevel(level);
            this._category = cate;
            this._score = score;
            this._createdAt = createtime;
            this._timeTaken = ConvertTime(timetaken);
        }


        public Exam(int userid, int level, string cate, float score, int timetaken)
        {
            this._userid = userid;
            this._level = level.ToString();
            this._category = cate;
            this._score = score;
            this._timeTaken = timetaken.ToString();
        }

        private int _userid;
        private int _id;
        private string _level;
        private string _category;
        private float _score;
        private DateTime _createdAt;
        private string _timeTaken;

        public int Userid
        {
            get => _userid;
            set => _userid = value;
        }
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public string Level
        {
            get => _level;
            set => _level = value;
        }
        public string Category
        {
            get => _category;
            set => _category = value;
        }
        public float Score
        {
            get => _score;
            set => _score = value;
        }
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }
        public string TimeTaken
        {
            get => _timeTaken;
            set => _timeTaken = value;
        }

        private string ConvertTime(int time)
        {
            TimeSpan ts = TimeSpan.FromSeconds(time);
            string FormatedTime = ts.ToString(@"mm\:ss");
            return FormatedTime;
        }
        private string ConvertLevel(int level)
        {
            string lvl = "";
            switch (level) 
            {
                case 1:
                    lvl = "Easy";
                    break;
                case 2:
                    lvl = "Advanced";
                    break;
            }
            return lvl;
        }
    }
}