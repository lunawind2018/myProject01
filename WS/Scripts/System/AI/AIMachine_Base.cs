using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class AIMachine_Base : SimpleStateMachine
    {
        public bool active;
        public Transform target;
        public Vector3 birthPos;
        protected Dictionary<int, AIState_Base> stateDic;
        protected AIState_Base CurrState;
        public FieldMonster Script { get; protected set; }

        public AIMachine_Base() : base()
        {
            stateDic = new Dictionary<int, AIState_Base>();
        }

        public virtual void Init(int id, FieldMonster sc)
        {
            Script = sc;
            stateDic.Add(AIState_Base.Idle,new AIState_Idle(this));
            stateDic.Add(AIState_Base.Attack,new AIState_Attack(this));
            CurrState = stateDic[AIState_Base.Idle];
        }

        public override bool Switch(int newState)
        {
            Debug.Log(Script.gameObject.name + " switch " + newState);
            if (!active) return false;
            if (State == newState) return false;
            if (!OnExitState()) return false;
            State = newState;
            CurrState = stateDic[newState];
            if (!OnEnterState()) return false;
            return true;
        }

        protected override bool OnEnterState()
        {
            return active && CurrState.Enter();
        }

        protected override bool OnExitState()
        {
            return active && CurrState.Exit();
        }

        public void Update(float deltaTime)
        {
            if (active)
            {
                CurrState.Update(deltaTime);
            }
        }
    }
}