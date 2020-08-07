using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class ApplyGravityComponent : MonoBehaviour
    {
        public bool grounded = false;
        public float gravity = 10f;
        public float distance = 0.1f;
        public LayerMask groundLayer;
        MovementComponent movement;

        // Start is called before the first frame update
        void Start()
        {
            movement = GetComponent<MovementComponent>();    
        }

        // Update is called once per frame
        void Update()
        {
            
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

        private void FixedUpdate()
        {
            grounded = movement.velocity.y < float.Epsilon && IsGrounded();

            if (grounded)
            {
                movement.velocity.y = 0f;
            }
            else
            {
                movement.yAcceleration = -10f * gravity;
            }
        }
    }
}
