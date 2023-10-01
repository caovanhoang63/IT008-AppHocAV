using System.Collections.Generic;

namespace IT008_AppHocAV.Models
{
    public class DictionaryEntry
    {
        public string word { get; set; } 
        public string phonetic{ get; set; } 
        public string origin{ get; set; } 
        public List<Phonetic> phonetics{ get; set; } 
        public List <Meaning> meanings { get; set; }
        public string[] sourceUrls { get; set; }
    }

    public class Meaning
    {
        public string partOfSpeech{ get; set; } 
        public string[] synonyms{ get; set; } 
        public string[] antonyms{ get; set; } 
    
        public List<Definition> definitions { get; set; }
    
    }

    public class Definition
    {
        public string definition { get; set; }
        public string example{ get; set; } 
        public string[] synonyms{ get; set; } 
        public string[] antonyms{ get; set; } 
    }

    public class Phonetic
    {
        public string text;
        public string audio;
    }
}