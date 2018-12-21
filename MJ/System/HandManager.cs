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

        private List<Card> playCardList = new List<Card>();

        private int[] cardArray;

        private Card lastCard;


        public HandManager(int i)
        {
            index = i;
            cardArray = new int[Define.C_ARR_MAX];
            chiDic = new Dictionary<int, int>();
        }

        public Card Think()
        {
            var result = cardList[0];
            return result;
        }


        public void Reset()
        {
            cardList.Clear();
            playCardList.Clear();
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
            //Debug.Log("===[hand] add card " + i + "/" + cardList.Count + " " + c.cName + " " + c.index);
        }

        public Card PlayCard(Card c = null)
        {
            if (c == null) c = lastCard;
            int i = cardList.IndexOf(c);
            if (i < 0)
            {
                Debug.LogError("??? no card in hand: " + c.index + " " + c.cName);
                i = 0;
            }
            cardList.RemoveAt(i);
            cardArray[c.cindex]--;
            playCardList.Add(c);
            MyEventSystem.SendEvent(new CardEvent(CardEvent.PLAY_CARD, new CardEvent.CardData(this.index, c)));
            MyEventSystem.SendEvent(new CardEvent(CardEvent.UI_UPDATE_HAND_CARD, new CardEvent.CardData(this.index, cardList)));
            return c;
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

        public List<Card> GetPlayCardList()
        {
            return playCardList;
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
        //
        private Dictionary<int, int> chiDic;

        public int CheckNaki(Card card, bool canChi)
        {
            chiDic.Clear();
            int result = 0;
            var carr = cardArray;
            var cindex = card.cindex;
            if (carr[cindex] == 3)
            {
                //kang
                result |= 4;
                Debug.Log("Kang");
            }
            else if (carr[cindex] == 2)
            {
                //peng
                result |= 2;
                Debug.Log("Peng");
            }
            if (canChi)
            {
                //chi
                var a = cindex >= 2 ? carr[cindex - 2] : 0;
                var b = cindex >= 1 ? carr[cindex - 1] : 0;
                var c = cindex <= 27 ? carr[cindex + 1] : 0;
                var d = cindex <= 26 ? carr[cindex + 2] : 0;
                if (a > 0 && b > 0)
                {
                    //12+3
                    chiDic.Add(cindex - 2, cindex - 1);
                    chiDic.Add(cindex - 1, cindex - 2);
                    Debug.Log((cindex - 2) + " " + (cindex - 1));
                }
                if (c > 0 && d > 0)
                {
                    //45+3
                    chiDic.Add(cindex + 2, cindex + 1);
                    chiDic.Add(cindex + 1, cindex + 2);
                    Debug.Log((cindex + 2) + " " + (cindex + 1));
                }
                if (b > 0 && c > 0)
                {
                    //24+3
                    SafeAdd(chiDic, cindex - 1, cindex + 1);
                    SafeAdd(chiDic, cindex + 1, cindex - 1);
                    Debug.Log((cindex - 1) + " " + (cindex + 1));
                }
                if (chiDic.Count > 0)
                {
                    result |= 1;
                    Debug.Log("Chi " + System.Convert.ToString(result, 2));
                }
            }
            return result;
        }
        private void SafeAdd<T, K>(Dictionary<T, K> dic, T k, K v)
        {
            if (dic.ContainsKey(k))
            {
                dic[k] = v;
            }
            else
            {
                dic.Add(k, v);
            }
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
