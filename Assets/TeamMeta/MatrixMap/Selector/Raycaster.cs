using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] Camera mapCamera;
        [SerializeField] LayerMask hittableMask;

        public System.Action<GameObject> OnHover;
        public System.Action<GameObject> OnHoverEnter;
        public System.Action<GameObject> OnHoverExit;
        public System.Action<GameObject> OnClickDown;
        public System.Action<GameObject> OnRightClickDown;

        GameObject hoveredObject;
        private void Awake()
        {
        }
        // Update is called once per frame
        void Update()
        {

            GameObject hitObject = ShootRay();
            if (Input.GetMouseButtonDown(0))
            {
                OnClickDown?.Invoke(hitObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnRightClickDown?.Invoke(hitObject);
            }
            OnHover?.Invoke(hitObject);

            if (hoveredObject != hitObject)
            {
                if(hitObject)
                    OnHoverEnter?.Invoke(hitObject);
                if (hoveredObject)
                    OnHoverExit?.Invoke(hoveredObject);
            }

            hoveredObject = hitObject;
        }
        GameObject ShootRay()
        {
            GameObject hitObject = null;

            var tempRenderTexture = mapCamera.targetTexture;
            mapCamera.targetTexture = null;
            Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
            mapCamera.targetTexture = tempRenderTexture;

            if (Physics.Raycast(ray, out RaycastHit hit, mapCamera.farClipPlane, hittableMask))
            {
                hitObject = hit.collider.gameObject;
            }

            return hitObject;
        }
        private void OnDisable()
        {
            hoveredObject = null;
        }
    }
}
