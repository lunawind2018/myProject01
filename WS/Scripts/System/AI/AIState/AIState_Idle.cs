using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class AIState_Idle : AIState_Base
    {
        protected float timeCount;

        public AIState_Idle(AIMachine_Base ma):base(ma)
        {

        }

        public override void Update(float deltaTime)
        {
            timeCount += deltaTime;
            if(timeCount>1)
            {
                timeCount -= 1;
                if (IsTargetInRange(alertRange))
                {
                    machine.Switch(Attack);
                }
            }
        }
    }
}