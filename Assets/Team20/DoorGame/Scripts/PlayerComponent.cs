using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class PlayerComponent : MonoBehaviour
    {
        MovementComponent movement;
        ApplyGravityComponent applyGravity;
        public float walkSpeed = 10f;
        public float jumpHeight = 10f;
        public bool resetHorizontal = false;
        float lastHorizontal = 0f;
        public LayerMask groundLayer;
        public float distance = 0.1f;
        [SerializeField] private bool _onMovingPlatform = false;
        public float movingPlatformFix = 1;

        [SerializeField] private float _coinNumber = 0f;

        private enum LookDirection
        {
            Right,
            Left
        }

        [SerializeField] private LookDirection _lookDirection;

        void Start()
        {
            movement = GetComponent<MovementComponent>();
            applyGravity = GetComponent<ApplyGravityComponent>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateVelocity();
            DoorCollection();
        }

        void UpdateVelocity()
        {
            float horizontal = Input.GetAxis("Horizontal");
            if (horizontal == 0f)
            {
                resetHorizontal = false;
            }

            if (!resetHorizontal)
            {
                lastHorizontal = horizontal;
                movement.velocity.x = horizontal * walkSpeed;
            }

            if (applyGravity.grounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    movement.velocity.y = jumpHeight;
                }

            }
        }

        private void FixedUpdate()
        {

        }


        public void DoorCollection()
        {
            if (Input.GetKeyDown(KeyCode.D))
                _lookDirection = LookDirection.Right;
            if (Input.GetKeyDown(KeyCode.A))
                _lookDirection = LookDirection.Left;
        }

        bool IsGrounded()
        {
            var bounds = this.GetComponent<CapsuleCollider2D>().bounds;
            Vector2 position = new Vector2(bounds.center.x, bounds.min.y);
            Vector2 direction = Vector2.down;

            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }

        public bool PlayerStands()
        {
            return movement.velocity.x == 0;
        }

        public bool PlayerIsStill()
        {
            return PlayerStands() && IsGrounded();
        }

        public void OnMovingPlatform(bool onThePlatform)
        {
            _onMovingPlatform = onThePlatform;
        }

        public void AddCoins(int number)
        {
            _coinNumber += number;
        }

        public void ExitPlatformVelocity(float velocity, float velocityX, float velocityY)
        {
            // //if (Input.GetAxis("Horizontal") == 0f)
            // //{
            // _addedVelocity = velocity;
            // _addedVelocityX = velocityX;
            // _addedVelocityY = velocityY;
            // //Debug.Log(Input.GetAxis("Horizontal"));
            // // }
            // 
            // Debug.Log(_addedVelocity);
        }
    }
}
