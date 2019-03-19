using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;

namespace WS
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField]
        public RectTransform UILayerCenter;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("multi instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            RegisterEvent();
            HideHint();
        }

        void OnDestroy()
        {
            UnRegisterEvent();
        }

        [SerializeField]
        private UIHeader uiHeader;
        [SerializeField]
        private UIBottomMenu uiBottomMenu;
        [SerializeField]
        private UIMessageWindow uiMessageWindow;
        [SerializeField]
        private UIHintPart uiHint;

        private UIWindowBase currUIWindow;
        private UIType currUIType;

        public static T OpenUI<T>(UIType t, params object[] args) where T : UIWindowBase
        {
            if (Instance.currUIType != UIType.None)
            {
                CloseUI();
            }

            var path = "Prefab/UI_Prefab/";
            if (prefabPath.ContainsKey(t))
            {
                path += prefabPath[t] + "/";
            }
            path += "UI" + t;
            Debug.Log("open " + t + " " + path);
            var ui = Instantiate(Resources.Load<T>(path));
            ui.transform.SetParent(Instance.UILayerCenter, false);
            ui.transform.localScale = Vector3.one;

            Instance.currUIWindow = ui;
            Instance.currUIType = t;

            ui.Init(args);

            return ui;
        }

        public static void CloseUI()
        {
            CloseUI(Instance.currUIWindow);
        }

        public static void CloseUI(UIWindowBase window)
        {
            if (Instance.currUIWindow.Busy) return;
            if (Instance.currUIWindow == window)
            {
                Instance.currUIWindow = null;
                Instance.currUIType = UIType.None;
            }
            DestroyImmediate(window.gameObject);
        }

        public static UIWindowBase OpenUI(UIType t, params object[] args)
        {
            return OpenUI<UIWindowBase>(t, args);
        }

        private void RegisterEvent()
        {
            MyEventSystem.RegistEvent(MyGameEvent.GLOBAL_MESSAGE, OnGlobalMessageHandler);
            MyEventSystem.RegistEvent(MyGameEvent.HINT_MESSAGE, OnHintMessageHandler);
            MyEventSystem.RegistEvent(MyKeyEvent.KEY_DOWN, OnKeyDownHandler);
        }

        private void UnRegisterEvent()
        {
            MyEventSystem.UnRegistEvent(MyGameEvent.GLOBAL_MESSAGE, OnGlobalMessageHandler);
            MyEventSystem.UnRegistEvent(MyGameEvent.HINT_MESSAGE, OnHintMessageHandler);
            MyEventSystem.UnRegistEvent(MyKeyEvent.KEY_DOWN, OnKeyDownHandler);
        }

        private void OnKeyDownHandler(MyEvent.MyEvent obj)
        {
            var key = obj.data.ToString();
            if (key == KeyControl.ESC)
            {
                if (this.currUIWindow == null)
                {
                    uiBottomMenu.Switch();
                }
                else
                {
                    CloseUI();
                }
                return;
            }
            if (this.currUIWindow!=null && key == KeyControl.RIGHT)
            {
                CloseUI();
                return;
            }
            if (strToType.ContainsKey(key))
            {
                var openType = strToType[key];
                if (currUIType != openType)
                {
                    OpenUI(openType);
                }
                else
                {
                    CloseUI();
                }
            }

        }

        private void OnGlobalMessageHandler(MyEvent.MyEvent obj)
        {
            var evt = obj as MyGameEvent;
            if (evt == null) return;
            var msg = evt.data.ToString();
            Debug.Log("message " + msg);
            uiMessageWindow.AddText(msg);
        }
        private void OnHintMessageHandler(MyEvent.MyEvent obj)
        {
            var arr = (string[])obj.data;
            uiHint.Show(arr[0], arr[1]);
        }

        private Dictionary<string, UIType> strToType = new Dictionary<string, UIType>()
        {
            {KeyControl.BAG,UIType.Bag},
            {KeyControl.CRAFT,UIType.Craft},
            {KeyControl.CHARACTER,UIType.Character},
        };
        private static Dictionary<UIType, string> prefabPath = new Dictionary<UIType, string>()
        {
            {UIType.Bag, "Bag"},
            {UIType.Craft, "Craft"},
            {UIType.Character, "Character"},
        };

        public void SetHint(string n,string d)
        {
            uiHint.Show(n,d);
        }

        public void HideHint()
        {
            uiHint.Hide();
        }
    }
    public enum UIType
    {
        None,
        Bag,
        System,
        Craft,
        Character,
    }
}
