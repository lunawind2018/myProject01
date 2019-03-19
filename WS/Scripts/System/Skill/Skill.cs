using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;

namespace WS
{
    public class Skill
    {
        private const int DEAD = 99;

        private MasterDataSkill skillData;

        private float currTime = 0;

        private int state = 0;

        public bool Dead { get { return state >= DEAD; } }

        private FieldObject caster;

        private EffectBase effect;

        private System.Action callBack; 

        public void Init(MasterDataSkill sd, FieldObject c, EffectBase e, System.Action cb)
        {
            this.skillData = sd;
            this.caster = c;
            this.effect = e;
            this.callBack = cb;
        }

        public void Update(float t)
        {
            if (this.state >= 99)
            {
                return;
            }
            this.currTime += t;
            if (this.currTime >= this.skillData.total_time)
            {
                this.state = DEAD;
                return;
            }
            if (this.state == 0 && this.currTime > this.skillData.hit_time)
            {
                //hit
                CalcHit();
                this.state = 1;
            }
        }

        private void CalcHit()
        {
            if (effect.HitList.Count > 0)
            {
                foreach (var fieldObject in effect.HitList)
                {
                    var monster = fieldObject as FieldMonster;
                    if (monster == null) continue;
                    monster.AddHp(-skillData.attack);
                    monster.BeHit(caster, 10);
                    Debug.Log("hit " + monster.name + " " + skillData.attack);
                }
            }

        }

        public void DestroySelf()
        {
            this.skillData = null;
            this.effect.DestroySelf();
            this.effect = null;
            if (this.callBack != null)
            {
                this.callBack.Invoke();
                this.callBack = null;
            }
        }
    }
}
