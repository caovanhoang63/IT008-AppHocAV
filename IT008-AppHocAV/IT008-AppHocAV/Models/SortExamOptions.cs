using System;

namespace IT008_AppHocAV.Models
{
    public class SortExamOptions
    {
        private static int OrderByDateCreate(Models.Exam exam1, Models.Exam exam2)
        {
            return exam1.CreatedAt.CompareTo(exam2.CreatedAt);
        }
        private static int OrderByLevel(Models.Exam exam1, Models.Exam exam2)
        {
            return exam1.Level.CompareTo(exam2.Level);
        }
        private static int OrderByScore(Models.Exam exam1,Models.Exam exam2)
        {
            return exam1.Score.CompareTo(exam2.Score);
        }

        public static Comparison<Exam> Factory(string option)
        {
            switch (option)
            {
                case "Level":
                    return OrderByLevel;
                case "Date created":
                    return OrderByDateCreate;
                case "Score":
                    return OrderByScore;
                default:
                    return null;
            }
        }

    }
}