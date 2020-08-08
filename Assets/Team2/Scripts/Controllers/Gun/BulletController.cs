using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        void Update()
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
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
