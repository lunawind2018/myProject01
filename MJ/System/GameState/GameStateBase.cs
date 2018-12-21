using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public abstract class GameStateBase
    {
        protected float time;
        public GameStateEnum stateEnum { get; private set; }

        public abstract void Update();

        public abstract void OnEnterState();

        public abstract void OnExitState();

    }
}
