using UnityEngine;

namespace TheFlyingDragons
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour
    {
        Player player;
        Vector3 velocity;
		float lastJump;

		public PlayerConfig PlayerConfig => player.playerConfig;
		public PlayerInputData Input => player.Input;
		public Rigidbody Body => player.Body;
		
		void Awake()
        {
            player = GetComponent<Player>();
		}

		void FixedUpdate()
        {
			if (true/*player.IsGrounded || PlayerConfig.airControl*/)
			{
				float horizontalMove = Input.move.x * PlayerConfig.moveSpeed.x * Time.fixedDeltaTime;
				float verticalMove = Input.move.y * PlayerConfig.moveSpeed.y * Time.fixedDeltaTime;

				/*if (player.IsCrouching)
				{
					horizontalMove *= PlayerConfig.crouchSpeed.x;
					player.head.Collider.isTrigger = true;
				}
				else // Not crouching
				{
					player.head.Collider.isTrigger = false;
				}*/

				if (PlayerConfig.moveEnabled)
				{
					Vector3 targetVelocity = new Vector3(horizontalMove, PlayerConfig.flipMovement ? Body.velocity.y : verticalMove, PlayerConfig.flipMovement ? -verticalMove : Body.velocity.z);
					Body.velocity = Vector3.SmoothDamp(Body.velocity, targetVelocity, ref velocity, PlayerConfig.movementSmoothing);
				}
			}

			//bool groundedFuzzy = player.IsGrounded || player.IsGroundedFuzzy;
			bool isJumping = Input.jump || player.IsJumpingFuzzy;
			if (PlayerConfig.jumpEnabled && isJumping/* && groundedFuzzy*/)
			{
				if ((Time.time - lastJump) > PlayerConfig.jumpCooldown)
				{
					lastJump = Time.time;
					Body.AddForce(new Vector2(0, PlayerConfig.jumpForce));
				}
				Input.jumpReady = false;
				Input.jump = false;
			}
		}

	}
}
