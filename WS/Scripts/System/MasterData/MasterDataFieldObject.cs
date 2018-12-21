namespace WS
{
    public class MasterDataFieldObject : MasterDataBase
    {
        public string name;
        public string desc;
        public string type1;
        public string type2;
        public string param1;
        public string param2;
        public string param3;

        public override void Init(string[] strings)
        {
            base.Init(strings);
            name = strings[1];
            desc = strings[2];
            type1 = strings[3];
            type2 = strings[4];
            param1 = strings[5];
            param2 = strings[6];
            param3 = strings[7];
        }
    }
}
