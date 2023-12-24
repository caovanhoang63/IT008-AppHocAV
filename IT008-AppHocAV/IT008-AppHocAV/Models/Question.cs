using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Models
{

    public class Question
    {
        public Question()
        {
            this._content = "";
            this._answera = "";
            this._answerb = "";
            this._answerc = "";
            this._answerd = "";
            this._correct = "";

        }
        public Question(string content, string answera, string answerb, string answerc, string answerd, string correct, DateTime updatedAt, DateTime createdAt)
        {

            this._content = content;
            this._answera = answera;
            this._answerb = answerb;
            this._answerc = answerc;
            this._answerd = answerd;
            this._correct = correct;
            this._updatedAt = updatedAt;
            this._createdAt = createdAt;
        }

        public Question(int id, string content, string answera, string answerb, string answerc, string answerd, string correct, DateTime updatedAt, DateTime createdAt)
        {
            this.Id = id;
            this._content = content;
            this._answera = answera;
            this._answerb = answerb;
            this._answerc = answerc;
            this._answerd = answerd;
            this._correct = correct;
            this._updatedAt = updatedAt;
            this._createdAt = createdAt;
        }



        private int _id;
        private string _content;
        private string _answera;
        private string _answerb;
        private string _answerc;
        private string _answerd;
        private string _correct;

        private string _explain;
        private int _category_id;
        private int level;
        private DateTime _updatedAt;
        private DateTime _createdAt;



        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string content
        {
            get => _content;
            set => content = value;
        }


        public string Answersa
        {
            get => _answera;
            set => _answera = value;
        }

        public string Answersb
        {
            get => _answerb;
            set => _answerb = value;
        }
        public string Answersc
        {
            get => _answerc;
            set => _answerc = value;
        }
        public string Answersd
        {
            get => _answerd;
            set => _answerd = value;
        }

        public string Correct
        {
            get => _correct;
            set => _correct = value;

        }
        public int CategoryId
        {
            get=> _category_id;
            set => _category_id = value;
        }
        public int Level
        {
            get=> level;
            set => level = value;
        }
        public string Explain
        {
            get => _explain;
            set => _explain = value;
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
