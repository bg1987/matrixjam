using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Nodes : MonoBehaviour
    {
        [SerializeField] Node nodePrefab;

        [SerializeField] int nodesCount;
        [SerializeField] float nodesSize = 1;

        public List<Node> nodes = new List<Node>();
        SortedSet<int> visitedNodesIndexesSorted = new SortedSet<int>();
        List<Vector3> nodesPositions = new List<Vector3>();


        [Header("Nodes Appear")]
        [SerializeField, Min(0)] float nodeAppearDuration = 0.2f;
        [SerializeField, Min(0)] float delayBetweenNodeAppearances = 0;
        [SerializeField, Min(0)] float totalTimeForAllNodeAppearances = 1;

        [Header("Node First Visit")]
        [SerializeField, Min(0)] public float nodesMovementDelay = 0;
        [SerializeField, Min(0)] public float nodesMovementDuration = 1;
        [SerializeField] AnimationCurve nodesMovementCurve;
        [SerializeField, Min(0)] float firstVisitNodeAppearDelay = 0.9f;
        [SerializeField, Min(0)] float firstVisitNodeAppearDuration = 0.8f;
        [SerializeField, Min(0)] float firstVisitNodeGlowDuration = 3f;
        [SerializeField] NodeFirstVisitEffect nodeFirstVisitEffect;

        [Header("Previous Active Node Marker")]
        [SerializeField] PreviousActiveNodeMarker previousActiveNodeMarker;
        [SerializeField] float previousActiveNodeMarkerDisaapearDelay = 1;

        [Header("Radius")]
        [SerializeField] float radius = 1;
        [SerializeField] float minRadius = 3;
        [SerializeField] float maxRadius = 5;

        const float TAU = Mathf.PI * 2;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void Init()
        {
            if (MatrixTraveler.Instance)
            {
                CreateNodes(MatrixTraveler.Instance);
            }
            else
            {
                CreateNodes();
            }

            CalculateNodesPositions(nodesCount);
            UpdateNodesPositions();

            foreach (var node in nodes)
            {
                node.gameObject.SetActive(false);
            }
            previousActiveNodeMarker.Init();
        }
        public List<Node> GetVisitedNodes()
        {
            var visitedNodes = new List<Node>();
            foreach (var nodeIndex in visitedNodesIndexesSorted)
            {
                var node = nodes[nodeIndex];
                visitedNodes.Add(node);
            }
            return visitedNodes;
        }
        public float CalculateTotalNodesAppearanceTime()
        {
            float totalNodesAppearanceTime = 0;
            totalNodesAppearanceTime += (visitedNodesIndexesSorted.Count - 1) * delayBetweenNodeAppearances;
            totalNodesAppearanceTime += nodeAppearDuration;
            totalNodesAppearanceTime += totalNodesAppearanceTime;

            return totalNodesAppearanceTime;
        }
        public float CalculateNodeFirstVisitTime()
        {
            float addedNodeTime = firstVisitNodeAppearDelay + firstVisitNodeAppearDuration + nodeFirstVisitEffect.CalculateEffectDuration();
            return addedNodeTime;
        }
        public float CalculateNodesMovementTime()
        {
            float nodeMovementTime = nodesMovementDelay + nodesMovementDuration;
            return nodeMovementTime;
        }
        public void Appear()
        {
            foreach (var index in visitedNodesIndexesSorted)
            {
                var node = nodes[index];
                node.gameObject.SetActive(true);
            }
            UpdateVisitedNodesPositions();

        }
        public void AppearGradually()
        {
            float delay = CalculateDelayBetweenNodeAppearances();
            int indexCount = 0;
            foreach (var nodeIndex in visitedNodesIndexesSorted)
            {
                var node = nodes[nodeIndex];

                node.Appear(nodeAppearDuration, indexCount * delay);

                indexCount++;
            }

            //previousActiveNodeMarker Appearance
            if (visitedNodesIndexesSorted.Count > 1)
            {
                previousActiveNodeMarkerAppearance(indexCount * delay);
            }
            
        }
        public void previousActiveNodeMarkerAppearance(float delay)
        {
            var previousNodeIndex = GetPreviousNodeIndex();

            var indexCount = 0;
            foreach (var nodeIndex in visitedNodesIndexesSorted)
            {
                if (previousNodeIndex == nodeIndex)
                {
                    var node = nodes[nodeIndex];
                    previousActiveNodeMarker.MarkNode(node, mapCenter: Vector3.zero);
                    previousActiveNodeMarker.Appear(nodeAppearDuration, indexCount * delay);
                    break;
                }
                indexCount++;
            }
            previousActiveNodeMarker.Disappear(nodeAppearDuration, indexCount * delay + nodeAppearDuration + previousActiveNodeMarkerDisaapearDelay);
        }
        public void Disappear()
        {
            foreach (var index in visitedNodesIndexesSorted)
            {
                Node node = nodes[index];
                node.Disappear();
            }
            previousActiveNodeMarker.Disappear(0, 0);
            previousActiveNodeMarker.UnmarkNode();
        }
        public float CalculateDelayBetweenNodeAppearances()
        {
            if (visitedNodesIndexesSorted.Count == 0)
                return 0;
            float delay = totalTimeForAllNodeAppearances / visitedNodesIndexesSorted.Count;
            delay += delayBetweenNodeAppearances;

            return delay;
        }
        public void ClearVisitedNodes()
        {
            visitedNodesIndexesSorted.Clear();
        }
        public void AddVisitedNode(int nodeIndex)
        {
            visitedNodesIndexesSorted.Add(nodeIndex);
        }
        //Destination Node related
        public void HandleDestinationNode()
        {
            int destinationNodeIndex = GetDestinationNodeIndex();
            Node destinationNode = nodes[destinationNodeIndex];

            if (IsFirstVisitToNode(destinationNodeIndex))
            {
                visitedNodesIndexesSorted.Add(destinationNodeIndex);

                ActivateNewNodeVisitEffect(destinationNode);
            }
        }
        public bool IsFirstVisitToNode(int nodeIndex)
        {
            bool isFirstVisit = !visitedNodesIndexesSorted.Contains(nodeIndex);
            return isFirstVisit;
        }
        public int GetDestinationNodeIndex()
        {
            var travelHistory = MatrixTraveler.Instance.travelData.GetHistory();
            MatrixEdgeData travelEdgeData = travelHistory[travelHistory.Count - 1];

            int destinationNodeIndex = travelEdgeData.endPort.nodeIndex;
            return destinationNodeIndex;
        }
        public int GetPreviousNodeIndex()
        {
            var travelHistory = MatrixTraveler.Instance.travelData.GetHistory();
            MatrixEdgeData travelEdgeData = travelHistory[travelHistory.Count - 1];

            int nodeIndex = travelEdgeData.startPort.nodeIndex;
            return nodeIndex;
        }
        void ActivateNewNodeVisitEffect(Node node)
        {
            CalculateNodesPositions(visitedNodesIndexesSorted.Count);

            node.gameObject.SetActive(true);
            node.Disappear();
            node.Appear(firstVisitNodeAppearDuration, firstVisitNodeAppearDelay);
            node.Glow(firstVisitNodeGlowDuration, firstVisitNodeAppearDelay);

            MoveNodesToPositions();

            nodeFirstVisitEffect.Play(node, firstVisitNodeAppearDelay);
        }
        //Node Creation
        public void CreateNodes(MatrixTraveler matrixTraveler)
        {
            var nodesData = matrixTraveler.matrixGraphData.nodes;
            nodes.Capacity = nodesData.Count;
            foreach (var nodeData in nodesData)
            {
                int i = nodeData.index;

                var node = CreateNode();
                node.name += i;
                node.SetIndex(i);

                node.SetModelColors(nodeData.colorHdr1, nodeData.colorHdr2);

                nodes.Insert(i, node);
            }
            nodesCount = nodesData.Count;
        }
        public void CreateNodes()
        {
            for (int i = 0; i < nodesCount; i++)
            {
                var node = CreateNode();
                node.SetIndex(i);
                nodes.Add(node);
            }

        }
        Node CreateNode()
        {
            var node = Instantiate(nodePrefab, transform);
            node.name = "Node";
            node.SetModelRadius(nodesSize);
            foreach (var childTransform in node.GetComponentsInChildren<Transform>())
            {
                childTransform.gameObject.layer = transform.gameObject.layer;

            }
            return node;
        }
        //Node Positioning
        private List<Vector3> CalculateNodesPositions(int nodesCount)
        {
            nodesPositions.Clear();

            float rotateOffset = -Mathf.PI / 2f; //90 degrees. For visual symmetry

            for (int i = 0; i < nodesCount; i++)
            {
                float t = i / (float)nodesCount;
                float rotateBy = t * TAU + rotateOffset;
                nodesPositions.Add(new Vector3(Mathf.Cos(rotateBy), Mathf.Sin(rotateBy), transform.position.z));
            }

            AddRadiusToPositions(nodesPositions);
            CenterPositions();

            return nodesPositions;
        }
        public void UpdateNodesPositions()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                node.transform.position = nodesPositions[i];

            }
        }
        void UpdateVisitedNodesPositions()
        {
            int i = 0;
            foreach (var index in visitedNodesIndexesSorted)
            {
                var node = nodes[index];
                node.transform.position = nodesPositions[i];
                i++;
            }

        }
        private void AddRadiusToPositions(List<Vector3> positions)
        {
            radius = Mathf.Lerp(minRadius, maxRadius, visitedNodesIndexesSorted.Count / (float)nodes.Count);
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] *= radius;
            }
        }
        private void CenterPositions()
        {
            float minNodeHeight = nodesPositions[0].y;
            float maxNodeHeight = minNodeHeight;

            for (int i = 1; i < nodesPositions.Count; i++)
            {
                if (maxNodeHeight < nodesPositions[i].y)
                {
                    maxNodeHeight = nodesPositions[i].y;
                }
            }
            float newMapHeight = (-minNodeHeight - maxNodeHeight) / 2f;

            for (int i = 0; i < nodesPositions.Count; i++)
            {
                var nodesPosition = nodesPositions[i];
                nodesPosition.y += newMapHeight;
                nodesPositions[i] = nodesPosition;
            }
            //transform.position = new Vector3(0, newMapHeight, 0);
        }

        //Movement
        void MoveNodesToPositions()
        {
            StartCoroutine(MoveNodesToPositionsRoutine());
        }
        IEnumerator MoveNodesToPositionsRoutine()
        {
            yield return new WaitForSeconds(nodesMovementDelay);

            var i = 0;
            foreach (var nodeIndex in visitedNodesIndexesSorted)
            {
                Node node = nodes[nodeIndex];
                node.MoveTo(nodesPositions[i], nodesMovementDuration, nodesMovementCurve);

                i++;
            }
        }
    }
}