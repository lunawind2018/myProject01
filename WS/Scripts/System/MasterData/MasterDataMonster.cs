using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class MasterDataMonster : MasterDataBase
    {
        public string name;
        public float size;
        public string desc;
        public string desc_dead;
        private string hp;
        public int Hp { get; private set; }
        public int HpGrowth { get; private set; }

        private string mp;
        public int Mp { get; private set; }
        public int MpGrowth { get; private set; }
        private string sp;
        public int Sp { get; private set; }
        public int SpGrowth { get; private set; }

        private string atk;
        public int Atk { get; private set; }
        public int AtkGrowth { get; private set; }
        private string def;
        public int Def { get; private set; }
        public int DefGrowth { get; private set; }

        protected override void Parse()
        {
            var arr = hp.Split('+');
            this.Hp = int.Parse(arr[0]);
            this.HpGrowth = arr.Length > 1 ? int.Parse(arr[1]) : 0;

            arr = mp.Split('+');
            this.Mp = int.Parse(arr[0]);
            this.MpGrowth = arr.Length > 1 ? int.Parse(arr[1]) : 0;

            arr = sp.Split('+');
            this.Sp = int.Parse(arr[0]);
            this.SpGrowth = arr.Length > 1 ? int.Parse(arr[1]) : 0;

            arr = atk.Split('+');
            this.Atk = int.Parse(arr[0]);
            this.AtkGrowth = arr.Length > 1 ? int.Parse(arr[1]) : 0;

            arr = def.Split('+');
            this.Def = int.Parse(arr[0]);
            this.DefGrowth = arr.Length > 1 ? int.Parse(arr[1]) : 0;
        }
    }
}