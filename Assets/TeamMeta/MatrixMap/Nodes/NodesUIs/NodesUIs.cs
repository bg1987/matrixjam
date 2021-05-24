using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodesUIs : MonoBehaviour
    {
        [SerializeField] NodeUI nodeUiPrefab;
        [SerializeField] MatrixTraveler matrixTraveler;

        public List<NodeUI> uis { get; private set; } = new List<NodeUI>();
        private void Awake()
        {
            //Deactivate();
        }

        public void Init(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var nodeUI = Instantiate(nodeUiPrefab, transform);
                nodeUI.SetLineStartColor(nodes[i].ColorHdr1);
                uis.Add(nodeUI);
            }
            Deactivate();

        }
        public void Deactivate()
        {
            foreach (var ui in uis)
            {
                ui.Deactivate();
            }
        }
        public NodeUI ActivateNodeUI(Node node)
        {
            var nodeUI = UpdateNodeUiTextAndPosition(node);
            nodeUI.DisappearInstantly();
            return nodeUI;
        }
        NodeUI UpdateNodeUiTextAndPosition(Node node)
        {

            var nodeUI = uis[node.Index];
            nodeUI.Activate();

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

                nodeUI.SetNodeData(name, visitsCount, DiscoveredEdgesCount, totalEdgesCount);
            }
            else
                nodeUI.SetNodeData("Test Game Name", 2, 5, 9);

            nodeUI.PositionAroundNode(mapCenter: Vector3.zero, node);

            return nodeUI;
        }
        public void NodeUiActivateAndAppear(Node node, float delay, bool shouldFadeOverallAlpha, NodeUiAppearParameters appearParameters)
        {
            //This wrapper for nodeUI.Appear() is necessary for populating nodeUI text and position before letting it appear
            StartCoroutine(NodeUiActivateAndAppearRoutine(node, delay, shouldFadeOverallAlpha, appearParameters));
        }
        IEnumerator NodeUiActivateAndAppearRoutine(Node node, float delay, bool shouldFadeOverallAlpha, NodeUiAppearParameters appearParameters)
        {
            yield return new WaitForSeconds(delay);
            var nodeUI = ActivateNodeUI(node);
            nodeUI.Appear(shouldFadeOverallAlpha, appearParameters);
        }
    }
}
