using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvent
{
    public class MyEventSystem
    {
        private static Dictionary<string, List<System.Action<MyEvent>>> eventDic = new Dictionary<string, List<Action<MyEvent>>>();
        public static void RegistEvent(string eventname, System.Action<MyEvent> handler)
        {
            if (eventDic.ContainsKey(eventname))
            {
                var list = eventDic[eventname];
                if (list.Contains(handler))
                {
                    Debug.LogError("same event already added");
                }
                list.Add(handler);
            }
            else
            {
                var list = new List<System.Action<MyEvent>> { handler };
                eventDic.Add(eventname, list);
            }
        }

        public static void UnRegistEvent(string eventname, System.Action<MyEvent> handler)
        {
            if (eventDic.ContainsKey(eventname))
            {
                var list = eventDic[eventname];
                var index = list.IndexOf(handler);
                if (index == -1)
                {
                    Debug.LogError("no event handler: " + eventname);
                }
                else
                {
                    list.RemoveAt(index);
                }
                if (list.Count <= 0)
                {
                    eventDic.Remove(eventname);
                }
            }
            else
            {
                Debug.LogError("no event handler key: " + eventname);
            }
        }

        public static void SendEvent(MyEvent evt)
        {
            var eventname = evt.type;
            if (eventDic.ContainsKey(eventname))
            {
                var list = eventDic[eventname];
                var l = list.Count;
                for (int i = 0; i < l; i++)
                {
                    list[i].Invoke(evt);
                }
            }
        }

    }
}
