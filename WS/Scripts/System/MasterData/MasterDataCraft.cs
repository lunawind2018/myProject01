using System.Collections.Generic;

namespace WS
{
    public class MasterDataCraft : MasterDataBase
    {
        public int type;
        public int get_num;
        private string material = "";
        public List<SingleItemData> materialItems;
        private string show_recipe = "";
        public List<SingleItemData> showRecipeItems; 
        public bool can_try;
        public int rarity;

        public override void Init(string[] fields, string[] datas)
        {
            base.Init(fields, datas);
            materialItems = Utils.ParseItemData(material);
            showRecipeItems = Utils.ParseItemData(show_recipe);
        }
    }
}
