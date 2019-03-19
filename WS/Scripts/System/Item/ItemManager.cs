using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace WS
{
    public class ItemManager
    {
        private static ItemManager mInstance;

        public static ItemManager Instance
        {
            get { return mInstance ?? (mInstance = new ItemManager()); }
        }

        private ItemManager()
        {
        }

        private Dictionary<string, int> ItemDic = new Dictionary<string, int>();


        public Hashtable ToHash()
        {
            return Utils.DicToHash(ItemDic);
        }

        public void FromHash(Hashtable hs)
        {
            ItemDic = Utils.HashToDic<string, int>(hs);
        }

        public void CreateNew()
        {
            ItemDic = new Dictionary<string, int>();
        }

        public void AddItem(string id, int num)
        {
            Debug.Log("add item " + id + " " + num);
            if (ItemDic.ContainsKey(id))
            {
                var n = ItemDic[id] + num;
                ItemDic[id] = Mathf.Clamp(n, 0, MasterDataManager.Item.GetData(id).maxNum);
            }
            else
            {
                ItemDic.Add(id, Mathf.Clamp(num, 0, MasterDataManager.Item.GetData(id).maxNum));
            }
        }

        public int GetNum(string iid)
        {
            return ItemDic.ContainsKey(iid) ? ItemDic[iid] : 0;
        }

        public bool HasUnlock(string iid)
        {
            return ItemDic.ContainsKey(iid);
        }

        public Dictionary<string, int> GetItemDic()
        {
            return ItemDic;
        }

        public bool CheckItem(List<SingleItemData> ilist)
        {
            foreach (var data in ilist)
            {
                var need = data.num;
                var has = GetNum(data.id);
                if (has < need) return false;
            }
            return true;
        }
    }
}
