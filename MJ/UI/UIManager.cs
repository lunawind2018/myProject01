using System;
using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace MJ
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private CardComponent cardPrefab;

        [SerializeField]
        private List<Transform> handCardParents;

        [SerializeField]
        private List<Transform> tableCardParents;//0-self 1-left 2-front 3-right

        //
        [SerializeField]
        private Text debugCardArrayText;

        [SerializeField]
        private Text debugText;

        [SerializeField]
        private Transform debugYama;

        //private List<CardComponent> debugYamaCardList; 
        //
        private int playerNum = 4;

        private List<List<CardComponent>> handCardList;
        private List<int> handCardNums;
        private List<CardComponent> drawCard;

        private int[] xIncs = new[] {50, 50, 50, 50};

        void Awake()
        {
        }

        void Start()
        {
            RegisterEvents();
        }

        void OnDestroy()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            MyEventSystem.RegistEvent(CardEvent.PLAY_CARD, OnPlayCardHandler);
            MyEventSystem.RegistEvent(CardEvent.UI_UPDATE_DRAW_CARD, OnUpdateDrawCardHandler);
            MyEventSystem.RegistEvent(CardEvent.UI_UPDATE_HAND_CARD, OnUpdateHandCardsHandler);
        }

        private void UnRegisterEvents()
        {
            MyEventSystem.UnRegistEvent(CardEvent.PLAY_CARD, OnPlayCardHandler);
            MyEventSystem.UnRegistEvent(CardEvent.UI_UPDATE_DRAW_CARD, OnUpdateDrawCardHandler);
            MyEventSystem.UnRegistEvent(CardEvent.UI_UPDATE_HAND_CARD, OnUpdateHandCardsHandler);
        }

        public void Reset(int p)
        {
            this.playerNum = p;

            handCardList = new List<List<CardComponent>>();
            handCardNums = new List<int>();
            for (int i = 0; i < playerNum; i++)
            {
                handCardList.Add(new List<CardComponent>());
                handCardNums.Add(0);
            }
            drawCard = new List<CardComponent>();
            for (int i = 0; i < playerNum; i++)
            {
                drawCard.Add(null);
            }

            //debugYamaCardList = new List<CardComponent>();
            foreach (var parent in tableCardParents)
            {
                Utils.DestroyChildren(parent);
            }
        }

        public void UpdatePlayerNaki()
        {
            
        }

        public void UpdateTableCard(int currentPlayer, Card card)
        {
            var p = tableCardParents[currentPlayer];
            var c = Instantiate(cardPrefab);
            c.Init(card);
            Utils.AddChild(p,c);

        }

        public void ShowDrawCard(int i, Card c)
        {
            if (drawCard[i] == null)
            {
                drawCard[i] = Instantiate(cardPrefab);
                Utils.AddChild(handCardParents[i], drawCard[i]);
                var x = handCardNums[i] * xIncs[i] + 10;
                Utils.SetX(drawCard[i],x);
            }
            drawCard[i].Init(c);
            drawCard[i].SetActive(true);
        }


        private void OnPlayCardHandler(MyEvent.MyEvent obj)
        {
            var data = (obj as CardEvent).data as CardEvent.CardData;
            UpdateTableCard(data.index, data.cards[0]);
        }

        private void OnUpdateHandCardsHandler(MyEvent.MyEvent obj)
        {
            var data = (obj as CardEvent).data as CardEvent.CardData;
            UpdateHandCards(data.index, data.cards);
        }

        private void UpdateHandCards(int index, List<Card> clist)
        {
            if (clist.Count > Define.HAND_CARD_NUM)
            {
                Debug.LogError("??? too many card " + clist.Count);
            }
            var hand = handCardList[index];
            handCardNums[index] = clist.Count;
            for (int i = 0; i < clist.Count; i++)
            {
                if (i >= hand.Count)
                {
                    var newCard = Instantiate(cardPrefab);
                    newCard.gameObject.name = "Card_" + (i + 1);
                    Utils.AddChild(handCardParents[index], newCard);
                    Utils.SetX(newCard, i * xIncs[index]);
                    newCard.Init(clist[i]);
                    //newCard.SetLbl2((i+1).ToString());
                    hand.Add(newCard);
                }
                else
                {
                    var card = hand[i];
                    card.Init(clist[i]);
                    card.SetActive(true);
                }
            }
            for (int i = clist.Count; i < Define.HAND_CARD_NUM; i++)
            {
                if (i < hand.Count)
                {
                    hand[i].SetActive(false);
                }
            }
            if (index<drawCard.Count && drawCard[index] != null)
            {
                drawCard[index].SetActive(false);
            }

        }

        private void OnUpdateDrawCardHandler(MyEvent.MyEvent obj)
        {
            var evt = obj as CardEvent;
            var data = evt.data as CardEvent.CardData;
            var index = data.index;
            var card = data.cards[0];
            if (index == 0)
            {
                ShowDrawCard(index, card);
            }
            else
            {
                //todo
            }
        }


        public void DebugHand(string getCardArrayStr)
        {
            this.debugCardArrayText.text = getCardArrayStr;
        }

        public void DebugText(string txt)
        {
            this.debugText.text = txt;
        }

        public void DebugYama(List<Card> cardList)
        {
            return;
//            var l = Mathf.Min(30, cardList.Count);
//            for (int i = 0; i < l; i++)
//            {
//                if (i >= debugYamaCardList.Count)
//                {
//                    var newCard = Instantiate(cardPrefab);
//                    newCard.Init(cardList[i]);
//                    newCard.gameObject.name = "YamaCard_" + i;
//                    Utils.AddChild(debugYama, newCard);
//                    debugYamaCardList.Add(newCard);
//                }
//                else
//                {
//                    var card = debugYamaCardList[i];
//                    card.Init(cardList[i]);
//                    card.SetActive(true);
//                }
//            }
//            for (int i = l; i < debugYamaCardList.Count; i++)
//            {
//                debugYamaCardList[i].SetActive(false);
//            }
        }

        public void SetDebugVisible(bool b)
        {
            debugText.transform.parent.gameObject.SetActive(b);
        }


    }
}
