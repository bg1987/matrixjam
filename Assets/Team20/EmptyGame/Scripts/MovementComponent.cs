using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class MovementComponent : MonoBehaviour
    {
        Rigidbody2D rigidBody;
        bool left = false;
        public float speed = 10f;
        public float jumpSpeed = 10f;
        public float maxSpeed = 10f;
        public bool grounded = true;

        // Start is called before the first frame update
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
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

            if (grounded)
            {
                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rigidBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                    grounded = false;
                }
                else
                {
                    grounded = true;
                }
            }
            else
            {
                rigidBody.AddForce(Horizontal * Vector2.right * speed);
                rigidBody.velocity = new Vector3(Mathf.Clamp(rigidBody.velocity.x, -maxSpeed, maxSpeed), rigidBody.velocity.y);
            }
        }
    }
}
