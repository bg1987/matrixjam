using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            // כוס אמא של rb.MovePosition
            // שתקע אותי במשך שעה במקום שאני אהיה עז עם הגיון שתשתמש ב 
            // rb.position
            // תודה ביי
            rb.position += (Vector2)(transform.right * movementSpeed * Time.deltaTime);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            PlayerController playerController;
            Floopable floopable;
            if (other.gameObject.TryGetComponent(out playerController))
            {
                // playerController.Kill();
                return;
            }
            if (other.gameObject.TryGetComponent(out floopable))
            {
                floopable.ChangeBulletTrajectory(this.transform, other.contacts[0]);
                return;
            }

            Destroy(gameObject);
        }
    }
}