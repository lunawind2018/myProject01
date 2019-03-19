using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    

    public class PlayerData
    {
        private const string KEY_NAME = "name";
        public string playerName = "";

        private const string KEY_POS = "pos";
        public Vector3 Position = new Vector3(2500, 2500, 0);

        private const string KEY_ROTATION = "rotation";
        public Vector3 Rotation = Vector3.zero;

        private const string KEY_ITEM = "item";
        public ItemManager Item = ItemManager.Instance;

        private const string KEY_RECIPE = "recipe";
        private List<string> unlockRecipe = new List<string>();

        private const string KEY_PROPERTY = "property";
        public CharProperty property { get; private set; }

        public PlayerData()
        {
            property = new CharProperty
            {
                Level = 1,
                MaxLevel = 999,
                Exp = 0,
                MaxExp = 10,
                Hp = 100,
                MaxHp = 100,
                Atk = 1,
                Def=0,
            };
        }

        public Hashtable ToHashtable()
        {
            var result = new Hashtable
            {
                {KEY_NAME,playerName},
                {KEY_PROPERTY,property.ToArray()},
                {KEY_POS, new int[]{(int)Position.x,(int)Position.y,(int)Position.z}},
                {KEY_ROTATION,new int[]{(int)Rotation.x,(int)Rotation.y,(int)Rotation.z}},
                {KEY_ITEM,Item.ToHash()},
                {KEY_RECIPE,unlockRecipe.ToArray()},
            };
            return result;
        }

        public void FromHashTable(Hashtable table)
        {
            if (table.ContainsKey(KEY_NAME))
            {
                playerName = table[KEY_NAME].ToString();
            }
            if (table.ContainsKey(KEY_PROPERTY))
            {
                property.FromHashTable(table);
            }
            if (table.ContainsKey(KEY_POS))
            {
                var posarr = table[KEY_POS] as object[];
                Position = new Vector3(int.Parse(posarr[0].ToString()), int.Parse(posarr[1].ToString()), 0);
            }
            if (table.ContainsKey(KEY_ROTATION))
            {
                var posarr = table[KEY_ROTATION] as object[];
                Rotation = new Vector3(int.Parse(posarr[0].ToString()), int.Parse(posarr[1].ToString()), int.Parse(posarr[2].ToString()));
            }
            if (table.ContainsKey(KEY_ITEM))
            {
                Item.FromHash(table[KEY_ITEM] as Hashtable);
            }
            if (table.ContainsKey(KEY_RECIPE))
            {
                this.unlockRecipe = new List<string>();
                var arr = table[KEY_RECIPE] as object[];
                foreach (var o in arr)
                {
                    this.unlockRecipe.Add(o.ToString());
                }
            }
        }

        public void UnlockRecipe(string id)
        {
            if (unlockRecipe.Contains(id)) return;
            unlockRecipe.Add(id);
        }

        public bool HasUnlockRecipe(string id)
        {
            return unlockRecipe.Contains(id);
        }
    }
    public struct CharProperty
    {
        public const string KEY = "property";
        public const int LEN = 12;

        public int Level;
        public int MaxLevel;

        public int Exp;
        public int MaxExp;

        public int Hp;
        public int MaxHp;
        public int Mp;
        public int MaxMp;
        public int Sp;
        public int MaxSp;

        public int Atk;
        public int Def;

        public int HpReg;
        public int MpReg;
        public int SpReg;

        public Array ToArray()
        {
            var parr = new int[LEN];
            parr[0] = Level;
            parr[1] = MaxLevel;
            parr[2] = Exp;
            parr[3] = MaxExp;
            parr[4] = Hp;
            parr[5] = MaxHp;
            parr[6] = Mp;
            parr[7] = MaxMp;
            parr[8] = Sp;
            parr[9] = MaxSp;
            parr[10] = Atk;
            parr[11] = Def;
            parr[12] = HpReg;
            parr[13] = MpReg;
            parr[14] = SpReg;

            return parr;
        }

        public void FromHashTable(Hashtable ht)
        {
#if UNITY_EDITOR
            if (!ht.ContainsKey(KEY))
            {
                Debug.LogError("no property");
                return;
            }
#endif
            Debug.Log(ht[KEY].GetType());
            var tarr = ht[KEY] as Array;
#if UNITY_EDITOR
            if (tarr == null)
            {
                Debug.LogError("parse error");
                return;
            }
#endif
            var parr = new int[LEN];
            Array.Copy(tarr, parr, tarr.Length);
            Level = parr[0];
            MaxLevel = parr[1];
            Exp = parr[2];
            MaxExp = parr[3];
            Hp = parr[4];
            MaxHp = parr[5];
            Mp = parr[6];
            MaxMp = parr[7];
            Sp = parr[8];
            MaxSp = parr[9];
            Atk = parr[10];
            Def = parr[11];
        }

    }
}
