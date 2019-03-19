using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class Command_Craft : Command_Base
    {
        public override IEnumerator DoCommand(object param)
        {
            var obj = param as Hashtable;
            if (obj == null)
            {
                Debug.LogError("param error");
                yield break;
            }
            var id = obj["id"].ToString();

            var craftTimes = obj.ContainsKey("nums") ? ((int)obj["nums"]) : 1;
            Debug.Log("craft " + id);
            var mdata = MasterDataManager.Craft.GetData(id);
            if (mdata == null)
            {
                CommandManager.Instance.CommandSuccess = false;
                yield break;
            }
            var items = mdata.materialItems;
            //var canCraft = CheckItems(items);
            foreach (var singleItemData in items)
            {
                ItemManager.Instance.AddItem(singleItemData.id, -singleItemData.num * craftTimes);
            }
            ItemManager.Instance.AddItem(id, mdata.get_num * craftTimes);
            PlayerManager.Instance.UnlockRecipe(id);
        }

        private bool CheckItems(List<SingleItemData> matItems)
        {
            foreach (var singleItemData in matItems)
            {
                var id = singleItemData.id;
                var need = singleItemData.num;
                var has = ItemManager.Instance.GetNum(id);
                if (has < need) return false;
            }
            return true;
        }
    }
}