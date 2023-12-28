using System.Collections.Generic;

namespace IT008_AppHocAV.Models
{
    public class DictionaryEntry
    {
        public int id { get; set; }
        public string word { get; set; } 
        public string phonetic{ get; set; } 
        public string origin{ get; set; } 
        public List<Phonetic> phonetics{ get; set; } 
        public List <Meaning> meanings { get; set; }
        public string[] sourceUrls { get; set; }
    }

    public class Meaning
    {
        public int id { get; set; }
        public int partOfSpeechId { get; set; }
        public string partOfSpeech{ get; set; } 
        public  List<Text> synonyms{ get; set; } 
        public List<Text> antonyms{ get; set; } 
    
        public List<Definition> definitions { get; set; }
    
    }

    public class Definition
    {
        public int id { get; set; }
        public string definition { get; set; }
        public string example{ get; set; } 
        public  List<Text> synonyms{ get; set; } 
        public List<Text> antonyms{ get; set; } 
    }

    public class Phonetic
    {
        public int id { get; set; }
        public Phonetic(string text, string audio)
        {
            this.text = text;
            this.audio = audio;
        }
        
        public Phonetic(){}

        public string text { get; set; }
        public string audio { get; set; }
    }

    public class Text
    {

        public Text(string text)
        {
            this.text = text;
        }
        public string text { get; set; }

    }
    
    
}