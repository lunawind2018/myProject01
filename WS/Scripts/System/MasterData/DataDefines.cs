namespace WS
{
    public struct DropItemData
    {
        public string id;
        public int num;
        public int weight;
    }

    public class SingleItemData
    {
        public string id;
        public int num;

        public SingleItemData(string i, int n)
        {
            id = i;
            num = n;
        }
    }
}
