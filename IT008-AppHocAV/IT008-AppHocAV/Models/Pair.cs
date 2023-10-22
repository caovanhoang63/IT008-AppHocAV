namespace IT008_AppHocAV.Models
{
    public class Pair<T,U>
    {
        public Pair()
        {
            
        }

        public Pair(T first,U second)
        {
            this.first = first;
            this.second = second;
        }
        public T first { get; set; }
        public U second { get; set; }
    }
}