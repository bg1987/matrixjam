using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class MatrixMap : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [Header("Nodes")]
        [SerializeField] Node nodePrefab;

        [SerializeField] int nodesCount;

        [SerializeField] float nodesSize = 1;
        [SerializeField] float radius = 1;

        List<Node> nodes = new List<Node>();
        SortedSet<int> visitedNodesIndexesSorted = new SortedSet<int>();
        List<Vector3> nodesPositions = new List<Vector3>();

        [Header("Nodes Appear")]
        [SerializeField, Min(0)] float nodeAppearDuration = 0.2f;
        [SerializeField, Min(0)] float delayBetweenNodeAppearances = 0;
        [SerializeField, Min(0)] float totalTimeForAllNodeAppearances = 1;

        [Header("Edges")]
        [SerializeField] Edge edgePrefab;
        [SerializeField] List<Edge> edges = new List<Edge>();
        HashSet<int> visitedEdgesIndexes = new HashSet<int>();
        
        [SerializeField] float sameNodeEdgesOffset = 0.2f;
        [SerializeField] private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative
        
        private IReadOnlyList<MatrixEdgeData> travelHistory; //1 positive, -1 negative

        const float TAU = Mathf.PI * 2;

        // Start is called before the first frame update
        void Start()
        {
            InitMap();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Appear();
                //HandleNewTravelHistoryEntry();
            }
        }
        void InitMap()
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

            if (MatrixTraveler.Instance)
            {
                CreateEdges(MatrixTraveler.Instance);

                SyncWithTravelHistory();
            }
            //UpdateNodesScale();
            //UpadteNodesRadius();

            //DebugVisualizeFirstLastNodes();
            foreach (var node in nodes)
            {
                node.gameObject.SetActive(false);
            }
            foreach (var edge in edges)
            {
                edge.gameObject.SetActive(false);
            }
            container.SetActive(false);

        }

        internal float CalculateTotalAppearanceTime()
        {
            float TotalAppearanceTime = 0;

            float delay = totalTimeForAllNodeAppearances / visitedNodesIndexesSorted.Count;
            delay += delayBetweenNodeAppearances;

            TotalAppearanceTime += (visitedNodesIndexesSorted.Count - 1) * delay;
            TotalAppearanceTime += (visitedNodesIndexesSorted.Count - 1) * delayBetweenNodeAppearances;
            TotalAppearanceTime += nodeAppearDuration;
            return TotalAppearanceTime;
        }

        public void Appear()
        {
            container.SetActive(true);
            HandleNewTravelHistoryEntry();

            CalculateNodesPositions(visitedNodesIndexesSorted.Count);
            
            UpdateVisitedNodesPositions();
            UpdateEdgesAnchors();
            foreach (var index in visitedNodesIndexesSorted)
            {
                var node = nodes[index];
                node.gameObject.SetActive(true);
            }
            foreach (var index in visitedEdgesIndexes)
            {
                var edge = edges[index];
                edge.gameObject.SetActive(true);
            }
            Disappear();
            AppearGradually();
        }
        private void AppearGradually()
        {
            float delay = totalTimeForAllNodeAppearances / visitedNodesIndexesSorted.Count;
            delay += delayBetweenNodeAppearances;
            int indexCount = 0;
            foreach (var index in visitedNodesIndexesSorted)
            {
                var node = nodes[index];

                node.Appear(nodeAppearDuration, indexCount * delay);
                //nodes[i].EdgesAppear(edgeAppearDuration, i * delay + edgeAppearDelay);
                indexCount++;
            }
        }
        public void Deactivate()
        {
            container.SetActive(false);
        }
        public void Disappear()
        {
            foreach (var index in visitedNodesIndexesSorted)
            {
                Node node = nodes[index];
                node.Disappear();
                //node.EdgesDisappear(0, 0);
            }
        }
        public void HandleNewTravelHistoryEntry()
        {
            MatrixEdgeData travelEdgeData = travelHistory[travelHistory.Count - 1];
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;

            int lastVisitedNodeIndex = travelEdgeData.endPort.nodeIndex;
            if (visitedNodesIndexesSorted.Contains(lastVisitedNodeIndex) == false)
            {
                visitedNodesIndexesSorted.Add(lastVisitedNodeIndex);

                ActivateNewNodeVisitEffect(nodes[lastVisitedNodeIndex]);
            }

            var edgeIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == travelEdgeData);
            if (visitedEdgesIndexes.Contains(edgeIndex) == false)
            {
                if (edgeIndex != -1)
                {
                    if (visitedEdgesIndexes.Contains(edgeIndex) == false)
                    {
                        visitedEdgesIndexes.Add(edgeIndex);

                        ActivateNewEdgeVisitEffect(edges[edgeIndex]);
                    }
                }

            }
        }
        void ActivateNewNodeVisitEffect(Node node)
        {
            Debug.Log("New node was added " + node.name+ ". Should active new node visit effect");
        }
        void ActivateNewEdgeVisitEffect(Edge edge)
        {
            Debug.Log("New edge was added " + edge.name + ". Should active new edge visit effect");

        }
        void SyncWithTravelHistory()
        {
            travelHistory = MatrixTraveler.Instance.travelData.GetHistory();

            visitedNodesIndexesSorted.Clear();
            visitedEdgesIndexes.Clear();

            foreach (var travelEdgeData in travelHistory)
            {
                SyncWithTravelHistoryEntry(travelEdgeData);
            }
        }
        void SyncWithTravelHistoryEntry(MatrixEdgeData travelEdgeData)
        {
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;

            visitedNodesIndexesSorted.Add(travelEdgeData.endPort.nodeIndex);

            var edgeIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == travelEdgeData);

            if (edgeIndex != -1)
            {
                visitedEdgesIndexes.Add(edgeIndex);
            }
        }
        private List<Vector3> CalculateNodesPositions(int nodesCount)
        {
            nodesPositions.Clear();

            float rotateOffset = -Mathf.PI / 2f; //90 degrees. For visual symmetry

            for (int i = 0; i < nodesCount; i++)
            {
                float t = i / (float)nodesCount;
                float rotateBy = t * TAU + rotateOffset;
                nodesPositions.Add(new Vector3(Mathf.Cos(rotateBy), Mathf.Sin(rotateBy), transform.position.z) * radius);
            }
            return nodesPositions;
        }
        void CreateNodes(MatrixTraveler matrixTraveler)
        {
            var nodesData = matrixTraveler.matrixGraphData.nodes;
            nodes.Capacity = nodesData.Count;
            foreach (var nodeData in nodesData)
            {
                int i = nodeData.index;

                var node = CreateNode();
                node.name += i;
                nodes.Insert(i,node);
            }
            nodesCount = nodesData.Count;
        }
        void UpdateNodesPositions()
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
        void CreateNodes()
        {
            for (int i = 0; i < nodesCount; i++)
            {
                var node = CreateNode();
                nodes.Add(node);
            }

        }
        Node CreateNode()
        {
            var node = Instantiate(nodePrefab, container.transform);
            node.name = "Node";
            node.transform.localScale = Vector3.one * nodesSize;
            foreach (var childTransform in node.GetComponentsInChildren<Transform>())
            {
                childTransform.gameObject.layer = transform.gameObject.layer;

            }
            return node;
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
            return edge;
        }
        Edge CreateEdge(MatrixEdgeData matrixEdgeData)
        {
            Node startNode = nodes[matrixEdgeData.startPort.nodeIndex];
            Node endNode = nodes[matrixEdgeData.endPort.nodeIndex];

            Edge edge = CreateEdge(startNode, endNode);
            return edge;
        }
        void UpdateEdgesAnchors()
        {
            var matrixTraveler = MatrixTraveler.Instance;
            var edgesData = matrixTraveler.matrixGraphData.edges;

            
            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                var edgeData = edgesData[index];
                Node startNode = nodes[edgeData.startPort.nodeIndex];
                Node endNode = nodes[edgeData.endPort.nodeIndex];

                CalculateEdgeAnchors(index, startNode.transform.localPosition, endNode.transform.localPosition, mapCenter: Vector3.zero, out Vector3 p1, out Vector3 p2, out Vector3 p3, usePreviousNormalSign: false);
                edge.UpdateBezierCurve(p1, p2, p3);
                edge.UpdateMesh();
            }
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
