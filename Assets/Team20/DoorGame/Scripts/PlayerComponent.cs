using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class PlayerComponent : MonoBehaviour
    {
        public Vector2 velocity = Vector2.zero;
        bool grounded = false;
        public float gravity = 10f;
        public float walkSpeed = 10f;
        public float jumpHeight = 10f;
        //public float ignoreHorizontalFor = 0.2f;
        public bool resetHorizontal = false;
        float lastHorizontal = 0f;
        public LayerMask groundLayer;
        public float distance = 0.1f;
        [SerializeField] private bool _onMovingPlatform = false;
        public float movingPlatformFix = 1;

        [SerializeField] private float _coinNumber = 0f;

        [SerializeField] private GameObject _collectedDoor;
        [SerializeField] private int _doorNumber = 0;
        [SerializeField] private float _doorYValue = 0;
        [SerializeField] private float _doorOffset = 1f;
        private float _doorYOffset = 0f;
        private bool _doorKeyReady = true;
        private enum LookDirection
        {
            Right,
            Left
        }

        [SerializeField] private LookDirection _lookDirection;


        void Start()
        {
            
        }



        // Update is called once per frame
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            if(horizontal == 0f)
            {
                resetHorizontal = false;
            }

            if(resetHorizontal)
            {
                //horizontal = lastHorizontal;
            }
            else
            {
                lastHorizontal = horizontal;
                velocity.x = horizontal * walkSpeed;
            }

            //if (ignoreHorizontalFor <= 0f)
            /*{
                ignoreHorizontalFor = 0f;
                velocity.x = horizontal * walkSpeed;
            }*/
            /*if(resetHorizontal)
            {
                velocity.x = 0f;
            }
            else
            {
                velocity.x = horizontal * walkSpeed;
                ignoreHorizontalFor = 0f;
            }*/

            //ignoreHorizontalFor -= Time.deltaTime;
            grounded = velocity.y < float.Epsilon && IsGrounded();

            if (grounded)
            {
                velocity.y = 0f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = jumpHeight;
                    grounded = false;
                }
            }
            DoorCollection();
        }

        private void FixedUpdate()
        {
            if (!grounded)
                velocity.y -= gravity*10f * Time.fixedDeltaTime;
            if (_onMovingPlatform)
                movingPlatformFix = -1;
            else
                movingPlatformFix = 1;
            this.transform.Translate(velocity * movingPlatformFix * Time.fixedDeltaTime);
        }


        public void DoorCollection()
        {
            if (Input.GetKeyDown(KeyCode.D))
                _lookDirection = LookDirection.Right;
            if (Input.GetKeyDown(KeyCode.A))
                _lookDirection = LookDirection.Left;
            if (Input.GetKeyDown(KeyCode.E) && _doorNumber == 1 && PlayerStands())
            {
                Debug.Log("Door Down");
                _doorKeyReady = false;
                SetDoors(0);
                _collectedDoor.SetActive(true);
                Vector3 playerLocation = gameObject.transform.position;
                if (_lookDirection == LookDirection.Right)
                    _doorOffset = Mathf.Abs(_doorOffset);
                else
                    _doorOffset = -Mathf.Abs(_doorOffset);
                playerLocation.x = playerLocation.x + _doorOffset;
                playerLocation.y = playerLocation.y + _doorYOffset;
                _collectedDoor.transform.position = playerLocation;
                if (_onMovingPlatform)
                    _collectedDoor.transform.parent = gameObject.transform.parent;
            }
            if (Input.GetKeyUp(KeyCode.E) & _doorNumber == 0)
                _doorKeyReady = true;
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
            if (velocity.x == 0)
            {
                return true;
            }
            return false;
        }

        public bool PlayerIsStill()
        {
            if (PlayerStands() && IsGrounded())
            {
                return true;
            }
            return false;
        }

        public bool DoorKeyReady()
        {
            return _doorKeyReady;
        }

        public void OnMovingPlatform(bool onThePlatform)
        {
            _onMovingPlatform = onThePlatform;
        }

        public void SetDoorsY(float yValue)
        {
            _doorYValue = yValue;
        }

        public void SetDoors(int number)
        {
            _doorNumber = number;
        }

        public void AddCoins(int number)
        {
            _coinNumber += number;
        }

        public int GetDoors()
        {
            return _doorNumber;
        }

        public void CollectDoor(GameObject door)
        {
            _collectedDoor = door;
            _doorYOffset = _doorYValue - gameObject.transform.position.y;
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
