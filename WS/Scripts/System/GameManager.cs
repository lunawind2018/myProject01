using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager mInstance;

        public static GameManager Instance
        {
            get { return mInstance; }
        }

        void Awake()
        {
            if (mInstance != null)
            {
                Debug.LogError("multi game manager " + this.name);
                Destroy(this.gameObject);
                return;
            }
            mInstance = this;
            InitGameManager();
        }


        public CommandManager commandManager { get; private set; }
        public PlayerDataManager playerDataManager { get; private set; }


        private void InitGameManager()
        {
            commandManager = CommandManager.Instance;
            playerDataManager = PlayerDataManager.Instance;
        }

        public void SendCommand(CommandType command, Hashtable param)
        {
            commandManager.DoCommand(command, param);
        }
    }
}
