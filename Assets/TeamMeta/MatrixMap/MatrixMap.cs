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
        [SerializeField] Edges edgesController;

        private IReadOnlyList<MatrixEdgeData> travelHistory; //1 positive, -1 negative
        const float TAU = Mathf.PI * 2;
        [Header("Selector")]
        [SerializeField] Selector selector;
        public Selector Selector { get => selector; }
        [Header("Interactable")]
        [SerializeField] float nodesInteractableFlashDuration = 0.8f;
        public bool interactable{set { SetInteractable(value); } }
        private void Awake()
        {
            container.SetActive(true);
        }
        // Start is called before the first frame update
        IEnumerator Start()
        {
            InitMap();
            yield return null;

            if (MatrixTraveler.Instance)
            {
                SyncWithTravelHistory();
            }
        }
        // Update is called once per frame
        void SetInteractable(bool isInteractable)
        {
            if (isInteractable)
                StartCoroutine(InteractableRoutine());
            else
                selector.Deactivate();

        }
        IEnumerator InteractableRoutine()
        {
            float selectedColorMark = 38 / 100f;
            float hoverColorMark = 24 / 100f;
            float idleColorMark = 38 / 100f;
            nodesController.FlashNodesInteractableEffect(nodesInteractableFlashDuration, selectedColorMark, hoverColorMark, idleColorMark);

            yield return new WaitForSeconds(nodesInteractableFlashDuration * (selectedColorMark+hoverColorMark));
            selector.Activate();
        }
        void InitMap()
        {
            nodesController.Init();
            edgesController.Init(nodesController);
            //UpdateNodesScale();
            //UpadteNodesRadius();

            //DebugVisualizeFirstLastNodes();
            
            
            container.SetActive(false);

        }
        internal float CalculateTotalAppearanceTime()
        {
            float totalAppearanceTime = 0;

            float totalNodesAppearanceTime = nodesController.CalculateTotalNodesAppearanceTime();
            
            float totalEdgesAppearanceTime = 0;
            //totalEdgesAppearanceTime += (visitedNodesIndexesSorted.Count - 1) * delayBetweenNodeAppearances; // Test this
            totalEdgesAppearanceTime += totalNodesAppearanceTime;
            totalEdgesAppearanceTime += edgesController.CalculateEdgeAppearTime();

            float nodeVisitTime = 0;
            bool isFirstVisitToNode = nodesController.IsFirstVisitToNode(nodesController.GetDestinationNodeIndex());
            bool isFirstVisitToEdge = edgesController.IsFirstVisitToEdge(edgesController.GetDestinationEdgeIndex());
            if (isFirstVisitToNode)
            {
                float nodeMovementTime = nodesController.CalculateNodesMovementTime();
                float nodeFirstVisitTime = nodesController.CalculateNodeFirstVisitTime();

                nodeVisitTime = Mathf.Max(nodeMovementTime, nodeFirstVisitTime);
            }
            float edgeVisitTime = 0;
            if (isFirstVisitToEdge)
            {
                float edgeFirstVisitTime = edgesController.CalculateEdgeFirstVisitTime();

                edgeVisitTime = edgeFirstVisitTime;
            }
            else
            {
                float edgeRevisitTime = edgesController.CalculateEdgeRevisitTime();
                edgeVisitTime = edgeRevisitTime;
            }

            totalAppearanceTime = Mathf.Max( totalNodesAppearanceTime, totalEdgesAppearanceTime, nodeVisitTime, edgeVisitTime);
            return totalAppearanceTime;
        }

        public void Appear()
        {
            container.SetActive(true);

            nodesController.Appear();
            edgesController.Appear();
            
            Disappear();

            edgesController.UpdateEdgesAnchors(false, nodesController);

            nodesController.AppearGradually();
            edgesController.AppearGradually(nodesController);

            bool IsFirstVisitToDestinationNode = nodesController.IsFirstVisitToNode(nodesController.GetDestinationNodeIndex());
            if (IsFirstVisitToDestinationNode)
            {
                nodesController.HandleDestinationNode();
                nodesController.CalculateNodesMovementTime();
                StartCoroutine(edgesController.UpdateEdgesAnchorsRoutine(nodesController.nodesMovementDelay, nodesController.nodesMovementDuration,nodesController));
            }
            edgesController.HandleDestinationEdge(nodesController);
        }
        public void CreditsAppear()
        {
            StartCoroutine(CreditsAppearRoutine());
        }
        IEnumerator CreditsAppearRoutine()
        {
            interactable = false;
            container.SetActive(true);
            //Disappear();
            
            var nodesIdsSequence = new List<int>();
            var edgesSequence = new List<MatrixEdgeData>();

            foreach (var edge in travelHistory)
            {
                nodesIdsSequence.Add(edge.endPort.nodeIndex);
                edgesSequence.Add(edge);
            }
            edgesSequence.RemoveAt(0); //First edge is a blank

            nodesController.Appear();
            nodesController.Disappear();

            nodesController.AppearCredits(nodesIdsSequence);

            edgesController.Appear();
            edgesController.Disappear();
            edgesController.UpdateEdgesAnchors(false, nodesController);

            edgesController.AppearCredits(edgesSequence);

            yield return new WaitForSeconds(8f);

            edgesController.HandleDestinationEdge(nodesController);

            yield return null;
        }
        public void Deactivate()
        {
            container.SetActive(false);
        }
        public void Disappear()
        {
            nodesController.Disappear();
            edgesController.Disappear();
        }
        void SyncWithTravelHistory()
        {
            travelHistory = MatrixTraveler.Instance.travelData.GetHistory();

            nodesController.ClearVisitedNodes();
            edgesController.ClearvisitedEdges();

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
                var node = nodesController.nodes[travelEdgeData.startPort.nodeIndex];
                edgesController.AddVisitedEdge(edgeIndex, node);
            }
        }
    }
}
