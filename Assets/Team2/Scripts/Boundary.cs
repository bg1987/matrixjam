using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team2
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Boundary : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        void OnValidate()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, Vector2.Scale(transform.localScale, boxCollider.size));
        }
    }
}
