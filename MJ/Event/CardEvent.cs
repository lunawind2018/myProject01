using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvent
{
    public class CardEvent : MyEvent
    {
        public const string CARD_CLICK = "card_click";
        public const string CARD_ADD = "card_add";

        public CardEvent(string t, object d) : base(t, d)
        {
        }
    }
}
