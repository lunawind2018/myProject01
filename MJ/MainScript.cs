using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MyEvent;
using UnityEngine;

namespace MJ
{
    public class MainScript : MonoBehaviour
    {
        public const int GAME_OVER = 0;
        public const int GAME_START = 1;
        public const int GAME_PLAY = 2;
        public const int GAME_WAIT = 3;
        //
        public const int PLAYER = 0;
        public const int RIGHT = 1;
        public const int FRONT = 2;
        public const int LEFT = 3;
        //

        public static int PlayerNum = 4;

        [SerializeField]
        public UIManager uiManager;

        [SerializeField]
        private bool ShowDebug;

        //
        private YamaManager yamaManager;
        private HandManager handManager;//player
        private HandManager[] handManagers;
        //
        private int currentPlayer;//0-self 1-right 2-front 3-left
        //
        private int gameStatus;//0-gameover 1-start 2-play 3-wait
        //
//        private Card currentCard;
        //
        private Card currentPlayCard;

        public string DebugString;

        private GameManager gameManager;

        public void Test()
        {
            if (string.IsNullOrEmpty(DebugString)) return;
            Debug.Log("===Test");
            Reset();
            var arr = DebugString.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                var card = yamaManager.DebugDraw(arr[i]);
                if (card == null)
                {
                    Debug.LogError("no card: " + i + " " + arr[i]);
                    break;
                }
                handManager.AddCard(card);
                if (i == Define.HAND_CARD_NUM - 1) UpdateUI();
                if (i == Define.HAND_CARD_NUM)
                {
                    uiManager.ShowDrawCard(0, card);
                    UpdateDebugUI();
                }
            }

        }

        void Awake()
        {
            yamaManager = YamaManager.Instance;
            handManagers = new HandManager[PlayerNum];
            for (int i = 0; i < PlayerNum; i++)
            {
                handManagers[i] = new HandManager(i);//0-self 1-right 2-front 3-left
            }
            handManager = handManagers[0];
            gameStatus = GAME_OVER;
            uiManager.SetDebugVisible(ShowDebug);
            if (uiManager == null)
            {
                var obj = GameObject.Find("UIRoot/Background");
                if (obj != null)
                {
                    uiManager = obj.GetComponent<UIManager>();
                }
                else
                {
                    Debug.LogError("no uimanager");
                }
            }
            RegistEvents();

            Debug.Log("===ready");
        }

        void OnDestroy()
        {
            UnRegistEvents();
        }

        // Use this for initialization
        public void Reset()
        {
            Debug.Log("===reset");
            yamaManager.Reset(PlayerNum);
            for (int i = 0; i < PlayerNum; i++)
            {
                handManagers[i].Reset();
            }
            uiManager.Reset(PlayerNum);

        }


        private void RegistEvents()
        {
            MyEventSystem.RegistEvent(CardEvent.CARD_CLICK, OnClickCardHandler);
        }

        private void UnRegistEvents()
        {
            MyEventSystem.UnRegistEvent(CardEvent.CARD_CLICK, OnClickCardHandler);
        }

        public void GameStart()
        {
            Debug.Log("===draw first cards");
            for (int j = 0; j < PlayerNum; j++)
            {
                for (int i = 0; i < Define.HAND_CARD_NUM; i++)
                {
                    var card = yamaManager.DrawCard();
                    handManagers[j].AddCard(card);
                }
            }
            UpdateUI();
            Debug.Log("===draw first cards end");
        }

        private void UpdateUI()
        {
            //uiManager.UpdatePlayerHand(handManager.GetHandCardList());
            for (int i = 0; i < PlayerNum; i++)
            {
                MyEventSystem.SendEvent(new CardEvent(CardEvent.UI_UPDATE_HAND_CARD, new CardEvent.CardData(i,handManagers[i].GetHandCardList())));
            }
            UpdateDebugUI();
        }

        public void DrawSingleCard()
        {
            var c = yamaManager.DrawCard();
            handManagers[currentPlayer].AddCard(c);
            Debug.Log("draw single card: player " + currentPlayer + " card " + c.cName);
            SendDrawCardEvent(c);
        }

        private void SendDrawCardEvent(Card currcard)
        {
            MyEventSystem.SendEvent(new CardEvent(CardEvent.UI_UPDATE_DRAW_CARD, new CardEvent.CardData(currentPlayer, currcard)));
        }

