
namespace WS
{
    public class MyKeyEvent : MyEvent.MyEvent
    {
        public const string KEY_DOWN = "KEY_DOWN";
        public const string KEY_UP = "KEY_UP";
        public MyKeyEvent(string t, object d = null)
            : base(t, d)
        {
        }
    }
}
