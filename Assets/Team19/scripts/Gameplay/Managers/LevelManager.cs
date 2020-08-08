using System;
using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{
    public delegate void LevelEvent();

    public class LevelManager : MonoBehaviour
    {
        public static LevelEvent LevelPassed;
        public static LevelEvent LevelLost;

        public static LevelManager Instance;

        [SerializeField]
        private int _winRequiredPassCount;

        [SerializeField]
        private int _failRequireLossCount;

        [SerializeField]
        private Exit _levelWinExit;

        [SerializeField]
        private Exit _levelFailExit;

        [Header("Player Handler")]
        [SerializeField]
        private LevelPlayerHandler _playerHandler;

        private int _levelWinCount = 0;
        private int _levelLossCount = 0;

        private GameStarter _gameStarter;
        private int _startedEntranceNumber;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void NotifyLevelStarted(GameStarter starter, int entrance_number)
        {   
            _gameStarter = starter;
            _startedEntranceNumber = entrance_number;

            _playerHandler.StartLevel(starter, entrance_number);
        }

        public void NotifyLevelLost()
        {
            _levelLossCount ++;

            LevelLost?.Invoke();

            if (_levelLossCount == _failRequireLossCount)
            {
                ActOnLevelFail();
            }
            else
            {
                RespawnPlayer();
            }
        }

        public void NotifyLevelPassed()
        {
            _levelWinCount++;

            if (_levelWinCount == _winRequiredPassCount)
            {
                ActOnLevelWin();
            }
            else
            {
                RespawnPlayer();

                LevelPassed?.Invoke();
            }
        }

        private void ActOnLevelFail()
        {
            _levelFailExit.EndLevel();

        }

        private void ActOnLevelWin()
        {
            _levelWinExit.EndLevel();
        }

        private void RespawnPlayer()
        {
            _playerHandler.StartLevel(_gameStarter, _startedEntranceNumber);
        }
    }
}