        public void PlayCard(Card c = null)
        {
            var card = handManagers[currentPlayer].PlayCard(c);
            Debug.Log("player " + currentPlayer + " plays " + card.cName);

            var nakiCode = 0;
            for (int i = 0; i < PlayerNum; i++)
            {
                if (i == currentPlayer) continue;
                var canChi = currentPlayer == i + 1 || currentPlayer == i - 3;
                var naki = handManagers[i].CheckNaki(card, canChi);
                if (naki > 0)
                {
                    Debug.Log("player "+i+" cannaki");
                    nakiCode |= (naki << (3 * i));
                }
            }
            Debug.Log("cannaki: " + Convert.ToString(nakiCode, 2));
            if (nakiCode > 0)
            {
                gameStatus = GAME_WAIT;
            }
            else
            {
                NextPlayer();
            }
        }

        private void UpdateDebugUI()
        {
            if (!ShowDebug) return;
            uiManager.DebugHand(handManager.GetCardArrayStr());
            uiManager.DebugText(DebugText());
            uiManager.DebugYama(yamaManager.GetYamaCardList());
        }

        public void OnClickChiButton()
        {
            Debug.Log("Chi");
        }

        public void OnClickPengButton()
        {
            Debug.Log("Peng");
        }

        public void OnClickGangButton()
        {
            Debug.Log("Gang");
        }

        public void OnClickReachButton()
        {
            Debug.Log("Reach");
        }

        public void OnClickRongButton()
        {
            Debug.Log("Rong");
        }

        private void OnClickCardHandler(MyEvent.MyEvent evt)
        {
            var card = evt.data as CardComponent;
            if (card == null)
            {
                Debug.LogError("???");
                return;
            }
            var c = card.cardData;
            if (currentPlayer == 0)
            {
                if (handManager.GetHandCardList().Contains(c))
                {
                    //click hand card
                    if (handManager.CanPlayCard())
                    {
                        PlayCard(c);
                    }
                }
//                else if (ShowDebug && yamaManager.GetYamaCardList().Contains(c) && handManager.CanDrawCard())
//                {
//                    //click yama card
//                    currentCard = c;
//                    yamaManager.DebugDraw(c);
//                    handManager.AddCard(c);
//                    uiManager.DebugYama(yamaManager.GetYamaCardList());
//                    uiManager.ShowDrawCard(c);
//                    UpdateDebugUI();
//                }
            }
        }

        private string DebugText()
        {
            var cardArray = handManager.GetCardArray();
            var b = handManager.CanPlayCard();
            var sb = new StringBuilder();
            sb.Append("Rong: ");
            List<Calculation.RongData> rdList ;
            var canRong = b && (Calculation.CheckRong(cardArray, out rdList) > 0);
            sb.Append(canRong.ToString());
            sb.Append("\n");
            if (!b)
            {
                sb.Append("Reach: ");
                var list = Calculation.CheckTing(cardArray);
                if (list!=null && list.Count > 0)
                {
                    sb.Append(" True\n");
                    sb.Append(string.Join(",", System.Array.ConvertAll(list.ToArray(), Calculation.IndexToName)));
                }

            }
            return sb.ToString();
        }

        private bool CheckKang()
        {
//            var carr = handManager.GetCardArray();
//            var cindex = this.currentCard.cindex;
//            if (carr[cindex] == 3)
//            {
//                return true;
//            }
            return false;
        }


        private void NextPlayer()
        {
            currentPlayer--;
            if (currentPlayer < 0) currentPlayer += PlayerNum;
        }

        void Update()
        {
            if (gameStatus == GAME_PLAY)
            {
                //npc
                if (currentPlayer != 0)
                {
                    if (handManagers[currentPlayer].CanDrawCard())
                    {
                        //npc draw
                        var c = yamaManager.DrawCard();
                        handManagers[currentPlayer].AddCard(c);
                    }
                    else if (handManagers[currentPlayer].CanPlayCard())
                    {
                        var c = handManagers[currentPlayer].Think();
                        //npc think
                        if (c != null)
                        {
                            PlayCard(c);
                        }
                    }
                }
            }
            else if (gameStatus == GAME_WAIT)
            {
            }
            //
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("gamestatus " + gameStatus + " " + this.gameObject.name);
                switch (gameStatus)
                {
                    case GAME_OVER:
                        Reset();
                        gameStatus = GAME_START;
                        break;
                    case GAME_START:
                        GameStart();
                        gameStatus = GAME_PLAY;
                        break;
                    case GAME_PLAY:
                        if (currentPlayer == 0)
                        {
                            if (handManager.CanDrawCard())
                            {
                                DrawSingleCard();
                            }
                            else
                            {
                                PlayCard();
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
