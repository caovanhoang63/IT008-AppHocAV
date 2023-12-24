using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_AppHocAV.Models
{
    public class SortCardOptions
    {
        private static int OrderByTitle(ListFlashCard card1, ListFlashCard card2)
        {
            return String.Compare(card1.Title, card2.Title, StringComparison.Ordinal);
        }

        private static int OrderByDateModified(ListFlashCard card1, ListFlashCard card2)
        {
            return card1.UpdatedAt.CompareTo(card2.UpdatedAt);
        }

        private static int OrderByDateCreate(ListFlashCard card1, ListFlashCard card2)
        {
            return card1.CreatedAt.CompareTo(card2.CreatedAt);
        }

      

        public static Comparison<ListFlashCard> Factory(string option)
        {
            switch (option)
            {
                case "Title":
                    return OrderByTitle;
                case "Date modified":
                    return OrderByDateModified;
                case "Date created":
                    return OrderByDateCreate;
                
                default:
                    return null;
            }
        }
    }
}
