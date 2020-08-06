using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float crouchSpeed = 5;
        [SerializeField] private float jumpForce = 2;

        private Rigidbody2D rb;

        private float horizontalInput = 0;
        private bool jumpInput;
        private bool isGrounded = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            jumpInput = Input.GetButton("Jump");
        }

        void FixedUpdate()
        {
            Move(horizontalInput);
            Jump(jumpInput);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            isGrounded = true;
        }
        void OnCollisionExit2D(Collision2D other)
        {
            isGrounded = false;
        }

        private void Move(float horizontalDir)
        {
            rb.velocity = new Vector2(horizontalDir * movementSpeed, rb.velocity.y);
        }

        private void Jump(bool jumpInput)
        {
            if (!jumpInput || !isGrounded) return;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}