using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class PlayerData
    {
        public const int RES_MAX = 999999999;
        public int Level;
        public int Exp;
        public Dictionary<ResourceType, int> resourceDic;

        public int GetResourceNum(ResourceType t)
        {
            return Utils.SafeGet(resourceDic, t, 0);
        }
    }
}
