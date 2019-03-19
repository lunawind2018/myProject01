using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace WS
{
    public class FieldMonster : FieldObjectNpc
    {
        public int level = 1;

        public int camp = 2;

        public int state = 0;

        private MasterDataMonster monsterData;

        protected AIMachine_Base AI;

        public void Init(int id)
        {
            this.alwaysShowHp = true;
            this.monsterData = MasterDataManager.Monster.GetData(id);
            InitProperty();
            UpdateHpBar();
            ShowHp();
            this.transform.localScale = Vector3.one * monsterData.size;
            this.SetName(monsterData.name);
            this.AI = new AIMachine_Base();
            this.AI.Init(id, this);
            this.AI.active = true;
        }

        protected virtual void InitProperty()
        {
            hp = maxhp = monsterData.Hp + level * monsterData.HpGrowth;
            mp = maxmp = monsterData.Mp + level * monsterData.MpGrowth;
            sp = monsterData.Sp + level * monsterData.SpGrowth;
            atk = monsterData.Atk + level * monsterData.AtkGrowth;
            def = monsterData.Def + level * monsterData.DefGrowth;
        }

        protected override void Update()
        {
            base.Update();
            if (!IsDead)
            {
                AI.Update(Time.deltaTime);
            }
        }

        protected bool canMove = true;
        public void MoveTo(Vector3 pos, float t)
        {
            if (canMove)
            {
                var dir = (pos - this.transform.localPosition).normalized;
                var x = moveSpeed.x * t * dir.x;
                var y = moveSpeed.y * t * dir.y;
                this.transform.Translate(x, y, 0, Space.Self);
            }
        }

        public void BeHit(FieldObject caster, int i)
        {
            var dir = (this.transform.localPosition - caster.transform.localPosition).normalized * 10;
            canMove = false;
            this.transform.DOLocalMove(this.transform.localPosition + dir, 0.1f).OnComplete(
                () =>
                {
                    canMove = true;
                });

        }

        public override string GetHintName()
        {
            return monsterData.name + "  Lv." + level;
        }

        public override string GetDesc()
        {
            return IsDead ? monsterData.desc_dead : monsterData.desc;
        }

        public int GetCorpseId()
        {
            return 1000 + this.monsterData.intid;
        }
    }
}