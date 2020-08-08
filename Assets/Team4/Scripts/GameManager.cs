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
        

        public List<MessageScript> IntroMessages;

        public int InitialUnitsCount = 35;

        void Awake()
        {
            _introManager = new IntroManager(IntroMessages);
            _boardManager = new BoardManager(9);
            _choiceManager = new ChoiceManager(_boardManager);
            
        }

    
        // Start is called before the first frame update
        void Start()
        {
            _boardManager.AddRandomUnits(InitialUnitsCount);
            SetState(GameState.intro);
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
            EventManager.Singleton.IntroDone -= OnIntroDone;     
            EventManager.Singleton.NextMessage -= _introManager.NextMessage;
            EventManager.Singleton.TurnOver += OnTurnOver;
            var choices = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};//TODO take from boardManager
            
            UIManager.SetPlayerAvailableNumbers(choices, true);
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
            _turnManager.PlayNextTurn(nextTurnData);
        }

        private bool IsGameOver()
        {
            //TODO implement check
            return false;
        }

        private void OnIntroDone()
        {
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
