using UnityEngine;

namespace TheFlyingDragons
{
    public class PlayerInputListener : MonoBehaviour
    {
        public PlayerInputData data;

        /*public void OnMove(InputAction.CallbackContext context)
        {
            data.move = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            data.jump = context.ReadValueAsButton();
            if (data.jump)
            {
                data.jumpReady = true;
                data.jumpTime = Time.time;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            data.crouch = context.ReadValueAsButton();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            data.fire = context.ReadValueAsButton();
        }

        public void OnRope(InputAction.CallbackContext context)
        {
            data.rope = context.ReadValueAsButton();
        }

        public void OnRopeShorter(InputAction.CallbackContext context)
        {
            data.ropeShorter = context.ReadValueAsButton();
        }

        public void OnRopeLonger(InputAction.CallbackContext context)
        {
            data.ropeLonger = context.ReadValueAsButton();
        }
        */
    }
}
