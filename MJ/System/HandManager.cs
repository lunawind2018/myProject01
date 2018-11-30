using System.Collections;
using System.Collections.Generic;
using System.Text;
using MyEvent;
using UnityEditor;
using UnityEngine;

namespace MJ
{
    public class HandManager
    {
        private int index;//0-player 1-right 2-front 3-left

        private List<Card> cardList = new List<Card>();

        private List<Naki> nakiCardList = new List<Naki>();

        private int[] cardArray;

        private Card lastCard;


        public HandManager()
        {
            cardArray = new int[Define.C_ARR_MAX];
        }

        public Card Think()
        {
            return cardList[0];
        }

        public void Reset()
        {
            cardList.Clear();
            for (int i = 0; i < cardArray.Length; i++)
            {
                cardArray[i] = 0;
            }
        }

        public void AddCard(Card c)
        {
            int l = cardList.Count;
            int i = 0;
            for (i = 0; i < l; i++)
            {
                if (cardList[i].index > c.index) break;
            }
            cardList.Insert(i, c);
            cardArray[c.cindex]++;
            lastCard = c;
            Debug.Log("===[hand] add card " + i + "/" + cardList.Count + " " + c.cName + " " + c.index);
            MyEventSystem.SendEvent(new CardEvent(CardEvent.CARD_ADD, this));
        }

        public void PlayCard(Card c)
        {
            int i = cardList.IndexOf(c);
            if (i < 0)
            {
                Debug.LogError("??? no card in hand: " + c.index + " " + c.cName);
                i = 0;
            }
//            int l = cardList.Count;
//            for (i = 0; i < l; i++)
//            {
//                if (cardList[i].index == c.index) break;
//            }

            cardList.RemoveAt(i);
            cardArray[c.cindex]--;
            Debug.Log("===[hand] play card " + i + " " + c.cName);
        }

        private int GetCardNum()
        {
            return cardList.Count + nakiCardList.Count * 3;
        }

        public bool CanDrawCard()
        {
            return GetCardNum() <= Define.HAND_CARD_NUM;
        }

        public bool CanPlayCard()
        {
            return GetCardNum() == Define.HAND_CARD_NUM + 1;
        }

        public List<Card> GetHandCardList()
        {
            return cardList;
        }

        public int[] GetCardArray()
        {
            return cardArray;
        }
        public string GetCardArrayStr()
        {
            var sb = new StringBuilder();
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    sb.Append(cardArray[i + j * 10]);
                    sb.Append(" ");
                }
                sb.Append("\n");
            }
            for (int i = 0; i < 4; i++)
            {
                sb.Append(cardArray[i*2 + 30]);
                sb.Append(" ");
            }
            sb.Append("\n");
            for (int i = 0; i < 3; i++)
            {
                sb.Append(cardArray[i*2 + 38]);
                sb.Append(" ");
            }
            return sb.ToString();
        }

    }

    public enum NakiType
    {
        Peng,
        Chi,
        Kang,
        AnKang,
    }

    public class Naki
    {
        public NakiType type { get; private set; }
        public Card[] cards;
        public int owner;//0-self 1-left 2-front 3-right

        public Naki(List<Card> cardList,Card income,int o)
        {
            this.cards = new Card[4];
            this.cards[0] = income;
            this.cards[1] = cardList[0];
            this.cards[2] = cardList[1];
            this.owner = o;
            if (cardList.Count == 3)
            {
                this.cards[3] = cardList[2];
                this.type = owner == 0 ? NakiType.AnKang : NakiType.Kang;
            }
            else
            {
                this.type = cards[0].cindex == cards[1].cindex ? NakiType.Peng : NakiType.Chi;
            }
        }
    }
}
