using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("multi instance");
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        [SerializeField] private UIHeader uiHeader;
        [SerializeField] private UIBottomMenu uiBottomMenu;
        [SerializeField] private UIMessageWindow uiMessageWindow;

    }
}
