using System;
using System.Collections;
using System.Collections.Generic;
using MyEvent;
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
            RegisterEvent();
            //
//            try
//            {
                //Test();
//            }
//            catch (Exception e)
//            {
//                Debug.LogError(e.Message);
//            }
        }

        void OnDestroy()
        {
            UnRegisterEvent();
        }

        void Start()
        {
            StartCoroutine(InitGameManager());
        }

        private void Test()
        {
            var ttt = new int[100];
            for (int i = 0; i < 100; i++) ttt[i] = i;
            var ttt2 = MyMsgPacker.Pack(ttt);
            var ttt3 = MyMsgPacker.Unpack(ttt2);
            Debug.Log(ttt3.GetType()+" ");
//            var aa = new Hashtable();
//            aa.Add("a1", 1);
//            aa.Add("b1", "bb");
//            aa.Add("c1", 3);
//            var a = MyMsgPacker.Pack(aa);
//            var b = MyMsgPacker.Unpack(a);
//            Debug.Log(b + " " + b.GetType());
//            var h = (Hashtable) b;
//            Debug.Log(h["a1"] + " " + h["b1"] + " " + h["c1"]);
        }

        public SaveDataManager saveDataManager { get; private set; }
        public CommandManager commandManager { get; private set; }
        public PlayerManager playerManager { get; private set; }
        public MasterDataManager masterDataManager { get; private set; }

        private IEnumerator InitGameManager()
        {
            masterDataManager = MasterDataManager.Instance;
            yield return StartCoroutine(masterDataManager.Init());
            Debug.Log("===masterDataManager Inited");
            //
            playerManager = PlayerManager.Instance;
            Debug.Log("===playManager Inited");
            //
            saveDataManager = SaveDataManager.Instance;
            yield return StartCoroutine(saveDataManager.Init());
            //
            playerManager.InitPlayer();

            commandManager = CommandManager.Instance;
            yield return 0;
        }

        private void RegisterEvent()
        {
            MyEventSystem.RegistEvent(MyGameEvent.MAP_OK,OnMapOkHandler);
        }
        private void UnRegisterEvent()
        {
            MyEventSystem.UnRegistEvent(MyGameEvent.MAP_OK, OnMapOkHandler);
        }

        private void OnMapOkHandler(MyEvent.MyEvent obj)
        {
        }

        public void SendCommand(CommandType command, object param, System.Action<bool> callback )
        {
            StartCoroutine(commandManager.DoCommand(command, param, callback));
        }
    }
}
