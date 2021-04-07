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

        // Start is called before the first frame update
        void Start()
        {
            InitMap();
        }
        // Update is called once per frame
        
        void InitMap()
        {
            nodesController.Init();
            edgesController.Init(nodesController);
            if (MatrixTraveler.Instance)
            {
                SyncWithTravelHistory();
            }
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

            float nodeAdditionTime = 0;
            bool isFirstVisitToNode = nodesController.IsFirstVisitToNode(nodesController.GetDestinationNodeIndex());
            bool isFirstVisitToEdge = edgesController.IsFirstVisitToEdge(edgesController.GetDestinationEdgeIndex());
            if (isFirstVisitToNode)
            {
                float nodeMovementTime = nodesController.CalculateNodesMovementTime();
                float addedNodeTime = nodesController.CalculateAddedNodeTime();
                float addedEdgeTime = edgesController.CalculateEdgeFirstVisitTime();

                nodeAdditionTime = Mathf.Max(nodeMovementTime, addedNodeTime, addedEdgeTime);
            }
            else if (isFirstVisitToEdge)
            {
                float addedEdgeTime = edgesController.CalculateEdgeFirstVisitTime();

                nodeAdditionTime = addedEdgeTime;
            }

            totalAppearanceTime = Mathf.Max( totalNodesAppearanceTime, totalEdgesAppearanceTime, nodeAdditionTime);
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
                edgesController.AddVisitedEdge(edgeIndex);
            }
        }
    }
}
