namespace WS
{
    public class MasterDataBase
    {
        public int id;

        public virtual void Init(string[] strings)
        {
            id = int.Parse(strings[0]);
        }
    }
}
