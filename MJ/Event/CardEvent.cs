using System.Collections;
using System.Collections.Generic;

namespace MJ
{
    public class CardEvent : MyEvent.MyEvent
    {
        public const string CARD_CLICK = "card_click";
        public const string PLAY_CARD = "play_card";
        public const string UI_UPDATE_HAND_CARD = "UI_UPDATE_HAND_CARD";
        public const string UI_UPDATE_DRAW_CARD = "UI_UPDATE_DRAW_CARD";

        public CardEvent(string t, object d) : base(t, d)
        {
        }

        public class CardData
        {
            public int index { get; private set; }
            public List<Card> cards { get; private set; }

            public CardData(int i, Card c)
            {
                index = i;
                cards = new List<Card>(){c};
            }

            public CardData(int i, List<Card> c)
            {
                index = i;
                cards = c;
            }
        }
    }
}
