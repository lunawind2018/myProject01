using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UI_Character : UIWindow
    {
        [SerializeField]
        private Text nameTxt;
        [SerializeField]
        private Text levelTxt;
        [SerializeField]
        private UI_BarWithText expTxt;
        [SerializeField]
        private UI_BarWithText hpTxt;
        [SerializeField]
        private UI_BarWithText mpTxt;
        [SerializeField]
        private UI_BarWithText spTxt;

        public override void Init(params object[] args)
        {
            var data = PlayerManager.Instance.playerData;
            nameTxt.text = string.IsNullOrEmpty(data.playerName) ? ConstTextManager.Get(TextId.Player_Name_Default) : data.playerName;
            UpdateProperty(data.property);
        }

        public void UpdateProperty(CharProperty p)
        {
            levelTxt.text = p.Level.ToString();

            expTxt.SetValue(p.Exp, p.MaxExp);

            hpTxt.SetValue(p.Hp, p.MaxHp,p.HpReg);

            mpTxt.SetValue(p.Mp, p.MaxMp,p.MpReg);

            spTxt.SetValue(p.Sp, p.MaxSp,p.SpReg);

        }
    }

    [Serializable]
    public class UI_BarWithText
    {
        [SerializeField]
        private Text currTxt;
        [SerializeField]
        private Text maxTxt;
        [SerializeField]
        private MySlider bar;

        [SerializeField]
        private Text regTxt;

        private int max;
        public void SetValue(int m1, int m2)
        {
            this.max = m2;
            if (this.maxTxt != null) this.maxTxt.text = m2+"";
            SetValue(m1);
        }

        public void SetValue(int m1, int m2, int reg)
        {
            SetValue(m1,m2);
            if (regTxt != null)
            {
                regTxt.text = string.Format("+{0}/s", reg);
            }
        }

        public void SetValue(int m)
        {
            if (maxTxt == null)
            {
                this.currTxt.text = m + "/" + this.max;
            }
            else
            {
                this.currTxt.text = m + "";
            }
            this.bar.value = (float)m / this.max;
        }
    }
}
