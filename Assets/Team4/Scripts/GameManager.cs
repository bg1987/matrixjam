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
        public List<MessageObject> IntroMessages;

        void Awake()
        {
            _introManager = new IntroManager(IntroMessages);
        }

    
        // Start is called before the first frame update
        void Start()
        {
            SetState(GameState.intro);
        }

        private void HandleIntro()
        {
            _introManager.nextMessage();
        }
        
        private void HandleMainPlay()
        {
            EventManager.Singleton.IntroDone += OnIntroDone;     
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
