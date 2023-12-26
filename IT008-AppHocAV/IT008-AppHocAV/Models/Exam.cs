using System;
using System.Security.Cryptography;

namespace IT008_AppHocAV.Models
{
    public class Exam
    {
        public Exam()
        {
            this._userid=0;
            this._id = 0;
            this._level = 0;
            this._score = 0;
        }
        public Exam(int id, int userid, int level, float score, DateTime createtime)
        {
            this._id = id;
            this._userid = userid;
            this._level = level;
            this._score = score;
            this._createdAt = createtime;
        }
        public Exam(int userid, int level, float score)
        {
            this._userid = userid;
            this._level = level;
            this._score = score;
        }

        private int _userid;
        private int _id;
        private int _level;
        private float _score;
        private DateTime _createdAt;
        
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
        public int Level
        {
            get => _level;
            set => _level = value;
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
    }
}