namespace IT008_AppHocAV.Models
{
    public class Pair<T,TU>
    {
        public Pair()
        {
            
        }

        public Pair(T first,TU second)
        {
            this.First = first;
            this.Second = second;
        }

        public TU this[T first]
        {
            get => Second;
            set => Second = value;
        } 
        public T First { get; set; }
        public TU Second { get; set; }
    }
}