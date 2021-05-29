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

        [SerializeField] SelectorTutorial selectorTutorial;
        // Start is called before the first frame update
        void Awake()
        {
            //raycaster.OnHover += HandleHover;
            raycaster.OnClickDown += HandleSelect;
            raycaster.OnRightClickDown += HandleRightClick;
            raycaster.OnHoverEnter += HandleHoverEnter;
            raycaster.OnHoverExit += HandleHoverExit;
        }

        private void HandleRightClick(GameObject obj)
        {
            HandleSelect(null);

            if(obj)
                HandleHoverExit(obj);
        }

        private void Start()
        {
            //Deactivate();
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
                selectorTutorial.EdgeHovered();
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
                selectorTutorial.NodeDeselected();

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
                selectorTutorial.NodeDeselected();

                return;
            }
            if (selectTarget is NodeSelectable)
            {
                nodeSelector.HandleSelect(selectTarget as NodeSelectable);
                selectorTutorial.NodeSelected();
            }
        }
        public void Activate()
        {
            raycaster.enabled = true;

            selectorTutorial.Activate();
        }
        public void Deactivate()
        {
            raycaster.enabled = false;
            selectorTutorial.Deactivate();  
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
