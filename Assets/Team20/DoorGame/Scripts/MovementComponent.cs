using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovementComponent : MonoBehaviour
    {
        public Vector2 velocity = Vector2.zero;
        public float yAcceleration = 0f;
        public float minYVelocity = -60f;

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
            velocity.y += yAcceleration * Time.fixedDeltaTime;

            velocity.y = Mathf.Max(velocity.y, minYVelocity);

            this.transform.Translate(velocity * Time.fixedDeltaTime);
        }
    }
}
