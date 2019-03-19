using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvent
{
    public class MyGameEvent : MyEvent
    {
        public const string MAP_OK = "map_ok";
        public const string GLOBAL_MESSAGE = "global_message";
        public const string HINT_MESSAGE = "hint_message";

        public const string UICRAFT_CLICK_RECIPE = "uicraft_click_recipe";

        public MyGameEvent(string t, object d = null) : base(t, d)
        {
        }
    }
}