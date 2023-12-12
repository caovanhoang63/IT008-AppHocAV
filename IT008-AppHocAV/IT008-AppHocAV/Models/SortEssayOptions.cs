using System;

namespace IT008_AppHocAV.Models
{
    public class SortEssayOptions
    {
        private static int OrderByTitle(Essay essay1, Essay essay2)
        {
            return String.Compare(essay1.Title, essay2.Title, StringComparison.Ordinal);
        }

        private static int OrderByDateModified(Essay essay1, Essay essay2)
        {
            return essay1.UpdatedAt.CompareTo(essay2.UpdatedAt);
        }

        private static int OrderByDateCreate(Essay essay1, Essay essay2)
        {
            return essay1.CreatedAt.CompareTo(essay2.CreatedAt);
        }

        private static int OrderBySize(Essay essay1, Essay essay2)
        {
            return essay1.Words.CompareTo(essay2.Words);
        }

        public static Comparison<Essay> Factory(string option)
        {
            switch (option)
            {
                case "Title":
                    return OrderByTitle; 
                case "Date modified":
                    return OrderByDateModified;
                case "Date created":
                    return OrderByDateCreate;
                case "Words":
                    return OrderBySize;
                default:
                    return null;
            }
        }
        
    }
}