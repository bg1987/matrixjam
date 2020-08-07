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
        public float jumpSpeed = 10f;
        public float ignoreHorizontalFor = 0.2f;
        public bool resetHorizontal = false;
        float lastHorizontal = 0f;
        public LayerMask groundLayer;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        bool IsGrounded()
        {
            var bounds = this.GetComponent<CapsuleCollider2D>().bounds;
            Vector2 position = new Vector2(bounds.center.x, bounds.min.y);
            Vector2 direction = Vector2.down;
            float distance = 1.0f;

            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
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

            ignoreHorizontalFor -= Time.deltaTime;
            grounded = velocity.y < float.Epsilon && IsGrounded();

            if (grounded)
            {
                velocity.y = 0f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = jumpSpeed;
                    grounded = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!grounded)
                velocity.y -= gravity * Time.fixedDeltaTime;

            this.transform.Translate(velocity * Time.fixedDeltaTime);
        }
    }
}
