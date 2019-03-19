using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UI_ItemIconSmall : MonoBehaviour
    {
        [SerializeField]
        private Text nameTxt;
        [SerializeField]
        private Text numText;

        private bool unlock;

        public virtual void Init(string id, bool ulock = true, int len = -1)
        {
            var data = MasterDataManager.Item.GetData(id);
            this.unlock = ulock;
            if (unlock)
            {
                var str = data.name;
                if (len > 0 && str.Length > len) str = str.Substring(0, len) + "...";
                this.nameTxt.text = str;
            }
            else
            {
                this.nameTxt.text = "????";
            }
            this.numText.text = "";
        }

        public virtual void Init()
        {
            this.nameTxt.text = "????";
        }

        public virtual void Init(MasterDataItem itemData, int len = -1)
        {
            var str = (len <= 0) ? itemData.name : itemData.name.Substring(0, len);
            this.nameTxt.text = str;
        }

        public void SetNum(int n)
        {
            this.numText.text = n + "";
        }

        public void SetNum(int n1, int n2, bool color=true)
        {
            if (color)
            {
                this.numText.text = n1 + "/" + n2;
                this.numText.color = n1 < n2 ? Color.red : Color.white;
            }
            else
            {
                this.numText.text = n1 + "/" + n2;
            }
        }

        public void SetActive(bool v)
        {
            if (this.gameObject.activeSelf != v)
            {
                this.gameObject.SetActive(v);
            }
        }
    }
}
