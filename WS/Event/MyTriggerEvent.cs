using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class MyTriggerEvent : MyEvent.MyEvent
    {
        public const string ON_PLAYER_ENTER = "ON_PLAYER_ENTER";
        public const string ON_PLAYER_EXIT = "ON_PLAYER_EXIT";

        public MyTriggerEvent(string t, GameObject d)
            : base(t, d)
        {
        }
    }
}
