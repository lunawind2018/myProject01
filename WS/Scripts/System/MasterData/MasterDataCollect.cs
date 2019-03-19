using System.Collections.Generic;

namespace WS
{
    public class MasterDataCollect : MasterDataBase
    {
        public int resource_id;
        public int tool_id;
        public float time;
        private string get_item;
        public List<DropItemData> dropItemData;
        private string get_resource;

        public bool destroy;
        public string message;
        public string message_fail;

        public override void Init(string[] fields, string[] datas)
        {
            base.Init(fields, datas);
            ConvertDropItem();
        }

        private void ConvertDropItem()
        {
            if (string.IsNullOrEmpty(get_item)) return;
            var list = get_item.Split(';');
            dropItemData= new List<DropItemData>();
            for (int i = 0; i < list.Length; i++)
            {
                var str = list[i];
                if (string.IsNullOrEmpty(str)) continue;
                var ss = str.Split(':');
                var data = new DropItemData
                {
                    id = ss[0],
                    num = int.Parse(ss[1]),
                    weight = ss.Length > 2 ? int.Parse(ss[2]) : 100
                };
                dropItemData.Add(data);
            }
        }

    }

    public class MasterDataCollectTable:MasterDataTable<MasterDataCollect>
    {
        public MasterDataCollect GetData(int resource, int tool)
        {
            return GetData(resource*100 + tool);
        }
    }
}
