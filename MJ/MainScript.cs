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
        private Card currentCard;
        //
        private Card currentPlayCard;
        //
        private Dictionary<int, int> chiDic;

        public string DebugString;


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
                    uiManager.ShowDrawCard(card);
                    UpdateDebugUI();
                }
            }

        }

        void Awake()
        {
            chiDic = new Dictionary<int, int>();
            yamaManager = YamaManager.Instance;
            handManagers = new HandManager[4];
            for (int i = 0; i < 4; i++)
            {
                handManagers[i] = new HandManager();//0-self 1-right 2-front 3-left
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
            yamaManager.Reset();
            handManager.Reset();

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
            for (int i = 0; i < Define.HAND_CARD_NUM; i++)
            {
                var card = yamaManager.DrawCard();
                handManager.AddCard(card);
            }
            for (int j = 1; j < PlayerNum; j++)
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
            uiManager.ResetHand(handManager.GetHandCardList());
            UpdateDebugUI();
        }

        public void PlayerDrawSingleCard()
        {
            currentCard = yamaManager.DrawCard();
            handManager.AddCard(currentCard);
            uiManager.ShowDrawCard(currentCard);
            UpdateDebugUI();
            CheckKang();
        }

        private void UpdateDebugUI()
        {
            if (!ShowDebug) return;
            uiManager.DebugHand(handManager.GetCardArrayStr());
            uiManager.DebugText(DebugText());
            uiManager.DebugYama(yamaManager.GetYamaCardList());
        }

        public void PlayCard(Card card)
        {
            if (card == null) Debug.LogError("???");
            handManager.PlayCard(card);
            UpdateUI();
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
                else if (handManager.CanDrawCard())
                {
                    //click yama card
                    currentCard = c;
                    yamaManager.DebugDraw(c);
                    handManager.AddCard(c);
                    uiManager.DebugYama(yamaManager.GetYamaCardList());
                    uiManager.ShowDrawCard(c);
                    UpdateDebugUI();
                }
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
            var carr = handManager.GetCardArray();
            var cindex = this.currentCard.cindex;
            if (carr[cindex] == 3)
            {
                return true;
            }
            return false;
        }
        private int CheckNaki()
        {
            int result = 0;
            var carr = handManager.GetCardArray();
            var cindex = this.currentPlayCard.cindex;
            if (carr[cindex] == 3)
            {
                //kang
                result &= 4;
                Debug.Log("Kang");
            }
            else if (carr[cindex] == 2)
            {
                //peng
                result &= 2;
                Debug.Log("Peng");
            }
            {
                //chi
                var a = cindex >= 2 ? carr[cindex - 2] : 0;
                var b = cindex >= 1 ? carr[cindex - 1] : 0;
                var c = cindex <= 27 ? carr[cindex + 1] : 0;
                var d = cindex <= 26 ? carr[cindex + 2] : 0;
                if (a > 0 && b > 0)
                {
                    //12+3
                    chiDic.Add(a, b);
                    chiDic.Add(b, a);
                }
                if (c > 0 && d > 0)
                {
                    //45+3
                    chiDic.Add(d, c);
                    chiDic.Add(c, d);
                }
                if (b > 0 && c > 0)
                {
                    //24+3
                    SafeAdd(chiDic, b, c);
                    SafeAdd(chiDic, c, b);
                }
                if (chiDic.Count > 0)
                {
                    result &= 1;
                    Debug.Log("Chi");
                }
            }
            return result;
        }

        private void SafeAdd(Dictionary<int, int> dic, int k, int v)
        {
            if (dic.ContainsKey(k))
            {
                dic[k] = v;
            }
            else
            {
                dic.Add(k,v);
            }
        }

        private void NextPlayer()
        {
            currentPlayer++;
            if (currentPlayer >= 4) currentPlayer = 0;
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
                            //npc play
                            handManagers[currentPlayer].PlayCard(c);
                            //check naki
                            var naki = CheckNaki();
                            //wait
                            if (naki > 0)
                            {
                                gameStatus = GAME_WAIT;
                                currentPlayCard = c;
                            }
                            else//no naki
                            {
                                NextPlayer();
                            }
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
                        if (handManager.CanDrawCard())
                        {
                            PlayerDrawSingleCard();
                        }
                        else
                        {
                            PlayCard(currentCard);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
