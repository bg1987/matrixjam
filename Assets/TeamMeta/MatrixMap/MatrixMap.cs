using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class MatrixMap : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [Header("Nodes")]
        [SerializeField] Nodes nodesController;

        [Header("Edges")]
        [SerializeField] Edge edgePrefab;
        [SerializeField] List<Edge> edges = new List<Edge>();
        HashSet<int> visitedEdgesIndexes = new HashSet<int>();

        [Header("Edges Appear")]
        [SerializeField, Min(0)] float edgeAppearDelay = 0.1f;
        [SerializeField, Min(0)] float edgeAppearDuration = 1;

        [SerializeField] float sameNodeEdgesOffset = 0.2f;
        private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative

        [Header("Edge First Visit")]
        [SerializeField, Min(0)] float firstVisitEdgeAppearDelay = 1f;
        [SerializeField, Min(0)] float firstVisitEdgeAppearDuration = 0.8f;

        private IReadOnlyList<MatrixEdgeData> travelHistory; //1 positive, -1 negative
        const float TAU = Mathf.PI * 2;

        // Start is called before the first frame update
        void Start()
        {
            InitMap();
        }
        // Update is called once per frame
        
        void InitMap()
        {
            nodesController.Init();
            if (MatrixTraveler.Instance)
            {
                CreateEdges(MatrixTraveler.Instance);

                SyncWithTravelHistory();
            }
            //UpdateNodesScale();
            //UpadteNodesRadius();

            //DebugVisualizeFirstLastNodes();
            
            foreach (var edge in edges)
            {
                edge.gameObject.SetActive(false);
            }
            container.SetActive(false);

        }
        internal float CalculateTotalAppearanceTime()
        {
            float totalAppearanceTime = 0;

            float totalNodesAppearanceTime = nodesController.CalculateTotalNodesAppearanceTime();
            
            float totalEdgesAppearanceTime = 0;
            //totalEdgesAppearanceTime += (visitedNodesIndexesSorted.Count - 1) * delayBetweenNodeAppearances; // Test this
            totalEdgesAppearanceTime += totalNodesAppearanceTime;
            totalEdgesAppearanceTime += edgeAppearDelay;
            totalEdgesAppearanceTime += edgeAppearDuration;

            float nodeAdditionTime = 0;
            bool isFirstVisitToNode = nodesController.IsFirstVisitToNode(nodesController.GetDestinationNodeIndex());
            bool isFirstVisitToEdge = IsFirstVisitToEdge(GetDestinationEdgeIndex());
            if (isFirstVisitToNode)
            {
                float nodeMovementTime = nodesController.CalculateNodesMovementTime();
                float addedNodeTime = nodesController.CalculateAddedNodeTime();
                float addedEdgeTime = firstVisitEdgeAppearDelay + firstVisitEdgeAppearDuration;

                nodeAdditionTime = Mathf.Max(nodeMovementTime, addedNodeTime, addedEdgeTime);
            }
            else if (isFirstVisitToEdge)
            {
                float addedEdgeTime = firstVisitEdgeAppearDelay + firstVisitEdgeAppearDuration;

                nodeAdditionTime = addedEdgeTime;
            }

            totalAppearanceTime = Mathf.Max( totalNodesAppearanceTime, totalEdgesAppearanceTime, nodeAdditionTime);
            return totalAppearanceTime;
        }

        public void Appear()
        {
            container.SetActive(true);

            nodesController.Appear();
            
            foreach (var index in visitedEdgesIndexes)
            {
                var edge = edges[index];
                edge.gameObject.SetActive(true);
            }
            Disappear();

            UpdateEdgesAnchors();

            AppearGradually();

            bool IsFirstVisitToDestinationNode = nodesController.IsFirstVisitToNode(nodesController.GetDestinationNodeIndex());
            if (IsFirstVisitToDestinationNode)
            {
                nodesController.HandleDestinationNode();
                nodesController.CalculateNodesMovementTime();
                StartCoroutine(UpdateEdgesAnchorsRoutine(nodesController.nodesMovementDelay, nodesController.nodesMovementDuration));
            }
            HandleDestinationEdge();
        }
        IEnumerator UpdateEdgesAnchorsRoutine(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            //UpdateEdgesAnchors(usePreviousNormalSign: false);

            float t = 0;
            while (t < duration)
            {
                UpdateEdgesAnchors(usePreviousNormalSign: true);

                t += Time.deltaTime;
                yield return null;

            }
            //UpdateVisitedNodesPositions();
            UpdateEdgesAnchors(usePreviousNormalSign: true);
        }
        private void AppearGradually()
        {
            float delay = nodesController.CalculateDelayBetweenNodeAppearances();
            int indexCount = 0;

            List<Node> visitedNodes = nodesController.GetVisitedNodes();
            for (int i = 0; i < visitedNodes.Count; i++)
            {
                var node = visitedNodes[i];

                foreach (var activeEdge in node.startPortActiveEdges)
                {
                    activeEdge.Appear(edgeAppearDuration, i * delay + edgeAppearDelay);
                } 
                indexCount++;
            }
        }
        public void Deactivate()
        {
            container.SetActive(false);
        }
        public void Disappear()
        {
            nodesController.Disappear();

            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                edge.Disappear();
            }
        }
        bool IsFirstVisitToEdge(int edgeIndex)
        {
            bool isFirstVisit = !visitedEdgesIndexes.Contains(edgeIndex);
            return isFirstVisit;
        }
        int GetDestinationEdgeIndex()
        {
            MatrixEdgeData destinationEdgeData = travelHistory[travelHistory.Count - 1];
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;

            var destinationIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == destinationEdgeData);
            return destinationIndex;
        }
        private void HandleDestinationEdge()
        {
            int edgeIndex = GetDestinationEdgeIndex();
            if (edgeIndex != -1)
            {
                if (IsFirstVisitToEdge(edgeIndex))
                {
                    visitedEdgesIndexes.Add(edgeIndex);

                    Edge destinationEdge = edges[edgeIndex];

                    int previousNodeIndex = nodesController.GetPreviousNodeIndex();
                    if(previousNodeIndex!=-1)
                    {
                        Node fromNode = nodesController.nodes[previousNodeIndex];
                        fromNode.AddToStartPortActiveEdges(destinationEdge);
                    }


                    ActivateNewEdgeVisitEffect(destinationEdge);
                }
            }
        }
        void ActivateNewEdgeVisitEffect(Edge edge)
        {
            edge.gameObject.SetActive(true);

            var matrixTraveler = MatrixTraveler.Instance;
            var edgeData = matrixTraveler.matrixGraphData.edges[edge.index];
            Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
            Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];

            UpdateEdgeAnchors(edge,startNode,endNode, usePreviousNormalSign: false);

            edge.Disappear();
            edge.Appear(firstVisitEdgeAppearDuration, firstVisitEdgeAppearDelay);
            Debug.Log("ToDo: New edge was added " + edge.name + ". Should active new edge visit effect");

        }
        void SyncWithTravelHistory()
        {
            travelHistory = MatrixTraveler.Instance.travelData.GetHistory();

            nodesController.ClearVisitedNodes();
            visitedEdgesIndexes.Clear();

            foreach (var travelEdgeData in travelHistory)
            {
                SyncWithTravelHistoryEntry(travelEdgeData);
            }
        }
        void SyncWithTravelHistoryEntry(MatrixEdgeData travelEdgeData)
        {
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;

            nodesController.AddVisitedNode(travelEdgeData.endPort.nodeIndex);

            var edgeIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == travelEdgeData);

            if (edgeIndex != -1)
            {
                visitedEdgesIndexes.Add(edgeIndex);
            }
        }
        void CreateEdges(MatrixTraveler matrixTraveler)
        {
            var edgesData = matrixTraveler.matrixGraphData.edges;

            for (int i = 0; i < edgesData.Count; i++)
            {
                Edge edge = CreateEdge(edgesData[i]);
                edge.index = i;
                edge.name = "Edge: Node " + edgesData[i].startPort.nodeIndex + " To " + edgesData[i].endPort.nodeIndex+
                            ", Port " + edgesData[i].startPort.id + " To " + edgesData[i].endPort.id;
            }
        }
        Edge CreateEdge(Node startNode, Node endNode)
        {
            var edge = Instantiate(edgePrefab, startNode.transform);

            edge.transform.localPosition = Vector3.zero;

            foreach (var childTransform in edge.GetComponentsInChildren<Transform>())
            {
                childTransform.gameObject.layer = transform.gameObject.layer;

            }

            startNode.AddToStartPortEdges(edge);
            endNode.AddToEndPortEdges(edge);

            EdgesNormalSign.Add(1);
            edges.Add(edge);
            CalculateEdgeAnchors(edges.Count - 1, startNode.transform.localPosition, endNode.transform.localPosition, mapCenter: Vector3.zero, out Vector3 p1, out Vector3 p2, out Vector3 p3, usePreviousNormalSign: false);
            edge.Init(p1, p2, p3);

            edge.SetModelColors(startNode.ColorHdr1, startNode.ColorHdr2);

            return edge;
        }
        Edge CreateEdge(MatrixEdgeData matrixEdgeData)
        {
            Node startNode = nodesController.nodes[matrixEdgeData.startPort.nodeIndex];
            
            Node endNode = nodesController.nodes[matrixEdgeData.endPort.nodeIndex];

            Edge edge = CreateEdge(startNode, endNode);

            return edge;
        }
        void UpdateEdgesAnchors(bool usePreviousNormalSign = false)
        {
            var matrixTraveler = MatrixTraveler.Instance;
            var edgesData = matrixTraveler.matrixGraphData.edges;

            
            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                var edgeData = edgesData[index];
                Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
                Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];

                UpdateEdgeAnchors(edge, startNode, endNode, usePreviousNormalSign);
            }
        }
        void UpdateEdgeAnchors(Edge edge, Node startNode, Node endNode, bool usePreviousNormalSign = false)
        {
            CalculateEdgeAnchors(edge.index, startNode.transform.localPosition, endNode.transform.localPosition, mapCenter: Vector3.zero, out Vector3 p1, out Vector3 p2, out Vector3 p3, usePreviousNormalSign);
            edge.UpdateBezierCurve(p1, p2, p3);
            edge.UpdateMesh();
        }
        void CalculateEdgeAnchors(int edgeIndex, Vector3 startNodePosition, Vector3 endNodePosition, Vector3 mapCenter, out Vector3 anchorPoint1, out Vector3 anchorPoint2, out Vector3 anchorPoint3, bool usePreviousNormalSign)
        {
            Edge edge = edges[edgeIndex];
            edge.transform.localPosition = Vector3.zero;

            Vector3 middlePoint = (endNodePosition - startNodePosition) / 2f;
            Vector3 edgeDirection = middlePoint.normalized;
            Vector3 normal = Vector3.Cross(edgeDirection, Vector3.back);

            bool isNormalTowardsCenter = Vector3.Dot(normal, mapCenter - endNodePosition) > -0.01 ? true : false;
            
            if (usePreviousNormalSign)
            {
                normal *= EdgesNormalSign[edgeIndex];
            }
            else if (isNormalTowardsCenter == false)
            {
                normal = -normal;
                EdgesNormalSign[edgeIndex] = -1;
            }
            else
                EdgesNormalSign[edgeIndex] = 1;

            Vector3 point2;
            float extraOffsetPoint2 = (1 + edgeIndex* sameNodeEdgesOffset); // takes care of the initial offset and maybe normal too
            point2 = middlePoint + normal * ((startNodePosition + middlePoint).magnitude + extraOffsetPoint2);

            Vector3 point3 = endNodePosition - startNodePosition;
            Vector3 point1 = Vector3.zero;
            point1.z += 0.01f;
            point3.z += -0.01f;

            anchorPoint1 = point1;
            anchorPoint2 = point2;
            anchorPoint3 = point3;
        }
    }
}
