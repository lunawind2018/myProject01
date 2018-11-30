using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.UI;

namespace MJ
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private CardComponent cardPrefab;

        [SerializeField]
        private Transform handCardParent;

        [SerializeField]
        private Transform getCardParent;

        //
        [SerializeField]
        private Text debugCardArrayText;

        [SerializeField]
        private Text debugText;

        [SerializeField]
        private Transform debugYama;

        private List<CardComponent> debugYamaCardList; 
        //


        private List<CardComponent> handCardList;
        private CardComponent drawCard;


        // Use this for initialization
        void Start()
        {
            handCardList = new List<CardComponent>();
            debugYamaCardList = new List<CardComponent>();
        }

        public void ResetHand(List<Card> cardlist)
        {
            if (cardlist.Count > Define.HAND_CARD_NUM)
            {
                Debug.LogError("??? too many card " + cardlist.Count);
            }
            for (int i = 0; i < cardlist.Count; i++)
            {
                if (i >= handCardList.Count)
                {
                    var newCard = Instantiate(cardPrefab);
                    newCard.gameObject.name = "Card_" + (i + 1);
                    AddChild(handCardParent, newCard);
                    newCard.Init(cardlist[i]);
                    newCard.SetLbl2((i+1).ToString());
                    handCardList.Add(newCard);
                }
                else
                {
                    var card = handCardList[i];
                    card.Init(cardlist[i]);
                    card.SetActive(true);
                }
            }
            for (int i = cardlist.Count; i < Define.HAND_CARD_NUM; i++)
            {
                if (i < handCardList.Count)
                {
                    handCardList[i].SetActive(false);
                }
            }
            if (drawCard != null)
            {
                drawCard.SetActive(false);
            }

        }

        public void ShowDrawCard(Card card)
        {
            if (drawCard == null)
            {
                drawCard = Instantiate(cardPrefab);
                AddChild(getCardParent, drawCard);
            }
            drawCard.Init(card);
            drawCard.SetActive(true);
        }

        private void AddChild(Transform parent, MonoBehaviour child)
        {
            child.transform.SetParent(parent);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
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
            var l = Mathf.Min(30, cardList.Count);
            for (int i = 0; i < l; i++)
            {
                if (i >= debugYamaCardList.Count)
                {
                    var newCard = Instantiate(cardPrefab);
                    newCard.Init(cardList[i]);
                    newCard.gameObject.name = "YamaCard_" + i;
                    AddChild(debugYama, newCard);
                    debugYamaCardList.Add(newCard);
                }
                else
                {
                    var card = debugYamaCardList[i];
                    card.Init(cardList[i]);
                    card.SetActive(true);
                }
            }
            for (int i = l; i < debugYamaCardList.Count; i++)
            {
                debugYamaCardList[i].SetActive(false);
            }
        }

        public void SetDebugVisible(bool b)
        {
            debugText.transform.parent.gameObject.SetActive(b);
        }


    }
}
