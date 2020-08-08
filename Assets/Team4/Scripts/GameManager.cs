using System.Collections;
using System.Collections.Generic;
using MatrixJam.Team;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class GameManager : MonoBehaviour
    {
        public GameState gameState = GameState.init;
        private IntroManager _introManager;
        private TurnManager _turnManager;
        private BoardManager _boardManager;
        private IChoiceManager _choiceManager;
        

        public List<Player> Players;
        public List<MessageScript> IntroMessages;

        public int InitialUnitsCount = 35;
        public bool ShowTutorial;

        void Awake()
        {
            _introManager = new IntroManager(IntroMessages);
            _boardManager = new BoardManager(9);
            _choiceManager = new ChoiceManager(_boardManager);
            _turnManager = new TurnManager(Players);
            UIManager.ChoiceManager = _choiceManager;

        }

    
        // Start is called before the first frame update
        void Start()
        {
            _boardManager.AddRandomUnits(InitialUnitsCount);
            if (ShowTutorial)
            {
                SetState(GameState.intro);
            }
            else
            {
                SetState(GameState.mainPlay);
            }
        }

        private void HandleIntro()
        {
            EventManager.Singleton.IntroDone += OnIntroDone;     
            EventManager.Singleton.NextMessage += _introManager.NextMessage;   
            _introManager.Start();
        }
        
        public void NextMessage()
        {
            _introManager.NextMessage();
        }
        
        private void HandleMainPlay()
        {
            if (ShowTutorial)
            {
                EventManager.Singleton.IntroDone -= OnIntroDone;     
                EventManager.Singleton.NextMessage -= _introManager.NextMessage;
            }
            EventManager.Singleton.TurnOver += OnTurnOver;
            OnTurnOver();
        }

        private void OnTurnOver()
        {
            if (IsGameOver())
            {
                EventManager.Singleton.OnGameOver();
                return;
            }
            
            var nextPlayer = _turnManager.GetNextPlayer();
            var nextTurnData = _boardManager.GetPlayerTurnData(nextPlayer);
            nextPlayer.YourTurn(nextTurnData);
        }

        private bool IsGameOver()
        {
            //TODO implement check
            return false;
        }

        private void OnIntroDone()
        {
            ShowTutorial = false;
            SetState(GameState.mainPlay);
        }

        public void SetState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.intro:
                    HandleIntro();
                    break;
                case GameState.mainPlay:
                    HandleMainPlay();
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

    public enum GameState
    {
        init,
        intro,
        mainPlay
    }
}
