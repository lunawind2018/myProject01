using System.Collections;
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
            
        }


        public PlayerData playerData { get; private set; }
        private void LoadFromData(byte[] data)
        {
            var dataHash = MyMsgPacker.Unpack(data) as Hashtable;
            playerData = new PlayerData();
            playerData.FromHashTable(dataHash["player_data"]as Hashtable);
        }

        private void NewSaveData()
        {
            playerData = new PlayerData();
            playerData.CreateNew();
            SaveSaveData();
        }

        public void SaveSaveData()
        {
            var savedatahash = new Hashtable();
            savedatahash.Add("player_data",playerData.ToHashtable());
            var databytes = MyMsgPacker.Pack(savedatahash);
            File.WriteAllBytes(SaveFilePath,databytes);
            Debug.Log("===save " + SaveFilePath);
        }

    }
}
