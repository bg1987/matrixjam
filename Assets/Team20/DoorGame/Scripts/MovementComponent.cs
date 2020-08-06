using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovementComponent : MonoBehaviour
    {
        bool left = false;
        public float speed = 10f;
        public float jumpSpeed = 10f;
        public float maxSpeed = 10f;
        public bool grounded = true;
        Vector2 velocity = new Vector2(0f, 0f);

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            float Horizontal = Input.GetAxisRaw("Horizontal");

            transform.Translate(Horizontal * Time.deltaTime * speed, 0f, 0f);

            if (Horizontal > Mathf.Epsilon)
            {
                left = false;
            }
            else if (Horizontal < -Mathf.Epsilon)
            {
                left = true;
            }

            velocity.x = Horizontal * speed * Time.deltaTime;
            if (grounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    grounded = false;
                }
                else
                {
                    grounded = true;
                }
            }
            else
            {
                
            }
        }
    }
}
