using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] Camera transitionCamera;
        [SerializeField] LayerMask selectableMask;
        [SerializeField] Raycaster raycaster;
        ISelectable hoveredSelectable;
        ISelectable selectedSelectable;

        [SerializeField] NodeSelector nodeSelector;

        // Start is called before the first frame update
        void Awake()
        {
            //raycaster.OnHover += HandleHover;
            raycaster.OnClickDown += HandleSelect;
            raycaster.OnHoverEnter += HandleHoverEnter;
            raycaster.OnHoverExit += HandleHoverExit;
            Deactivate();
        }

        private void HandleHoverEnter(GameObject target)
        {
            ISelectable hoverTarget = target.GetComponent<ISelectable>();
            if(hoverTarget == null)
            {
                return;
            }

            if (hoverTarget is NodeSelectable)
            {
                nodeSelector.HandleHoverEnter(hoverTarget as NodeSelectable);
            }
        }
        private void HandleHoverExit(GameObject target)
        {
            ISelectable hoverTarget = target.GetComponent<ISelectable>();
            if (hoverTarget == null)
            {
                return;
            }

            if (hoverTarget is NodeSelectable)
            {
                nodeSelector.HandleHoverExit(hoverTarget as NodeSelectable);
            }
        }

        private void HandleSelect(GameObject target)
        {
            if (selectedSelectable == null && target == null)
                return;
            if (target == null)
            {
                nodeSelector.HandleSelect(null);
                selectedSelectable = null;

                return;
            }
            ISelectable selectTarget = target.GetComponent<ISelectable>();
            
            if (selectedSelectable == selectTarget)
                return;

            selectedSelectable = selectTarget;

            if (selectTarget == null)
            {
                nodeSelector.HandleSelect(null);
                return;
            }
            if (selectTarget is NodeSelectable)
            {
                nodeSelector.HandleSelect(selectTarget as NodeSelectable);
            }
        }
        public void Activate()
        {
            raycaster.enabled = true;
        }
        public void Deactivate()
        {
            raycaster.enabled = false;
            Reset();
        }
        private void Reset()
        {
            nodeSelector.Reset();
        }
    }
}
