using UnityEngine;

namespace TheFlyingDragons
{
    [RequireComponent(typeof(PlayerInputListener))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public PlayerConfig playerConfig;

        [Space]
        
        public Sensor2D head;
        public Sensor2D feet;

        PlayerInputListener inputListener;
        Rigidbody2D body;
        float lastGroundedTime;

        public PlayerInputData Input => inputListener.data;
        public Rigidbody2D Body => body;
        public bool IsGrounded => feet.IsDetected;
        public bool IsGroundedFuzzy => (Time.time - lastGroundedTime) < playerConfig.groundedFuzzy;
        public bool IsJumpingFuzzy => Input.jumpReady && (Time.time - Input.jumpTime) < playerConfig.jumpFuzzy;
        public bool IsHunched => head.IsDetected;
        public bool IsCrouching => playerConfig.crouchEnabled && (Input.crouch || (IsHunched && IsGrounded));

        void Awake()
        {
            inputListener = GetComponent<PlayerInputListener>();
            body = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            Input.Reset();

            head.layers = playerConfig.groundLayers;
            feet.layers = playerConfig.groundLayers;
        }

        void Update()
        {
            if (IsGrounded)
            {
                lastGroundedTime = Time.time;
            }
        }
    }
}
