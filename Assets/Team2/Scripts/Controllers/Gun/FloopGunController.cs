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
        [SerializeField] private Gradient floopableLaserColor;
        [SerializeField] private Gradient nonFloopableLaserColor;
        [SerializeField] private int broniLayer;


        // TODO: Ideally currentMaterial should be private and we should create event when switching material
        public FloopableMaterialTypes currentMaterial = FloopableMaterialTypes.Reflective;
        private LineRenderer fireTrail;
        private float rayCastLength = 100;

        void Start()
        {
            fireTrail = GetComponent<LineRenderer>();
        }

        void Update()
        {
            // TODO: Ideally should work with dictionary<int,matType> - GetButton(Material{int})->currentMat=dict[int]
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
            // ~(1 << layer) means "All but layer"
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, rayCastLength, ~(1 << broniLayer));

            // What? Why is RaycastHit2D acting as a bool ???
            if (hitInfo)
            {
                var floopable = hitInfo.transform.GetComponent<Floopable>();
                Gradient rayColor;
                if (floopable != null)
                {
                    floopable.ChangeMaterial(currentMaterial);
                    rayColor = floopableLaserColor;
                }
                else
                {
                    rayColor = nonFloopableLaserColor;
                }
                StartCoroutine(DrawRay(hitInfo.point, rayColor));
            }
        }

        IEnumerator DrawRay(Vector2 hitPoint, Gradient color)
        {
            fireTrail.colorGradient = color;
            fireTrail.SetPosition(0, firePoint.position);
            fireTrail.SetPosition(1, hitPoint);
            fireTrail.enabled = true;
            yield return new WaitForSeconds(0.02f);
            fireTrail.enabled = false;
        }
    }
}