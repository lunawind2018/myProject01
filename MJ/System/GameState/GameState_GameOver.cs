using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public class GameState_GameOver : GameStateBase
    {
        public override void Update()
        {
        }

        public override void OnEnterState()
        {
            Debug.Log("===enter gameover");
        }

        public override void OnExitState()
        {
            Debug.Log("===exit gameover");
        }
    }
}
