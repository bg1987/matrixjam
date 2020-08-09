using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float crouchSpeed = 5;
        [SerializeField] private float jumpForce = 2;
        [SerializeField] private float walkAnimSpeedThresh = 0.1f;
        [SerializeField] private int maxJumps = 1;

        private Animator playerAnimator;
        private Rigidbody2D rb;
        private bool isGrounded = false;
        private int currentJumpsCount = 0;
        private Checkpoint checkpoint;

        void Awake()
        {
            playerAnimator = gameObject.GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            bool jumpInput = Input.GetKeyDown(KeyCode.Space);
            Move(horizontalInput);
            Jump(jumpInput);

            if (Input.GetKeyDown(KeyCode.G))
            {
                ReturnToCheckpoint();
            }
            AnimWalk();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Checkpoint checkpoint))
            {
                checkpoint.GetComponent<Collider2D>().enabled = false;
                this.checkpoint = checkpoint;
            }

            GoThroughController goThroughController;
            if (other.TryGetComponent<GoThroughController>(out goThroughController))
            {
                if (goThroughController.enabled) return;
            }

            isGrounded = true;
            currentJumpsCount = 0;
        }
        void OnTriggerExit2D(Collider2D other)
        {
            isGrounded = false;
        }

        public void ReturnToCheckpoint()
        {
            if (checkpoint != null)
            {
                for (int i = 0; i < checkpoint.transform.childCount; i++)
                {
                    checkpoint.transform.GetChild(i).gameObject.SetActive(true);
                }
                transform.position = checkpoint.transform.position;
            }
        }

        private void Move(float horizontalDir)
        {
            rb.AddForce(Vector2.right * horizontalDir * movementSpeed, ForceMode2D.Force);
        }

        private void Jump(bool jumpInput)
        {
            if (!jumpInput || !CanJump) return;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumpsCount++;
        }

        private bool CanJump => isGrounded || currentJumpsCount < maxJumps;

        private void AnimWalk()
        {
            if (Mathf.Abs(rb.velocity.x) > walkAnimSpeedThresh
                && !playerAnimator.GetBool("walk"))
            {
                playerAnimator.SetBool("walk", true);
            }

            else if (Mathf.Abs(rb.velocity.x) < walkAnimSpeedThresh
                     && playerAnimator.GetBool("walk"))
            {
                playerAnimator.SetBool("walk", false);
            }
        }
    }
}