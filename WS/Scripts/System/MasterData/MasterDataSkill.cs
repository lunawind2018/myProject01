using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class MasterDataSkill : MasterDataBase
    {
        public int type;
        public string effect;
        public bool effect_local;
        public Vector2 effect_offset;
        public int target;
        public int attack;
        public int hit;
        public float total_time;
        public float hit_time;
    }
}