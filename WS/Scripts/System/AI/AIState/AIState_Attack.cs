using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class AIState_Attack : AIState_Base
    {
        protected int atkRange = 50;

        public AIState_Attack(AIMachine_Base ma) : base(ma)
        {
        }

        public override void Update(float deltaTime)
        {
            //base.Update(deltaTime);
            if (!IsTargetInRange(alertRange))
            {
                this.machine.Switch(Idle);
                return;
            }
            if (IsTargetInRange(atkRange))
            {
                return;
            }
            MoveTo(targetTrans.localPosition, deltaTime);

        }
    }
}