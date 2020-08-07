using UnityEngine;

namespace TheFlyingDragons
{
    [CreateAssetMenu(menuName = "TheFlyingDragons/PlayerInputData")]
    public class PlayerInputData : ScriptableObject
    {
        public Vector2 move = Vector2.zero;
        public bool jump;
        public bool jumpReady;
        public float jumpTime;
        public bool crouch;
        [Space]
        public bool fire;
        [Space]
        public bool rope;
        public bool ropeLonger;
        public bool ropeShorter;

        public void Reset()
        {
            move = Vector2.zero;
            jump = false;
            jumpReady = false;
            jumpTime = 0f;
            crouch = false;
            
            fire = false;

            rope = false;
            ropeLonger = false;
            ropeShorter = false;
        }
    }
}
