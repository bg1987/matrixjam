using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
    {
        namespace MatrixJam.Team2
{
    public class RotateTowardsPoint : MonoBehaviour
        {
            [SerializeField] private Transform target;

            private Vector3 offset;

            void Start()
            {
                offset = new Vector3(transform.position.x - transform.parent.position.x, 0, 0);
            }

            void Update()
            {
                Vector3 targetPos = target.position;

                Vector3 aimDir = (targetPos - transform.parent.position);
                float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.position = transform.parent.position + transform.rotation * offset;
            }
        }
    }
}
