using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{


    public class KeyControl : MonoBehaviour
    {
        public const string UP = "UP";
        public const string DOWN = "DOWN";
        public const string LEFT = "LEFT";
        public const string RIGHT = "RIGHT";
        public const string INTERACT = "INTERACT";

        public static Dictionary<string, KeyCode[]> keyDic = new Dictionary<string, KeyCode[]>()
    {
        {UP,new []{KeyCode.W, KeyCode.UpArrow}},
        {DOWN,new []{KeyCode.S, KeyCode.DownArrow}},
        {LEFT,new []{KeyCode.A, KeyCode.LeftArrow}},
        {RIGHT,new []{KeyCode.D, KeyCode.RightArrow}},
        {INTERACT,new []{KeyCode.F, KeyCode.None}},
    };

        public static Dictionary<KeyCode, string> reverseDic = new Dictionary<KeyCode, string>();

        public Dictionary<string, bool> keyDownDic = new Dictionary<string, bool>();

        void Awake()
        {
            Reset();
        }

        private static void Reset()
        {
            reverseDic.Clear();
            foreach (KeyValuePair<string, KeyCode[]> keyValuePair in keyDic)
            {
                var arr = keyValuePair.Value;
                foreach (var code in arr)
                {
                    reverseDic.Add(code, keyValuePair.Key);
                }
            }
        }

        void Update()
        {

        }

        void OnGUI()
        {
            if (Input.anyKeyDown)
            {
                var e = Event.current;
                if (e.isKey)
                {
                    var c = e.keyCode;
                    if (c == KeyCode.None) return;
                    Debug.Log("key down " + c);

                }
            }
        }

        public static KeyCode[] GetKey(string keyname)
        {
            return keyDic[keyname];
        }

        public static void SetKey(string keyname, KeyCode k, int index = 0)
        {
            keyDic[keyname][index] = k;
            Reset();
        }
    }
}
