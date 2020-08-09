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

        // Changed to public so panel can access it!!!
        // IS BAD!!
        //
        // TODO: Create event when switching material
        public FloopableMaterialTypes currentMaterial = FloopableMaterialTypes.Reflective;
        private LineRenderer fireTrail;
        private float rayCastLength = 100;

        void Start()
        {
            fireTrail = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Material1"))
            {
                currentMaterial = FloopableMaterialTypes.Reflective;
            }
            if (Input.GetButtonDown("Material2"))
            {
                currentMaterial = FloopableMaterialTypes.Opaque;
            }
            if (Input.GetButtonDown("Material3"))
            {
                currentMaterial = FloopableMaterialTypes.GoThrough;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        void Shoot()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, rayCastLength, 1 << floopableLayer);

            // What? Why is RaycastHit2D acting as a bool ???
            if (hitInfo)
            {
                StartCoroutine(DrawRay(hitInfo.point));
                var floopable = hitInfo.transform.GetComponent<Floopable>();
                floopable.ChangeMaterial(currentMaterial);
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