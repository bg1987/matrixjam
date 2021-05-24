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
        [Header("Overlay Appearance")]
        [SerializeField] private float overlayFadeOutDuration = 0.2f;
        [SerializeField] private float overlayFadeInDuration = 0.2f;

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
            overlay.Deactivate();
            if (hoveredNode)
                hoveredNode.HoverExit();
            if (selectedNode)
                UnfocusNode(0);
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
            UnfocusNode(overlayFadeOutDuration);
            selectedNode.Unselect();
            selectedNode = null;
            selectedNodeUI.Disappear(true);
            selectedNodeUI = null;

            overlay.Disappear(overlayFadeOutDuration);

        }

        private void Select(NodeSelectable target)
        {
            selectedNode = target;
            target.Select();
            FocusNode();

            //UI
            //overlay.Activate(); //Todo Make into a fade
            overlay.Appear(overlayFadeInDuration);

            selectedNodeUI = nodeUis.uis[selectedNode.GetNode().Index];
            UpdateNodeUiTextAndPosition();
            selectedNodeUI.Appear(true);
        }
        void ReplaceSelectedNode(NodeSelectable target)
        {
            UnfocusNode(0);
            selectedNode.Unselect();
            selectedNodeUI.Disappear(false);
         
            selectedNode = target;
            selectedNode.Select();
            FocusNode();

            selectedNodeUI = nodeUis.uis[selectedNode.GetNode().Index];
            UpdateNodeUiTextAndPosition();
            selectedNodeUI.OverallAlphaAppearInstantly();
            selectedNodeUI.Appear(false);
        }
        void UpdateNodeUiTextAndPosition()
        {
            Node node = selectedNode.GetNode();
            selectedNodeUI = nodeUis.ActivateNodeUI(node);
        }

        void FocusNode()
        {
            var localPos = selectedNode.transform.parent.localPosition;
            localPos.z = -1;
            selectedNode.transform.parent.localPosition = localPos;
        }
        void UnfocusNode(float duration)
        {
            var localPos = selectedNode.transform.parent.localPosition;
            var startPos = localPos;
            startPos.z = -1;
            var endPos = localPos;
            endPos.z = 0;

            if(duration == 0)
            {
                UnfocusNodeExecute(1, startPos, endPos, selectedNode.transform.parent);
                return;
            }
            StartCoroutine(UnfocusNodeRoutine(duration, startPos, endPos, selectedNode.transform.parent));
        }
        IEnumerator UnfocusNodeRoutine(float duration, Vector3 startPosition, Vector3 targetPosition, Transform nodeTransform)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / duration;

                UnfocusNodeExecute(t, startPosition, targetPosition, nodeTransform);
                yield return null;

                if (nodeTransform.localPosition == startPosition)
                    yield break;
            }
            UnfocusNodeExecute(1, startPosition,targetPosition, nodeTransform);
        }
        void UnfocusNodeExecute(float t, Vector3 start, Vector3 end, Transform nodeTransform)
        {
            var position = Vector3.Lerp(start, end, t);
            nodeTransform.localPosition = position;
        }
    }
}
