using System.Collections;
using System.Collections.Generic;
using MyEvent;
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
        public const string TAB = "TAB";
        public const string BAG = "BAG";
        public const string ESC = "ESC";
        public const string CRAFT = "CRAFT";
        public const string CHARACTER = "CHARACTER";
        public const string M_LEFT = "M_LEFT";
        public const string M_RIGHT = "M_RIGHT";

        public static Dictionary<string, KeyCode[]> keyDic = new Dictionary<string, KeyCode[]>()
    {
        {UP,new []{KeyCode.W, KeyCode.UpArrow}},
        {DOWN,new []{KeyCode.S, KeyCode.DownArrow}},
        {LEFT,new []{KeyCode.A, KeyCode.LeftArrow}},
        {RIGHT,new []{KeyCode.D, KeyCode.RightArrow}},
        {INTERACT,new []{KeyCode.E, KeyCode.None}},
        {TAB,new []{KeyCode.Tab, KeyCode.None}},
        {BAG,new []{KeyCode.B, KeyCode.None}},
        {ESC,new []{KeyCode.Escape, KeyCode.None}},
        {CRAFT,new []{KeyCode.O,KeyCode.None}},
        {CHARACTER,new []{KeyCode.C,KeyCode.None}},

    };

        public static Dictionary<KeyCode, string> reverseDic = new Dictionary<KeyCode, string>();

        public static Dictionary<KeyCode, bool> keyDownDic = new Dictionary<KeyCode, bool>();

        public static Dictionary<string, int> keyCountDic = new Dictionary<string, int>();

        private static List<KeyCode> keyList = new List<KeyCode>();

        void Awake()
        {
            Reset();
        }

        private static void Reset()
        {
            reverseDic.Clear();
            keyDownDic.Clear();
            keyList.Clear();
            keyCountDic.Clear();
            foreach (KeyValuePair<string, KeyCode[]> keyValuePair in keyDic)
            {
                keyCountDic.Add(keyValuePair.Key, 0);
                var arr = keyValuePair.Value;
                foreach (var code in arr)
                {
                    if (code == KeyCode.None) continue;
                    reverseDic.Add(code, keyValuePair.Key);
                    keyDownDic.Add(code, false);
                    keyList.Add(code);
                }
            }
        }

        void Update()
        {
            foreach (KeyCode k in keyList)
            {
                if (keyDownDic[k])
                {
                    if (Input.GetKeyUp(k))
                    {
                        keyDownDic[k] = false;
                        keyCountDic[reverseDic[k]]--;
                        //Debug.Log("key up " + k + " " + reverseDic[k] + " " + keyCountDic[reverseDic[k]]);
                        if (keyCountDic[reverseDic[k]] == 0)
                        {
                            MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_UP, reverseDic[k]));
                        }
                    }
                }
            }
//            if (Input.GetMouseButtonDown(0))
//            {
//                //Debug.Log("mouse left");
//                MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_DOWN, M_LEFT));
//            }
//            if (Input.GetMouseButtonDown(1))
//            {
//                //Debug.Log("mouse right");
//                MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_DOWN, M_RIGHT));
//            }
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
                    if (!keyDownDic.ContainsKey(c) || keyDownDic[c]) return;
                    keyDownDic[c] = true;
                    keyCountDic[reverseDic[c]]++;
                    //Debug.Log("key down " + c + " " + reverseDic[c] + " " + keyCountDic[reverseDic[c]]);
                    if (keyCountDic[reverseDic[c]] == 1)
                    {
                        MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_DOWN, reverseDic[c]));
                    }
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
