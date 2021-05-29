using MatrixJam.TeamMeta.MatrixMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class NodesCreditsAppearance : MonoBehaviour
    {
        [SerializeField] NodeFirstVisitEffect nodeAppearEffectPrefab;

        [SerializeField] private float nodeAppearDuration = 0.5f;
        [SerializeField] private float delayBetweenNodes = 0.5f;
        [SerializeField] private NodeUiAppearParameters nodeUisAppearParameters;
        [SerializeField] private float nodeUisDelay = 0.5f;
        [SerializeField] private float nodeUisDistanceFromNodeStart = 0.0f;
        [SerializeField] private float hnodeUisDistanceFromNodeEnd = 1.6f;
        [SerializeField] private float nodeUisMinDistanceFromNode = 0.8f;
        public void Appear(List<int> nodeIdsSequence, Nodes nodesController, NodesUIs nodesUIs)
        {
            StartCoroutine(AppearRoutine(nodeIdsSequence, nodesController, nodesUIs));
        }
        IEnumerator AppearRoutine(List<int> nodeIdsSequence, Nodes nodesController, NodesUIs nodesUIs)
        {
            SortedSet<int> alreadyAppearedNodesIndexes = new SortedSet<int>();

            List<Node> nodes = nodesController.nodes;
            for (int i = 0; i < nodeIdsSequence.Count; i++)
            {
                var node = nodes[nodeIdsSequence[i]];
                if (alreadyAppearedNodesIndexes.Contains(node.Index))
                {
                    continue;
                }
                node.gameObject.SetActive(true);

                node.Appear(nodeAppearDuration, 0);
                alreadyAppearedNodesIndexes.Add(node.Index);

                //Visual effects
                node.NodeSelectable.HoverEnter();

                NodeFirstVisitEffect visitEffect = Instantiate(nodeAppearEffectPrefab, transform);
                visitEffect.Play(node, 0);
                Destroy(visitEffect.gameObject, visitEffect.CalculateEffectDuration());
                //End visual effects

                var nodeUI = nodesUIs.uis[node.Index];
                nodeUI.Activate();

                var nodeData = MatrixTraveler.Instance.matrixGraphData.nodes[node.Index];

                nodeUI.SetNodeCreditData(nodeData.teamMembers);
                nodeUI.DisappearInstantly();

                float distanceFromNodeT = 1 - (Mathf.Cos(node.transform.position.x / nodesController.Radius * Mathf.PI) + 1) / 2f;

                float distanceFromNode = Mathf.Lerp(nodeUisDistanceFromNodeStart, hnodeUisDistanceFromNodeEnd, distanceFromNodeT);
                if (distanceFromNode < nodeUisMinDistanceFromNode)
                    distanceFromNode = nodeUisMinDistanceFromNode;
                nodeUI.PositionAroundNode(Vector3.zero, node, distanceFromNode);

                nodeUI.Appear(true, nodeUisAppearParameters);

                yield return new WaitForSeconds(delayBetweenNodes);
            }

            //Might make use of the following v

            //float delay = CalculateDelayBetweenNodeAppearances();
            //previousActiveNodeMarker Appearance
            //if (visitedNodesIndexesSorted.Count > 1)
            //{
            //    previousActiveNodeMarkerAppearance(indexCount * delay);
            //}
        }
        public float CalculateAppearanceTime(int nodesCount)
        {
            float totalNodesAppearanceTime = 0;
            totalNodesAppearanceTime += (nodesCount - 1) * delayBetweenNodes;
            totalNodesAppearanceTime += nodeAppearDuration;
            //totalNodesAppearanceTime += totalNodesAppearanceTime;

            return totalNodesAppearanceTime;
        }
    }
}
