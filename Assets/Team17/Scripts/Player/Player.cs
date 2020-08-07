using UnityEngine;

namespace TheFlyingDragons
{
    //[RequireComponent(typeof(PlayerInputListener))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public PlayerConfig playerConfig;

        [Space]
        
        public Sensor head;
        public Sensor feet;

        Rigidbody body;
        
        public PlayerInputData Input => Game.Input; // inputListener.data;
        public Rigidbody Body => body;

        public bool IsJumpingFuzzy => Input.jumpReady && (Time.time - Input.jumpTime) < playerConfig.jumpFuzzy;

        void Awake()
        {
            //inputListener = GetComponent<PlayerInputListener>();
            body = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            Input.Reset();

            //head.layers = playerConfig.groundLayers;
            //feet.layers = playerConfig.groundLayers;
        }

        void Update()
        {

        }
    }
}
