using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UIWindowBase : MonoBehaviour
    {
        [HideInInspector]
        public bool Busy = false;

        protected virtual void Awake()
        {
            var obj = this.transform.Find("Bg/BtnClose");
            if (obj != null)
            {
                var btnClose = obj.gameObject.GetComponent<Button>();
                btnClose.onClick.AddListener(Close);
            }
            RegisterEvent();
        }

        protected virtual void OnDestroy()
        {
            UnRegisterEvent();
        }

        protected virtual void RegisterEvent()
        {
            
        }

        protected virtual void UnRegisterEvent()
        {
            
        }

        public virtual void Init(params object[] args)
        {
            
        }

        public virtual void Close()
        {
            UIManager.CloseUI(this);
        }
    }
}
