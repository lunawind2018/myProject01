using System.Reflection;
using UnityEngine;

namespace WS
{
    public class MasterDataBase
    {
        private const BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        public string id;
        public int intid;

        public virtual void Init(string[]fields, string[] datas)
        {
            //id = int.Parse(strings[0]);
            var t = this.GetType();
            for (int i = 0; i < fields.Length; i++)
            {
                var fname = fields[i];
                var f = t.GetField(fname, flag);
#if UNITY_EDITOR
                if (f == null)
                {
                    UnityEngine.Debug.LogError("no field " + fname);
                }
#endif
                var tt = f.FieldType;
                if (tt == typeof(int))
                {
                    f.SetValue(this, int.Parse(datas[i]));
                }
                else if (tt == typeof(string))
                {
                    f.SetValue(this, datas[i]);
                }
                else if (tt == typeof (float))
                {
                    f.SetValue(this, float.Parse(datas[i]));
                }
                else if (tt == typeof (bool))
                {
                    f.SetValue(this, (int.Parse(datas[i]) > 0));
                }
                else if (tt == typeof (Vector2))
                {
                    var ss = datas[i];
                    if (string.IsNullOrEmpty(ss))
                    {
                        f.SetValue(this,Vector2.zero);
                    }
                    else
                    {
                        var aa = ss.Split(',');
                        var v2 = new Vector2(float.Parse(aa[0]), float.Parse(aa[1]));
                        f.SetValue(this, v2);
                    }
                }
#if UNITY_EDITOR
                else
                {
                    UnityEngine.Debug.LogError("type error: " + fname);
                    f.SetValue(this, datas[i]);
                }
#endif
            }
            this.intid = 0;
            int.TryParse(this.id, out this.intid);

            Parse();
        }

        protected virtual void Parse()
        {
            
        }
    }
}
