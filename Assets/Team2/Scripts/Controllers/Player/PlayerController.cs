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
        [SerializeField] private int maxJumps = 1;

        private Rigidbody2D rb;

        private bool isGrounded = false;
        private int currentJumpsCount = 0;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            bool jumpInput = Input.GetButtonDown("Jump");
            Move(horizontalInput);
            Jump(jumpInput);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            isGrounded = true;
            currentJumpsCount = 0;
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
            if (!jumpInput || !CanJump) return;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpsCount++;
        }

        private bool CanJump => isGrounded || currentJumpsCount < maxJumps;
    }
}