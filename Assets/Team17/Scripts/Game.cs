using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TheFlyingDragons
{
    [RequireComponent(typeof(PlayerInputListener))]
    public class Game : MonoBehaviour {

        [Space]
        public GameConfig config;

        static Game game;
        public static Game App
        {
            get
            {
                // If not for the Matrix Jam could have set the excecution order instead
                if (game == null) 
                {
                    game = FindObjectOfType<Game>();
                    game.Initialize();
                }
                return game;
            }
        }
        public static GameConfig Config => App.config;
        public static PlayerInputData Input => App.inputListener.data;

        PlayerInputListener inputListener;

        // If not for the Matrix Jam could have set the excecution order instead
        /*
        void Awake()
        {
            if (game != null)
            {
                Debug.LogError("Multiple Game instances!");
                return;
            }
            Initialize();
        }
        */

        void Initialize()
        {
            game = this;
            inputListener = GetComponent<PlayerInputListener>();
        }

    }
}