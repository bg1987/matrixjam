using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovementComponent : MonoBehaviour
    {
        public Vector2 velocity = Vector2.zero;
        bool grounded = false;
        public float gravity = 10f;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if (!grounded)
                velocity.y -= gravity * 10f * Time.fixedDeltaTime;
            this.transform.Translate(velocity * Time.fixedDeltaTime);
        }
    }
}
