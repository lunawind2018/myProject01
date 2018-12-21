using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class MasterDataManager : MonoBehaviour
    {
        public static MasterDataManager Instance;

        [SerializeField]
        private TextAsset field_object;

        [HideInInspector]
        public Dictionary<int, MasterDataFieldObject> masterDataFieldObjectDic = new Dictionary<int, MasterDataFieldObject>();

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("multi Instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            Init();
        }

        public void Init()
        {
            ReadCsv(field_object, masterDataFieldObjectDic);
        }

        private void ReadCsv<T>(TextAsset t, Dictionary<int, T> dic) where T : MasterDataBase, new()
        {
            var str = t.text;
            var arr = Utils.ConvertCSV(str);
            for (int i = 0; i < arr.Count; i++)
            {
#if UNITY_EDITOR
                try
                {
#endif
                    var strs = (string[])(((ArrayList)arr[i]).ToArray(typeof(string)));
                    T tt = new T();
                    tt.Init(strs);
                    var id = tt.id;
                    dic.Add(id, new T());
#if UNITY_EDITOR
                }
                catch (Exception e)
                {
                    Debug.LogError("parse error " + i + " " + e.Message);
                    throw;
                }
#endif
            }
        }
    }
}
