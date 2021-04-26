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
        [SerializeField] EdgeSelector edgeSelector;

        // Start is called before the first frame update
        void Awake()
        {
            //raycaster.OnHover += HandleHover;
            raycaster.OnClickDown += HandleSelect;
            raycaster.OnHoverEnter += HandleHoverEnter;
            raycaster.OnHoverExit += HandleHoverExit;
        }
        private void Start()
        {
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
            else if (hoverTarget is EdgeSelectable)
            {
                edgeSelector.HandleHoverEnter(hoverTarget as EdgeSelectable);
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
            else if (hoverTarget is EdgeSelectable)
            {
                edgeSelector.HandleHoverExit(hoverTarget as EdgeSelectable);
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
            hoveredSelectable = null;
            selectedSelectable = null;
            nodeSelector.Reset();
            edgeSelector.Reset();
        }
    }
}
