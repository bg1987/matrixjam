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
        [SerializeField, ColorUsage(true,true)] Color activeEdgeEndColor = Color.yellow;

        [SerializeField] float sameNodeEdgesOffset = 0.2f;
        private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative

        [Header("Edge First Visit")]
        [SerializeField, Min(0)] float firstVisitEdgeAppearDelay = 1f;
        [SerializeField, Min(0)] float firstVisitEdgeAppearDuration = 0.8f;
        [SerializeField, ColorUsage(true,true)] Color firstVisitEdgeColor = Color.yellow;
        [SerializeField] EdgeVisitEffect edgeVisitEffect;
        [Header("EdgesUIs")]
        [SerializeField] EdgesUIs edgeUis;
        [SerializeField] private int[] endNormalSigns;
        [SerializeField] private int[] startNormalSigns;
        [Header("Edge History Sequence Appearance")]
        [SerializeField] private float delayBetweenEdgesHistoryEntriesSequenceAppear = 0.5f;
        [SerializeField] private float edgeSequenceAppearDuration = 0.5f;
        [SerializeField] private float sequenceAppearDelay = 1.5f;

        // Start is called before the first frame update
        void Start()
        {
            
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
                edge.MeshCollider.enabled = false;
            }
            edgeUis.Init(edges);

            endNormalSigns = new int[edges.Count];
            startNormalSigns = new int[edges.Count];
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
        public void Appear()
        {
            foreach (var index in visitedEdgesIndexes)
            {
                var edge = edges[index];
                edge.gameObject.SetActive(true);
            }
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
        public void Disappear()
        {
            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                edge.Disappear();
            }
            edgeVisitEffect.RevisitEffect.Stop();
        }
        internal void ClearvisitedEdges()
        {
            visitedEdgesIndexes.Clear();
        }
        public void AddVisitedEdge(int index, Node node)
        {
            visitedEdgesIndexes.Add(index);
            node.AddToStartPortActiveEdges(edges[index]);
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
        public float CalculateEdgeRevisitTime()
        {
            return firstVisitEdgeAppearDelay + edgeVisitEffect.RevisitEffect.CalculateEffectDuration();
        }
        //Destination Edge related
        public void HandleDestinationEdge(Nodes nodesController)
        {
            int edgeIndex = GetDestinationEdgeIndex();
            if (edgeIndex != -1)
            {
                Edge destinationEdge = edges[edgeIndex];

                if (IsFirstVisitToEdge(edgeIndex))
                {
                    
                    FirstEdgeVisit(edgeIndex, nodesController);
                }
                else
                {
                    edgeVisitEffect.RevisitEffect.Play(destinationEdge, firstVisitEdgeAppearDelay);
                }
            }
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

            var destinationIndex = FindEdgeIndexByEdgeData(destinationEdgeData);
            return destinationIndex;
        }
        public int FindEdgeIndexByEdgeData(MatrixEdgeData targetEdgeData)
        {
            var edgesData = MatrixTraveler.Instance.matrixGraphData.edges;
            var destinationIndex = edgesData.FindIndex((MatrixEdgeData edgeData) => edgeData == targetEdgeData);
            return destinationIndex;
        }
        void FirstEdgeVisit(int edgeIndex, Nodes nodesController)
        {
            Edge edge = edges[edgeIndex];

            visitedEdgesIndexes.Add(edgeIndex);

            int previousNodeIndex = nodesController.GetPreviousNodeIndex();
            if (previousNodeIndex != -1)
            {
                Node fromNode = nodesController.nodes[previousNodeIndex];
                fromNode.AddToStartPortActiveEdges(edge);
            }
            edge.gameObject.SetActive(true);

            var matrixTraveler = MatrixTraveler.Instance;
            var edgeData = matrixTraveler.matrixGraphData.edges[edge.index];
            Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
            Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];

            UpdateEdgeAnchors(edge, startNode, endNode, usePreviousNormalSign: false);

            edge.Disappear();
            edge.Appear(firstVisitEdgeAppearDuration, firstVisitEdgeAppearDelay);

            edge.SetTintColor(firstVisitEdgeColor);

            edgeVisitEffect.FirstVisitEffect.Play(edge, firstVisitEdgeAppearDelay);

        }
        public void AppearByHistorySequence(List<MatrixEdgeData> edgesSequence)
        {
            StartCoroutine(AppearByHistorySequenceRoutine(edgesSequence));
        }
        IEnumerator AppearByHistorySequenceRoutine(List<MatrixEdgeData> edgesSequence)
        {
            yield return new WaitForSeconds(sequenceAppearDelay);
            SortedSet<int> alreadyAppearedEdgesIndexes = new SortedSet<int>();

            List<int> edgesIdsSequence = new List<int>();
            foreach (var edgeData in edgesSequence)
            {
                edgesIdsSequence.Add(FindEdgeIndexByEdgeData(edgeData));
            }



            for (int i = 0; i < edgesIdsSequence.Count-1; i++)
            {
                if (edgesIdsSequence[i] == -1)
                {
                    yield return new WaitForSeconds(delayBetweenEdgesHistoryEntriesSequenceAppear);
                    continue;
                }
                var edge = edges[edgesIdsSequence[i]];
                if (alreadyAppearedEdgesIndexes.Contains(edge.index))
                {
                    continue;
                }
                edge.gameObject.SetActive(true);

                edge.Appear(edgeSequenceAppearDuration, 0);
                alreadyAppearedEdgesIndexes.Add(edge.index);

                yield return new WaitForSeconds(delayBetweenEdgesHistoryEntriesSequenceAppear);
            }
            //Might make use of the following v

            //float delay = CalculateDelayBetweenNodeAppearances();
            //previousActiveNodeMarker Appearance
            //if (visitedNodesIndexesSorted.Count > 1)
            //{
            //    previousActiveNodeMarkerAppearance(indexCount * delay);
            //}
        }

        //Edge Creation
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
        //Update Edge Position
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
        void CalculateEdgeAnchors(int edgeIndex, Vector3 startNodePosition, Vector3 endNodePosition, Vector3 mapCenter, out Vector3 anchorPoint1, out Vector3 anchorPoint2, out Vector3 anchorPoint3, int normalSign, float distanceScaleP2)
        {
            Edge edge = edges[edgeIndex];
            edge.transform.localPosition = Vector3.zero;

            Vector3 middlePoint = (endNodePosition - startNodePosition) / 2f;
            Vector3 edgeDirection = middlePoint.normalized;
            Vector3 normal = Vector3.Cross(edgeDirection, Vector3.back);

            bool isNormalTowardsCenter = Vector3.Dot(normal, mapCenter - endNodePosition) > -0.01 ? true : false;

            if (isNormalTowardsCenter == false)
            {
                EdgesNormalSign[edgeIndex] = -1;
            }
            else
                EdgesNormalSign[edgeIndex] = 1;

            normal *= normalSign;

            Vector3 point2;
            float extraOffsetPoint2 = (1 + edgeIndex * sameNodeEdgesOffset); // takes care of the initial offset and maybe normal too
            point2 = middlePoint + (normal * ((startNodePosition + middlePoint).magnitude + extraOffsetPoint2)*distanceScaleP2);

            Vector3 point3 = endNodePosition - startNodePosition;
            Vector3 point1 = Vector3.zero;
            point1.z += 0.01f;
            point3.z += -0.01f;

            anchorPoint1 = point1;
            anchorPoint2 = point2;
            anchorPoint3 = point3;
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
        public void UpdateEdgesAnchors(Nodes nodesController, float normalT)
        {
            var matrixTraveler = MatrixTraveler.Instance;
            var edgesData = matrixTraveler.matrixGraphData.edges;

            foreach (var index in visitedEdgesIndexes)
            {

                Edge edge = edges[index];
                var edgeData = edgesData[index];
                Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
                Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];
                int previousNormalSign = startNormalSigns[index];
                int endNormalSign = endNormalSigns[index];

                int normalSign = normalT > 0.5 ? endNormalSign : previousNormalSign;
                float distanceScale = Mathf.Abs(Mathf.SmoothStep(previousNormalSign, endNormalSign, normalT));

                UpdateEdgeAnchors(edge, startNode, endNode, normalSign, distanceScale);
            }
        }
        public IEnumerator UpdateEdgesAnchorsRoutine(float delay, float duration, Nodes nodesController)
        {
            yield return new WaitForSeconds(delay);
            //UpdateEdgesAnchors(usePreviousNormalSign: false);

            CalculateNodesStartAndEndNormalSigns(nodesController);
            float t = 0;
            while (t < 1)
            {
                //UpdateEdgesAnchors(usePreviousNormalSign: true, nodesController);
                UpdateEdgesAnchors(nodesController, t);

                t += Time.deltaTime/duration;
                yield return null;

            }
            UpdateEdgesAnchors(nodesController, 1);
            //UpdateEdgesAnchors(usePreviousNormalSign: true, nodesController);
        }

        private void CalculateNodesStartAndEndNormalSigns(Nodes nodesController)
        {
            var matrixTraveler = MatrixTraveler.Instance;
            var edgesData = matrixTraveler.matrixGraphData.edges;

            var visitedNodes = nodesController.GetVisitedNodes();

            foreach (var index in visitedEdgesIndexes)
            {
                Edge edge = edges[index];
                var edgeData = edgesData[index];
                Node startNode = nodesController.nodes[edgeData.startPort.nodeIndex];
                Node endNode = nodesController.nodes[edgeData.endPort.nodeIndex];
                
                int startNodePositionIndex = visitedNodes.FindIndex(node => node.Index == startNode.Index);
                Vector3 startNodeEndPosition = nodesController.NodesPositions[startNodePositionIndex];
                
                int endNodePositionIndex = visitedNodes.FindIndex(node => node.Index == endNode.Index);
                Vector3 endNodeEndPosition = nodesController.NodesPositions[endNodePositionIndex];

                int endNormalSign = CalculateEdgeAnchorNormal(startNodeEndPosition, endNodeEndPosition, mapCenter: Vector3.zero);
                endNormalSigns[index]= endNormalSign;

                Vector3 startNodeStartPosition = startNode.transform.position;
                Vector3 endNodeStartPosition = endNode.transform.position;

                int startNormalSign = CalculateEdgeAnchorNormal(startNodeStartPosition, endNodeStartPosition, mapCenter: Vector3.zero);
                startNormalSigns[index] = startNormalSign;
            }
        }
        int CalculateEdgeAnchorNormal(Vector3 startPosition, Vector3 endPosition, Vector3 mapCenter)
        {
            Vector3 middlePoint = (endPosition - startPosition) / 2f;
            Vector3 edgeDirection = middlePoint.normalized;
            Vector3 normal = Vector3.Cross(edgeDirection, Vector3.back);

            bool isNormalTowardsCenter = Vector3.Dot(normal, mapCenter - endPosition) > -0.01 ? true : false;

            int normalSign;
            if (isNormalTowardsCenter == false)
            {
                normalSign = -1;
            }
            else
                normalSign = 1;

            return normalSign;
        }
        void UpdateEdgeAnchors(Edge edge, Node startNode, Node endNode, bool usePreviousNormalSign = false)
        {
            CalculateEdgeAnchors(edge.index, startNode.transform.localPosition, endNode.transform.localPosition, mapCenter: Vector3.zero, out Vector3 p1, out Vector3 p2, out Vector3 p3, usePreviousNormalSign);
            edge.UpdateBezierCurve(p1, p2, p3);
            edge.UpdateMesh();
        }
        void UpdateEdgeAnchors(Edge edge, Node startNode, Node endNode, int normalSign, float distanceScaleP2)
        {
            CalculateEdgeAnchors(edge.index, startNode.transform.localPosition, endNode.transform.localPosition,mapCenter: Vector3.zero,
                                 out Vector3 p1, out Vector3 p2, out Vector3 p3, normalSign, distanceScaleP2);
            edge.UpdateBezierCurve(p1, p2, p3);
            edge.UpdateMesh();
        }
    }
}
