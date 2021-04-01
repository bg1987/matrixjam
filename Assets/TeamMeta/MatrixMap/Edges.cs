using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Edges : MonoBehaviour
    {
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

        // Start is called before the first frame update
        void Start()
        {
            
        }
        public float CalculateEdgeAppearTime()
        {
            float EdgeAppearanceTime = 0;
            EdgeAppearanceTime += edgeAppearDelay;
            EdgeAppearanceTime += edgeAppearDuration;

            return EdgeAppearanceTime;
        }
        public float CalculateEdgeFirstVisitTime()
        {
            return firstVisitEdgeAppearDelay + firstVisitEdgeAppearDuration;
        }
        public void Init(Nodes nodesController)
        {
            if (MatrixTraveler.Instance)
            {
                CreateEdges(MatrixTraveler.Instance, nodesController);
            }
            foreach (var edge in edges)
            {
                edge.gameObject.SetActive(false);
            }
        }
        void CreateEdges(MatrixTraveler matrixTraveler, Nodes nodesController)
        {
            var edgesData = matrixTraveler.matrixGraphData.edges;

            for (int i = 0; i < edgesData.Count; i++)
            {
                Edge edge = CreateEdge(edgesData[i], nodesController);
                edge.index = i;
                edge.name = "Edge: Node " + edgesData[i].startPort.nodeIndex + " To " + edgesData[i].endPort.nodeIndex +
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
        Edge CreateEdge(MatrixEdgeData matrixEdgeData, Nodes nodesController)
        {
            Node startNode = nodesController.nodes[matrixEdgeData.startPort.nodeIndex];

            Node endNode = nodesController.nodes[matrixEdgeData.endPort.nodeIndex];

            Edge edge = CreateEdge(startNode, endNode);

            return edge;
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
            float extraOffsetPoint2 = (1 + edgeIndex * sameNodeEdgesOffset); // takes care of the initial offset and maybe normal too
            point2 = middlePoint + normal * ((startNodePosition + middlePoint).magnitude + extraOffsetPoint2);

            Vector3 point3 = endNodePosition - startNodePosition;
            Vector3 point1 = Vector3.zero;
            point1.z += 0.01f;
            point3.z += -0.01f;

            anchorPoint1 = point1;
            anchorPoint2 = point2;
            anchorPoint3 = point3;
        }

        internal void UpdateEdgesAnchors()
        {
            throw new NotImplementedException();
        }

        internal void AppearGradually(Nodes nodesController)
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

        public void Appear()
        {
            foreach (var index in visitedEdgesIndexes)
            {
                var edge = edges[index];
                edge.gameObject.SetActive(true);
            }
        }
        public void Disappear()
        {
            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                edge.Disappear();
            }
        }
        internal void ClearvisitedEdges()
        {
            visitedEdgesIndexes.Clear();
        }
        public List<Edge> GetVisitedEdges()
        {
            var visitedEdges = new List<Edge>();
            foreach (var index in visitedEdgesIndexes)
            {
                var edge = edges[index];
                visitedEdges.Add(edge);
            }
            return visitedEdges;
        }
        public void AddVisitedEdge(int index)
        {
            visitedEdgesIndexes.Add(index);
        }
        public bool IsFirstVisitToEdge(int edgeIndex)
        {
            bool isFirstVisit = !visitedEdgesIndexes.Contains(edgeIndex);
            return isFirstVisit;
        }
        public int GetDestinationEdgeIndex()
        {
            var travelHistory = MatrixTraveler.Instance.travelData.GetHistory();
            MatrixEdgeData destinationEdgeData = travelHistory[travelHistory.Count - 1];
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;

            var destinationIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == destinationEdgeData);
            return destinationIndex;
        }
        public void HandleDestinationEdge(Nodes nodesController)
        {
            int edgeIndex = GetDestinationEdgeIndex();
            if (edgeIndex != -1)
            {
                if (IsFirstVisitToEdge(edgeIndex))
                {
                    visitedEdgesIndexes.Add(edgeIndex);

                    Edge destinationEdge = edges[edgeIndex];

                    int previousNodeIndex = nodesController.GetPreviousNodeIndex();
                    if (previousNodeIndex != -1)
                    {
                        Node fromNode = nodesController.nodes[previousNodeIndex];
                        fromNode.AddToStartPortActiveEdges(destinationEdge);
                    }


                    ActivateNewEdgeVisitEffect(destinationEdge, nodesController);
                }
            }
        }
        void ActivateNewEdgeVisitEffect(Edge edge, Nodes nodesController)
        {
            edge.gameObject.SetActive(true);

            var matrixTraveler = MatrixTraveler.Instance;
            var edgeData = matrixTraveler.matrixGraphData.edges[edge.index];
            Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
            Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];

            UpdateEdgeAnchors(edge, startNode, endNode, usePreviousNormalSign: false);

            edge.Disappear();
            edge.Appear(firstVisitEdgeAppearDuration, firstVisitEdgeAppearDelay);
            Debug.Log("ToDo: New edge was added " + edge.name + ". Should active new edge visit effect");

        }
        public void UpdateEdgesAnchors(bool usePreviousNormalSign, Nodes nodesController)
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
        public IEnumerator UpdateEdgesAnchorsRoutine(float delay, float duration, Nodes nodesController)
        {
            yield return new WaitForSeconds(delay);
            //UpdateEdgesAnchors(usePreviousNormalSign: false);

            float t = 0;
            while (t < duration)
            {
                UpdateEdgesAnchors(usePreviousNormalSign: true, nodesController);

                t += Time.deltaTime;
                yield return null;

            }
            //UpdateVisitedNodesPositions();
            UpdateEdgesAnchors(usePreviousNormalSign: true, nodesController);
        }
        void UpdateEdgeAnchors(Edge edge, Node startNode, Node endNode, bool usePreviousNormalSign = false)
        {
            CalculateEdgeAnchors(edge.index, startNode.transform.localPosition, endNode.transform.localPosition, mapCenter: Vector3.zero, out Vector3 p1, out Vector3 p2, out Vector3 p3, usePreviousNormalSign);
            edge.UpdateBezierCurve(p1, p2, p3);
            edge.UpdateMesh();
        }
    }
}
