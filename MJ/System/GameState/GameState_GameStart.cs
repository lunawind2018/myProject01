using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public class GameState_GameStart : GameStateBase
    {
        public override void Update()
        {
        }

        public override void OnEnterState()
        {
            time = Time.time;
        }

        public override void OnExitState()
        {
            
        }
    }
}
