using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TheFlyingDragons
{
    public class Game : MonoBehaviour {

        [Space]
        public GameConfig config;

        static Game game;
        public static Game App => game;
        public static GameConfig Config => game.config;

        void Awake()
        {
            if (game != null)
            {
                Debug.LogError("Multiple Game instances!");
                return;
            }
            
            game = this;
        }

    }
}