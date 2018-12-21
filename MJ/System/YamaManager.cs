using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public class YamaManager
    {
        private static YamaManager mInstance;

        public static  YamaManager Instance
        {
            get { return mInstance ?? (mInstance = new YamaManager()); }
        }

        private YamaManager() { }


        private List<Card> yamaCardList = new List<Card>();
        public void Reset(int playerNum)
        {
            yamaCardList.Clear();
            yamaCardList = CardFactory.GenerateCardDeck(playerNum);
            Suffle();
        }

        private void Suffle()
        {
            var l = yamaCardList.Count;
            for (int i = 0; i < l; i++)
            {
                var index = Random.Range(0, l - i);
                var temp = yamaCardList[index];
                yamaCardList[index] = yamaCardList[l - i - 1];
                yamaCardList[l - i - 1] = temp;
            }
        }

        public Card DrawCard()
        {
            if (yamaCardList.Count <= 0)
            {
                Debug.LogError("no card !!!");
                return null;
            }
            var card = yamaCardList[0];
            yamaCardList.RemoveAt(0);
            //Debug.Log("===[yama] draw card :" + card.cName + " , left:" + yamaCardList.Count);
            return card;
        }

        public void DebugDraw(Card cardData)
        {
            if (yamaCardList.Count <= 0)
            {
                Debug.LogError("no card !!!");
                return;
            }
            var index = yamaCardList.IndexOf(cardData);
            if (index == -1)
            {
                Debug.LogError("no card :" + cardData.cName);
                return;
            }
            yamaCardList.RemoveAt(index);
        }

        public Card DebugDraw(string cardName)
        {
            if (yamaCardList.Count <= 0)
            {
                Debug.LogError("no card !!!");
                return null;
            }
            int i;
            for (i = 0; i < yamaCardList.Count; i++)
            {
                if (yamaCardList[i].cName.Equals(cardName)) break;
            }
            var result = yamaCardList[i];
            yamaCardList.RemoveAt(i);
            return result;
        }

        public List<Card> GetYamaCardList()
        {
            return yamaCardList;
        }
    }
}
