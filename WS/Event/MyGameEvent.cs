using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvent
{
    public class MyGameEvent : MyEvent
    {
        public const string MAP_OK = "map_ok";

        public MyGameEvent(string t, object d = null) : base(t, d)
        {
        }
    }
}