namespace MyEvent
{
    public class MyEvent
    {
        public string type { get; private set; }
        public object data { get; private set; }

        public MyEvent(string t, object d = null)
        {
            this.type = t;
            this.data = d;
        }
    }
}
