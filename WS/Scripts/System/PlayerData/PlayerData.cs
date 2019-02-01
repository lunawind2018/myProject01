using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class PlayerData
    {
        public const int RES_MAX = 999999999;
        public int Level = 1;
        public int Exp = 0;
        public Dictionary<ResourceType, int> resourceDic;

        public Vector3 Position = new Vector3(2500, 2500, 0);

        public int GetResourceNum(ResourceType t)
        {
            return Utils.SafeGet(resourceDic, t, 0);
        }

        public Hashtable ToHashtable()
        {
            var result = new Hashtable
            {
                {"level", Level}, 
                {"exp", Exp},
                {"pos", new int[]{(int)Position.x,(int)Position.y,(int)Position.z}}
            };
            return result;
        }

        public void FromHashTable(Hashtable table)
        {
            Level = int.Parse(table["level"].ToString());
            Exp = int.Parse(table["exp"].ToString());
            var posarr = table["pos"] as object[];
            Position = new Vector3(int.Parse(posarr[0].ToString()), int.Parse(posarr[1].ToString()), 0);
        }

        public void CreateNew()
        {
        }
    }
}
