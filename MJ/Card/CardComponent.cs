using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyEvent;
using UnityEngine;
using UnityEngine.UI;

namespace MJ
{
    public class CardComponent : MonoBehaviour
    {
        private Color defaultColor = Color.white;
        private Color hlColor = Color.gray;

        public Card cardData { get; private set; }

        [SerializeField]
        private Text lbl1;

        [SerializeField]
        private Text lbl2;

        [SerializeField]
        private Image bgImage;

        void Awake()
        {
            this.bgImage.color = defaultColor;
            if (SpriteDic == null || SpriteDic.Count == 0)
            {
                InitSpriteDic();
            }
            this.lbl1.gameObject.SetActive(false);
        }

        private void InitSpriteDic()
        {
            var sprites = Resources.LoadAll<Sprite>("cards");
            SpriteDic = new Dictionary<string, Sprite>();
            foreach (KeyValuePair<string, string> keyValuePair in SpriteNameDic)
            {
                SpriteDic.Add(keyValuePair.Key, sprites.First(s => s.name == keyValuePair.Value));
            }
        }

        public void Init(Card data)
        {
            if (this.cardData == data) return;
            this.cardData = data;
            this.lbl1.text = data.cName;
            var key = data.cName;
            if (data.AkaDora) key += "R";
            this.bgImage.sprite = SpriteDic[key];
        }

        public void SetActive(bool b)
        {
            if (this.gameObject.activeSelf == b) return;
            this.gameObject.SetActive(b);
        }

        public void OnPointerEnterHandler()
        {
            bgImage.color = hlColor;
        }

        public void OnPointerExitHandler()
        {
            bgImage.color = defaultColor;
        }

        public void OnPointerClickHandler()
        {
            Debug.Log("===click "+ this.cardData.index);
            MyEventSystem.SendEvent(new CardEvent(CardEvent.CARD_CLICK, this));
        }

        public void SetLbl2(string s)
        {
            this.lbl2.text = s;
        }

        private static Dictionary<string, Sprite> SpriteDic;

        private static Dictionary<string,string> SpriteNameDic = new Dictionary<string, string>()
        {
            {"M1","cards_0"},
            {"M2","cards_1"},
            {"M3","cards_2"},
            {"M4","cards_3"},
            {"M5","cards_4"},
            {"M6","cards_5"},
            {"M7","cards_6"},
            {"M8","cards_7"},
            {"M9","cards_8"},
            {"S1","cards_18"},
            {"S2","cards_19"},
            {"S3","cards_20"},
            {"S4","cards_21"},
            {"S5","cards_22"},
            {"S6","cards_23"},
            {"S7","cards_24"},
            {"S8","cards_25"},
            {"S9","cards_26"},
            {"P1","cards_36"},
            {"P2","cards_37"},
            {"P3","cards_38"},
            {"P4","cards_39"},
            {"P5","cards_40"},
            {"P6","cards_41"},
            {"P7","cards_42"},
            {"P8","cards_43"},
            {"P9","cards_44"},
            {"D","cards_54"},
            {"N","cards_55"},
            {"X","cards_56"},
            {"B","cards_57"},
            {"P","cards_58"},
            {"F","cards_59"},
            {"Z","cards_60"},
            {"M5R","cards_13"},
            {"S5R","cards_31"},
            {"P5R","cards_49"},

        };
    }
}
