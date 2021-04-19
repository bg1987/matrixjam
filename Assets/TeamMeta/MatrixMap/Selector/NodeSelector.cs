using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodeSelector : MonoBehaviour
    {
        NodeSelectable selectedNode;
        NodeSelectable hoveredNode;

        [SerializeField] Overlay overlay;
        [SerializeField] SelectedNodeUI nodeUI;
        // Start is called before the first frame update
        void Start()
        {
            DeactivateUI();
        }
        public void Reset()
        {
            DeactivateUI();
            HandleUnselect();
            if(hoveredNode)
                hoveredNode.HoverExit();
            if (selectedNode)
                UnfocusNode();
        }
        public void HandleHoverEnter(NodeSelectable target)
        {
            target.HoverEnter();
            hoveredNode = target;
        }
        public void HandleHoverExit(NodeSelectable target)
        {
            target.HoverExit();
            if(hoveredNode == target)
                hoveredNode = null;
        }
        public void HandleSelect(NodeSelectable target)
        {
            HandleUnselect();
            if (target != null)
            {
                selectedNode = target;
                target.Select();
                ActivateUI();
                FocusNode();
            }
            else
            {
                DeactivateUI();
            }
        }
        void HandleUnselect()
        {
            if (selectedNode == null)
                return;
            UnfocusNode();
            selectedNode.Unselect();
            selectedNode = null;
        }
        void ActivateUI()
        {
            overlay.Activate();

            nodeUI.Activate();
            nodeUI.SetNodeData("Hello", 2, 5, 9);
        }
        void DeactivateUI()
        {
            overlay.Deactivate();

            nodeUI.deactivate();
        }
        void FocusNode()
        {
            var localPos = selectedNode.transform.parent.localPosition;
            localPos.z = -1;
            selectedNode.transform.parent.localPosition = localPos;
        }
        void UnfocusNode()
        {
            var localPos = selectedNode.transform.parent.localPosition;
            localPos.z = 0;
            selectedNode.transform.parent.localPosition= localPos;
        }
    }
}
