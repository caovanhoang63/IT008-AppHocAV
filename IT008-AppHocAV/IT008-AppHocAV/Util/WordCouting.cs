using System;

namespace IT008_AppHocAV.Util
{
    public class WordCouting
    {
        public static int WordCount(string s)
        {
            return s.Split(new char[] {' '}, 
                StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}