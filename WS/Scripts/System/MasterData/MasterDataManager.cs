using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<GameObject, string> testdic = new Dictionary<GameObject, string>();

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

        public IEnumerator Init()
        {
            ReadCsv(field_object, masterDataFieldObjectDic);
            yield return 0;
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
                    dic.Add(id, tt);
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
