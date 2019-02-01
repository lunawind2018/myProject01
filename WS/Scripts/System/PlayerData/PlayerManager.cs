using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private PlayerManager()
        {

        }

        public void InitPlayer(PlayerData pd)
        {
            playerData = pd;
            fieldPlayer = Object.Instantiate(Resources.Load<FieldPlayer>("Prefab/Player"));
            fieldPlayer.transform.SetParent(FieldMap.Instance.playerLayer);
            fieldPlayer.SetPosition(playerData.Position);
        }
    }
}