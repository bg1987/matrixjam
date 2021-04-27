using System;
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
        [SerializeField] NodeUI selectedNodeUI;
        [SerializeField] NodesUIs nodeUis;
        [SerializeField] MatrixTraveler matrixTraveler;
        // Start is called before the first frame update
        void Start()
        {
        }
        public void Reset()
        {

            selectedNodeUI = null;
            overlay.Deactivate();
            if (selectedNode)
            {
                selectedNode.Unselect();
                selectedNode = null;
            }
            nodeUis.Deactivate();

            if (hoveredNode)
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
            if(selectedNode == null)
            {
                if (target != null)
                    Select(target);
            }
            else if(selectedNode == target)
            {

            }
            else if (target!=null)
            {
                ReplaceSelectedNode(target);
            }
            else if (target == null)
            {
                Unselect();
            }
        }

        private void Unselect()
        {
            UnfocusNode();
            selectedNode.Unselect();
            selectedNode = null;
            overlay.Deactivate();
            selectedNodeUI.Disappear(true);
            selectedNodeUI = null;
        }

        private void Select(NodeSelectable target)
        {
            selectedNode = target;
            target.Select();
            FocusNode();

            //UI
            overlay.Activate(); //Todo Make into a fade
            selectedNodeUI = nodeUis.uis[selectedNode.GetNode().Index];
            UpdateNodeUiTextAndPosition();
            selectedNodeUI.Appear(true);
        }
        void ReplaceSelectedNode(NodeSelectable target)
        {
            UnfocusNode();
            selectedNode.Unselect();
            selectedNodeUI.Disappear(false);
         
            selectedNode = target;
            selectedNode.Select();
            FocusNode();

            selectedNodeUI = nodeUis.uis[selectedNode.GetNode().Index];
            UpdateNodeUiTextAndPosition();
            selectedNodeUI.Appear(false);
        }
        void UpdateNodeUiTextAndPosition()
        {
            Node node = selectedNode.GetNode();

            var nodeUI = nodeUis.uis[node.Index];

            selectedNodeUI = nodeUI;
            selectedNodeUI.Activate();

            if (matrixTraveler != null)
            {
                MatrixNodeData nodeData = matrixTraveler.matrixGraphData.nodes[node.Index];
                MatrixTravelHistory travelData = matrixTraveler.travelData;
                travelData.TryGetLastTravel(out var lastTravelData);
                bool isSelectedNodeDestinationNode = false;
                if (lastTravelData.endPort.nodeIndex > -1)
                {
                    if (lastTravelData.endPort.nodeIndex == node.Index)
                        isSelectedNodeDestinationNode = true;
                }

                string name = nodeData.name;
                int visitsCount = matrixTraveler.travelData.GetGameVisitCount(nodeData);
                if (isSelectedNodeDestinationNode)
                    visitsCount -= 1;

                int DiscoveredEdgesCount = matrixTraveler.travelData.GetGameVisitedEdgesCount(nodeData.index);
                int totalEdgesCount = nodeData.outputPorts.Count;

                selectedNodeUI.SetNodeData(name, visitsCount, DiscoveredEdgesCount, totalEdgesCount);
            }
            else
                selectedNodeUI.SetNodeData("Test Game Name", 2, 5, 9);

            selectedNodeUI.PositionAroundNode(mapCenter: Vector3.zero, node);
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
