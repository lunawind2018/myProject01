using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public partial class Utils
    {
        public static void SetParent(Transform c, Transform p)
        {
            c.SetParent(p);
            c.localPosition = Vector3.zero;
            c.localScale = Vector3.one;
            c.localRotation = Quaternion.identity;
        }
        //==================================================
#region SetN
        public static void SetX(MonoBehaviour obj, float x)
        {
            var t = obj.transform.localPosition;
            t.x = x;
            obj.transform.localPosition = t;
        }
        public static void SetY(MonoBehaviour obj, float y)
        {
            var t = obj.transform.localPosition;
            t.y = y;
            obj.transform.localPosition = t;
        }
        public static void SetZ(MonoBehaviour obj, float z)
        {
            var t = obj.transform.localPosition;
            t.z = z;
            obj.transform.localPosition = t;
        }
        //==================================================
        public static void SetX(Transform obj, float x)
        {
            var t = obj.transform.localPosition;
            t.x = x;
            obj.transform.localPosition = t;
        }
        public static void SetY(Transform obj, float y)
        {
            var t = obj.transform.localPosition;
            t.y = y;
            obj.transform.localPosition = t;
        }
        public static void SetZ(Transform obj, float z)
        {
            var t = obj.transform.localPosition;
            t.z = z;
            obj.transform.localPosition = t;
        }
        //==================================================
        public static void SetX(MonoBehaviour mono, float x, bool islocal)
        {
            if (islocal)
            {
                var t = mono.transform.localPosition;
                t.x = x;
                mono.transform.localPosition = t;
            }
            else
            {
                var t = mono.transform.position;
                t.x = x;
                mono.transform.position = t;
            }
        }
        public static void SetY(MonoBehaviour mono, float y, bool islocal)
        {
            if (islocal)
            {
                var t = mono.transform.localPosition;
                t.y = y;
                mono.transform.localPosition = t;
            }
            else
            {
                var t = mono.transform.position;
                t.y = y;
                mono.transform.position = t;
            }
        }
        public static void SetZ(MonoBehaviour mono, float z, bool islocal)
        {
            if (islocal)
            {
                var t = mono.transform.localPosition;
                t.z = z;
                mono.transform.localPosition = t;
            }
            else
            {
                var t = mono.transform.position;
                t.z = z;
                mono.transform.position = t;
            }
        }
        //==================================================
        public static void SetX(Transform trans, float x, bool islocal)
        {
            if (islocal)
            {
                var t = trans.transform.localPosition;
                t.x = x;
                trans.transform.localPosition = t;
            }
            else
            {
                var t = trans.transform.position;
                t.x = x;
                trans.transform.position = t;
            }
        }
        public static void SetY(Transform trans, float y, bool islocal)
        {
            if (islocal)
            {
                var t = trans.transform.localPosition;
                t.y = y;
                trans.transform.localPosition = t;
            }
            else
            {
                var t = trans.transform.position;
                t.y = y;
                trans.transform.position = t;
            }
        }
        public static void SetZ(Transform trans, float z, bool islocal)
        {
            if (islocal)
            {
                var t = trans.transform.localPosition;
                t.z = z;
                trans.transform.localPosition = t;
            }
            else
            {
                var t = trans.transform.position;
                t.z = z;
                trans.transform.position = t;
            }
        }
#endregion
        //==================================================
#region AddN
        public static void AddX(MonoBehaviour obj, float x)
        {
            var t = obj.transform.localPosition;
            t.x += x;
            obj.transform.localPosition = t;
        }
        public static void AddY(MonoBehaviour obj, float y)
        {
            var t = obj.transform.localPosition;
            t.y += y;
            obj.transform.localPosition = t;
        }
        public static void AddZ(MonoBehaviour obj, float z)
        {
            var t = obj.transform.localPosition;
            t.z += z;
            obj.transform.localPosition = t;
        }
        //==================================================
        public static void AddX(Transform obj, float x)
        {
            var t = obj.transform.localPosition;
            t.x += x;
            obj.transform.localPosition = t;
        }
        public static void AddY(Transform obj, float y)
        {
            var t = obj.transform.localPosition;
            t.y += y;
            obj.transform.localPosition = t;
        }
        public static void AddZ(Transform obj, float z)
        {
            var t = obj.transform.localPosition;
            t.z += z;
            obj.transform.localPosition = t;
        }
        //==================================================
        public static void AddX(MonoBehaviour obj, float x, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.x += x;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.x += x;
                obj.transform.position = t;
            }
        }
        public static void AddY(MonoBehaviour obj, float y, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.y += y;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.y += y;
                obj.transform.position = t;
            }
        }
        public static void AddZ(MonoBehaviour obj, float z, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.z += z;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.z += z;
                obj.transform.position = t;
            }
        }
        //==================================================
        public static void AddX(Transform obj, float x, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.x += x;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.x += x;
                obj.transform.position = t;
            }
        }
        public static void AddY(Transform obj, float y, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.y += y;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.y += y;
                obj.transform.position = t;
            }
        }
        public static void AddZ(Transform obj, float z, bool islocal)
        {
            if (islocal)
            {
                var t = obj.transform.localPosition;
                t.z += z;
                obj.transform.localPosition = t;
            }
            else
            {
                var t = obj.transform.position;
                t.z += z;
                obj.transform.position = t;
            }
        }
#endregion
        //==================================================

    }
}
