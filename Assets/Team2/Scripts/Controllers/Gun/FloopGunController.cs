using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(LineRenderer))]
    public class FloopGunController : MonoBehaviour
    {
        // Floop / Flooper / Triple floop / FloopGun / FloopTron / FloopMizer
        [SerializeField] private Transform firePoint;
        [SerializeField] private int floopableLayer;

        private LineRenderer fireTrail;
        private float rayCastLength = 100;

        void Start()
        {
            fireTrail = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        void Shoot()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, mousePosition - firePoint.position, rayCastLength, 1 << floopableLayer);

            // What? Why is RaycastHit2D acting as a bool ???
            if (hitInfo)
            {
                StartCoroutine(DrawRay(hitInfo.point));
            }
        }

        IEnumerator DrawRay(Vector2 hitPoint)
        {
            fireTrail.SetPosition(0, firePoint.position);
            fireTrail.SetPosition(1, hitPoint);
            fireTrail.enabled = true;
            yield return new WaitForSeconds(0.02f);
            fireTrail.enabled = false;
        }
    }
}