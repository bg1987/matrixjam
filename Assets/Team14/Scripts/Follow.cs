using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team14
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool useStartOffset;

        private void Start()
        {
            if (useStartOffset)
                offset = transform.position - target.position;
            else
                SetPosWithOffset();
        }

        private void SetPosWithOffset()
        {
            var pos = target.position + offset;
            transform.position = pos;
        }

        private void LateUpdate()
        {
            SetPosWithOffset();
        }
    }
}
