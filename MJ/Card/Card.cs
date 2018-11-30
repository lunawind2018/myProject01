using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public class Card
    {
        public int index;
        public int cindex;
        public string cName;
        public int cNumber;
        public CardType cType;
        public bool Yao { get; private set; }
        public bool SanGen { get; private set; }
        public bool SiShi { get; private set; }
        public bool AkaDora { get; private set; }

        public Card(int idx, CardType t, int n)
        {
            index = idx;
            cindex = CIndex(t,n);
            cType = t;
            cNumber = n;
            GenerateName();
            Yao = IsYao();
            SanGen = IsSanGen();
            SiShi = IsSiShi();
            AkaDora = IsAkaDora();
        }

        private int CIndex(CardType t, int n)
        {
            var tt = (int) t;
            if (tt < 3) return tt * 10 + n - 1;
            if (tt == 3) return 28 + n*2;
            Debug.LogError("???");
            return -1;
        }

        public void SetAkaDora()
        {
            AkaDora = true;
        }

        private bool IsYao()
        {
            if (cType == CardType.Zi) return true;
            return cNumber == 1 || cNumber == 9;
        }

        private bool IsSanGen()
        {
            return cType == CardType.Zi && cNumber >= 5;
        }

        private bool IsSiShi()
        {
            return cType == CardType.Zi && cNumber <= 4;
        }

        private bool IsAkaDora()
        {
            return (cNumber == 5) && (cType != CardType.Zi) && (index % 4 == 0);
        }

        //private static string[] NNAME = { "", "D", "N", "X", "B", "P", "F", "Z" };
        private void GenerateName()
        {
            this.cName = Calculation.IndexToName(this.cindex);
//            switch (cType)
//            {
//                case CardType.Man:
//                    this.cName = "M" + cNumber;
//                    break;
//                case CardType.So:
//                    this.cName = "S" + cNumber;
//                    break;
//                case CardType.Pin:
//                    this.cName = "P" + cNumber;
//                    break;
//                case CardType.Zi:
//                    this.cName = NNAME[cNumber];
//                    break;
//                default:
//                    Debug.LogError("???");
//                    break;
//            }
        }


    }

    public enum CardType
    {
        Man = 0,
        So = 1,
        Pin = 2,
        Zi = 3,
    }

    public class CardFactory
    {
        public static List<Card> GenerateCardDeck()
        {
            var list = new List<Card>();
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new Card(list.Count + 1, CardType.Man, j + 1));
                }
            }
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new Card(list.Count + 1, CardType.So, j + 1));
                }
            }
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new Card(list.Count + 1, CardType.Pin, j + 1));
                }
            }
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    list.Add(new Card(list.Count + 1, CardType.Zi, j + 1));
                }
            }
            return list;
        }
    }
}
