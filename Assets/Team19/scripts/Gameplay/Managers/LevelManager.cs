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

        [SerializeField]
        private LevelPlayerHandler _playerHandler;

        [SerializeField]
        private ModifiedContentManager _modifiedContentManager;

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

            _modifiedContentManager.InitializeByEntrance(entrance_number, _winRequiredPassCount);
            _modifiedContentManager.ModifyContentByProgress(_levelWinCount);

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
                ActOnLevelLost();
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
                ActOnLevelPassed();
            }
        }

        private void ActOnLevelPassed()
        {
            RespawnPlayer();

            _modifiedContentManager.ModifyContentByProgress(_levelWinCount);
            LevelPassed?.Invoke();
        }

        private void ActOnLevelLost()
        {
            RespawnPlayer();
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
