using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WS
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        public FieldPlayer fieldPlayer { get; private set; }
        public PlayerData playerData { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("multi Instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        public void InitPlayer()
        {
            fieldPlayer = Object.Instantiate(Resources.Load<FieldPlayer>("Prefab/Player"));
            fieldPlayer.transform.SetParent(FieldMap.Instance.playerLayer);
            fieldPlayer.SetPosition(playerData.Position);
            fieldPlayer.SetRotation(playerData.Rotation);
        }

        public void CreateNew()
        {
            playerData = new PlayerData();
        }

        public void SetPlayerData(Hashtable pd)
        {
            if (playerData == null) playerData = new PlayerData();
            playerData.FromHashTable(pd);
        }

        public Hashtable GetPlayerDataHash()
        {
            if (fieldPlayer != null)
            {
                playerData.Position = fieldPlayer.GetPosition();
                playerData.Rotation = fieldPlayer.GetRotation();
            }
            return playerData.ToHashtable();
        }

        public List<SingleItemData> Reward(List<DropItemData> dropItemDatas)
        {
            var l = dropItemDatas.Count;
            var random = new System.Random();
            var tempdic = new Dictionary<string, SingleItemData>();
            for (int i = 0; i < l; i++)
            {
                var d = dropItemDatas[i];
                var r = random.Next(0, 100);
                //Debug.Log("r " + r + "/" + d.weight);
                if (r < d.weight)
                {
                    if (tempdic.ContainsKey(d.id))
                    {
                        tempdic[d.id].num += d.num;
                    }
                    else
                    {
                        tempdic.Add(d.id, new SingleItemData(d.id, d.num));
                    }
                    ItemManager.Instance.AddItem(d.id, d.num);
                }
            }
            var result = tempdic.Values.ToList();
            return result;
        }
        public void UnlockRecipe(string id)
        {
            playerData.UnlockRecipe(id);
        }
        
    }
}