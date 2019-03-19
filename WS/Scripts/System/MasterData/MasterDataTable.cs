using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WS
{
    public class MasterDataTable<T> where T : MasterDataBase, new()
    {
        protected Dictionary<string, T> dataDic = new Dictionary<string, T>();

        public void Init(TextAsset csv)
        {
            ReadCsv(csv, dataDic);
        }

        private void ReadCsv(TextAsset t, Dictionary<string, T> dic)
        {
            var str = t.text;
            var arr = Utils.ConvertCSV(str);
            if (arr.Count < 1)
            {
                Debug.LogError("csv error " + typeof(T));
                return;
            }
            var fields = (string[])((ArrayList)arr[0]).ToArray(typeof(string));
            for (int i = 1; i < arr.Count; i++)
            {
#if UNITY_EDITOR
                try
                {
#endif
                    var strs = (string[])((ArrayList)arr[i]).ToArray(typeof(string));
                    T tt = new T();
                    tt.Init(fields, strs);
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
        public T GetData(int id)
        {
#if UNITY_EDITOR
            if (!dataDic.ContainsKey(id + ""))
            {
                Debug.LogError("no data " + id);
                return null;
            }
#endif
            return dataDic[id + ""];
        }

        public T GetData(string id)
        {
#if UNITY_EDITOR
            if (!dataDic.ContainsKey(id))
            {
                Debug.LogError("no data " + id);
                return null;
            }
#endif
            return dataDic[id];
        }

        public List<string> GetIds()
        {
            return dataDic.Keys.ToList();
        }

        public List<T> GetAll()
        {
            return dataDic.Values.ToList();
        } 


    }
}
