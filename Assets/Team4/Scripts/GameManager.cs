using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team4
{
    public class GameManager : MonoBehaviour
    {
        public GameState gameState = GameState.init;
        private IntroManager _introManager;
        private TurnManager _turnManager;
        private BoardManager _boardManager;
        

        public List<MessageScript> IntroMessages;

        public int InitialUnitsCount = 35;

        void Awake()
        {
            _introManager = new IntroManager(IntroMessages);
            _boardManager = new BoardManager(9, InitialUnitsCount);
        }

    
        // Start is called before the first frame update
        void Start()
        {
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
            EventManager.Singleton.OnTurnOver += OnTurnOver;
            var choices = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9};//TODO take from boardManager
            
            UIManager.SetPlayerAvailableNumbers(choices, true);
        }

        private void OnTurnOver()
        {
            var nextPlayer = _turnManager.GetNextPlayer();
            var nextTurnData = _boardManager.GetPlayerTurnData(nextPlayer);
            _turnManager.PlayNextTurn(nextTurnData);
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
