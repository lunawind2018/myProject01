using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UIHintPart : MonoBehaviour
    {

        [SerializeField]
        private Text nameTxt;
        [SerializeField]
        private Text descTxt;

        public void Show(string n,string d)
        {
            if (string.IsNullOrEmpty(n) && string.IsNullOrEmpty(d)) return;
            if(!this.gameObject.activeSelf)this.gameObject.SetActive(true);
            nameTxt.text = n;
            descTxt.text = d;
        }

        public void Hide()
        {
            if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
        }
    }
}