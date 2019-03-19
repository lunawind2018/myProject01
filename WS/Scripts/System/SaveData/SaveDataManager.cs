using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace WS
{
    public class SaveDataManager
    {
        public const string SaveFile = "savedata.dat";


        private static SaveDataManager mInstance;
        public static SaveDataManager Instance
        {
            get { return mInstance ?? (mInstance = new SaveDataManager()); }
        }

        public string SaveFilePath { get; private set; }
        private SaveDataManager()
        {
            SaveFilePath = Application.persistentDataPath + "/" + SaveFile;
        }

        public IEnumerator Init()
        {
            var webrequest = UnityWebRequest.Get(SaveFilePath);
            yield return webrequest.Send();
            if (webrequest.isNetworkError)
            {
                // no data
                Debug.Log("===new data");
                NewSaveData();
            }
            else
            {
                Debug.Log("===load data");
                var datas = webrequest.downloadHandler.data;
                if (datas != null)
                {
                    LoadFromData(datas);
                }
                else
                {
                    Debug.LogError("???");
                    NewSaveData();
                }
            }
            yield return 0;

        }

        private const string KEY_PLAYER_DATA = "player_data";
        private const string KEY_ITEM_Data = "item_data";

        public Hashtable dataHash;
        private void LoadFromData(byte[] data)
        {
//            try
//            {
                dataHash = MyMsgPacker.Unpack(data) as Hashtable;
                PlayerManager.Instance.SetPlayerData(dataHash[KEY_PLAYER_DATA] as Hashtable);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("load error " + e.Message);
//            }
        }

        private void NewSaveData()
        {
            PlayerManager.Instance.CreateNew();
            SaveSaveData();
        }

        public void SaveSaveData(System.Action callback = null)
        {
            GameManager.Instance.StartCoroutine(SaveDataCo(callback));

        }

        private IEnumerator SaveDataCo(System.Action callback)
        {
            var savedatahash = new Hashtable();
            savedatahash.Add(KEY_PLAYER_DATA, PlayerManager.Instance.GetPlayerDataHash());
            var databytes = MyMsgPacker.Pack(savedatahash);
            File.WriteAllBytes(SaveFilePath, databytes);
            Debug.Log("===save " + SaveFilePath);
            yield return 0;
            if (callback != null) callback.Invoke();
        }
    }
}
