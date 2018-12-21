using System.Collections;
using UnityEngine;

namespace MJ
{
    public class Utils
    {
        public static void AddChild(Transform parent, MonoBehaviour child)
        {
            child.transform.SetParent(parent);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
            child.transform.localRotation = Quaternion.identity;
        }

        public static void SetX(Transform tran, float x)
        {
            var t = tran.localPosition;
            t.x = x;
            tran.localPosition = t;
        }

        public static void SetX(MonoBehaviour m, float x)
        {
            SetX(m.transform, x);
        }

        public static void SetGlobalX(Transform tran, float x)
        {
            var t = tran.position;
            t.x = x;
            tran.position = t;
        }

        public static void DestroyChildren(Transform parent)
        {
            if (parent.childCount > 0)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Object.Destroy(parent.GetChild(i).gameObject);
                }
            }
        }
    }
}