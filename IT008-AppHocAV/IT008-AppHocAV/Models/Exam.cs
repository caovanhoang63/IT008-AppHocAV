using System.Security.Cryptography;

namespace IT008_AppHocAV.Models
{
    public class Exam
    {
        public Exam()
        {
            this._id = 0;
            this._level = 0;
            this._score = 0;
            this._createtime = "";
        }
        public Exam(int id, int level, float score, string createtime)
        {
            this._id = id;
            this._level = level;
            this._score = score;
            this._createtime = createtime;
        }

        private int _id;
        private int _level;
        private float _score;
        private string _createtime;
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
        public string Createtime
        {
            get => _createtime;
            set => _createtime = value;
        }
    }
}