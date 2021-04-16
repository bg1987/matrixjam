using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodeSelector : MonoBehaviour
    {
        NodeSelectable selectedNode;
        NodeSelectable hoveredNode;
        // Start is called before the first frame update
        void Start()
        {
            
        }
        public void Reset()
        {
            HandleUnselect();
            if(hoveredNode)
                hoveredNode.HoverExit();
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
            }
        }
        void HandleUnselect()
        {
            if (selectedNode == null)
                return;
            selectedNode.Unselect();
            selectedNode = null;
        }
    }
}
