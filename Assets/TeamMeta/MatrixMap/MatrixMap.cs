using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class MatrixMap : MonoBehaviour
    {
        [Header("Nodes")]
        [SerializeField] Node nodePrefab;

        [SerializeField] int nodesCount;

        [SerializeField] float nodesSize = 1;
        [SerializeField] float radius = 1;

        List<Node> nodes = new List<Node>();
        List<Node> visitedNodes = new List<Node>();
        List<Vector3> nodesPositions = new List<Vector3>();

        [Header("Edges")]
        [SerializeField] Edge edgePrefab;
        [SerializeField] List<Edge> edges = new List<Edge>();
        [SerializeField] float sameNodeEdgesOffset = 0.2f;
        [SerializeField] private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative

        const float TAU = Mathf.PI * 2;

        // Start is called before the first frame update
        void Start()
        {
            InitMap();
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        void InitMap()
        {
            if (MatrixTraveler.Instance)
            {
                CreateNodes(MatrixTraveler.Instance);
            }
            else
                CreateNodes();

            CalculateNodesPositions();
            UpdateNodesPositions();

            if (MatrixTraveler.Instance)
            {
                CreateEdges(MatrixTraveler.Instance);
            }
            //UpdateNodesScale();
            //UpadteNodesRadius();

            //DebugVisualizeFirstLastNodes();
        }
        private List<Vector3> CalculateNodesPositions()
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
            var node = Instantiate(nodePrefab, transform);
            node.name = "Node";
            node.transform.localScale = Vector3.one * nodesSize;

            return node;
        }
        void CreateEdges(MatrixTraveler matrixTraveler)
        {
            var edgesData = matrixTraveler.matrixGraphData.edges;

            for (int i = 0; i < edgesData.Count; i++)
            {
                Edge edge = CreateEdge(edgesData[i]);
                edge.name = "Edge " + edgesData[i].startPort.nodeIndex + " To " + edgesData[i].endPort.nodeIndex;
            }
        }
        Edge CreateEdge(Node startNode, Node endNode)
        {
            var edge = Instantiate(edgePrefab, startNode.transform);

            edge.transform.localPosition = Vector3.zero;

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

        public void CalculateEdgeAnchors(int edgeIndex, Vector3 startNodePosition, Vector3 endNodePosition, Vector3 mapCenter, out Vector3 anchorPoint1, out Vector3 anchorPoint2, out Vector3 anchorPoint3, bool usePreviousNormalSign)
        {
            Edge edge = edges[edgeIndex];
            edge.transform.localPosition = Vector3.zero;

            Vector3 middlePoint = (endNodePosition - startNodePosition) / 2f;
            Vector3 edgeDirection = middlePoint.normalized;
            Vector3 normal = Vector3.Cross(edgeDirection, Vector3.back);

            Vector3 centerOfMap = Vector3.zero;
            bool isNormalTowardsCenter = Vector3.Dot(normal, centerOfMap - endNodePosition) > -0.01 ? true : false;
            
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
