using UnityEngine;

namespace MatrixJam.Team19.Gameplay.Managers
{

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] 
        private int _winRequiredPassCount;

        [SerializeField]
        private int _failRequireLossCount;

        [SerializeField]
        private Exit _levelWinExit;

        [SerializeField]
        private Exit _levelFailExit;

        private int _levelWinCount = 0;
        private int _levelLossCount = 0;

        public delegate void LevelEvent();

        public LevelEvent LevelStarted;
        public LevelEvent LevelPassed;
        public LevelEvent LevelLost;

        private GameStarter _gameStarter;
        private int _startedEntranceNumber;

        public static LevelManager Instance;

        [SerializeField]
        private GameObject _playerPrefab;

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

            Instantiate(_playerPrefab, starter.transform.position, starter.transform.rotation);

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
                _gameStarter.StartHelp(_startedEntranceNumber);
            }
        }

        public void NotifyLevelPassed()
        {
            _levelWinCount++;

            if (_levelWinCount == _winRequiredPassCount)
            {
                ActOnLevelWin();
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
    }
}
