using System;

namespace IT008_AppHocAV.Models
{
    public class VocabularyRecallLog
    {

        public VocabularyRecallLog(int userId)
        {
            UserId = userId;
            Word = "";
            Meaning = "";
            IsSuccessful = false;
            Example = "";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public VocabularyRecallLog()
        {
            
        }
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        
        public int? DefinitionId { get; set; }
        public bool IsSuccessful { get; set; }
        public string Example { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}