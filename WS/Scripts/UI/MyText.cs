using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class MyText : Text
    {
        [SerializeField]
        private string textId = "";

        protected override void Start()
        {
            base.Start();
            if (!string.IsNullOrEmpty(textId))
            {
                this.text = MasterDataManager.ConstText.GetData(textId).text;
            }
        }
    }

}
