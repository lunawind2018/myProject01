using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public enum GameStateEnum
    {
        GameOver,
        GameStart,
        DrawAndWait,
        PlayAndWait,
        WaitForNaki,
    }
    public class GameManager
    {
        private static GameManager mInstance;

        public static GameManager Instance
        {
            get { return mInstance ?? (mInstance = new GameManager()); }
        }

        private GameStateEnum currStateEnum;
        private GameStateBase currState;
        private Dictionary<GameStateEnum, GameStateBase> stateDic;

        private GameManager()
        {
            stateDic = new Dictionary<GameStateEnum, GameStateBase>
            {
                {GameStateEnum.GameOver, new GameState_GameOver()},
                {GameStateEnum.GameStart, new GameState_GameOver()},
                {GameStateEnum.DrawAndWait, new GameState_GameOver()},
                {GameStateEnum.PlayAndWait, new GameState_GameOver()},
                {GameStateEnum.WaitForNaki, new GameState_GameOver()},
            };
        }

        public void Update()
        {
            currState.Update();
        }

        public void ChangeState(GameStateEnum state,bool forceChange=false)
        {
            if (currStateEnum == state && !forceChange) return;
            currState.OnExitState();
            currStateEnum = state;
            currState = stateDic[state];
            currState.OnEnterState();
        }
    }


}
