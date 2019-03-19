using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class FieldObjectNpc : FieldObjWithCircle
    {

        public int hp;
        public int maxhp;
        public int mp;
        public int maxmp;
        public int sp;
        public int maxsp;

        public int atk;
        public int def;

        public Vector2 moveSpeed = new Vector2(50, 50);

        public bool alwaysShowHp;
        private bool needUpdate;
        private Slider hpBar;

        public bool IsDead { get; protected set; }
        protected override void Awake()
        {
            base.Awake();
            this.hpBar = this.transform.Find("bar").GetComponent<Slider>();
        }

        public void UpdateHpBar()
        {
            var v = this.maxhp > 0 ? (float)this.hp/this.maxhp : 0;
            this.hpBar.value = v;
            var b = v > 0.001f;
            if (hpBar.fillRect.gameObject.activeSelf != b)
            {
                hpBar.fillRect.gameObject.SetActive(b);
            }
        }

        public void HideHp()
        {
            this.hpBar.gameObject.SetActive(false);
        }

        public void ShowHp()
        {
            this.hpBar.gameObject.SetActive(true);
        }

        public void AddHp(int v)
        {
            hp += v;
            if (hp > maxhp) hp = maxhp;
            if (hp <= 0)
            {
                Dead();
            }
            needUpdate = true;
        }

        public virtual void Dead()
        {
            this.IsDead = true;
            this.nameTxt.color = Color.gray;
        }

        protected virtual void Update()
        {
            if (needUpdate)
            {
                UpdateHpBar();
                needUpdate = false;
            }
            if (this.body.transform.localPosition != Vector3.zero)
            {
                this.transform.localPosition += this.body.transform.localPosition;
                this.body.transform.localPosition = Vector3.zero;
            }
        }
        
    }
}