using UnityEngine;

namespace WS
{
    public class AIState_Base
    {
        protected int alertRange;
        protected Transform targetTrans;
        protected Transform myTrans;
        protected AIMachine_Base machine;

        public AIState_Base(AIMachine_Base ma)
        {
            this.machine = ma;
            this.alertRange = 300;//ma.data.alert;
            this.myTrans = ma.Script.transform;
            this.targetTrans = PlayerManager.Instance.fieldPlayer.transform;
        }

        public virtual bool Enter()
        {
//            Debug.Log("enter " + this);
            return true;
        }

        public virtual bool Exit()
        {
//            Debug.Log("exit " + this);
            return true;
        }

        public virtual void Update(float deltaTime)
        {

        }

        protected virtual bool IsTargetInRange(int r)
        {
            var vec = targetTrans.localPosition - myTrans.localPosition;
            return (vec.x > -r && vec.x < r && vec.y > -r && vec.y < r);
        }

        protected virtual void MoveTo(Vector3 pos, float d)
        {
            machine.Script.MoveTo(pos, d);
        }

        public const int Idle = 0;
        public const int Attack = 1;

    }
}
