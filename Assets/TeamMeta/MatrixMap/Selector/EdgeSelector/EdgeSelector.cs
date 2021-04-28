using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeSelector : MonoBehaviour
    {
        EdgeSelectable hoveredEdge;

        [SerializeField] HoveredEdgeUI edgeUI;
        [SerializeField] MatrixTraveler matrixTraveler;

        // Start is called before the first frame update
        void Start()
        {
            DeactivateUI();
        }
        public void Reset()
        {
            if (hoveredEdge)
            {
                hoveredEdge.HoverExit();
                hoveredEdge.interactable = false;
            }
            DeactivateUI();
        }
        public void HandleHoverEnter(EdgeSelectable target)
        {
            target.HoverEnter();
            hoveredEdge = target;

            ActivateUI();
        }
        public void HandleHoverExit(EdgeSelectable target)
        {
            target.HoverExit();
            if (hoveredEdge == target)
            {
                hoveredEdge = null;
                DeactivateUI();
            }
        }
        void ActivateUI()
        {
            Edge edge = hoveredEdge.GetEdge();

            edgeUI.Activate();
            
            if (matrixTraveler != null)
            {

                MatrixEdgeData edgeData = matrixTraveler.matrixGraphData.edges[edge.index];
                MatrixTravelHistory travelData = matrixTraveler.travelData;
                travelData.TryGetLastTravel(out var lastTravelData);
                bool isDestinationEdge = false;
                if (lastTravelData == edgeData)
                {
                    isDestinationEdge = true;
                }

                int visitsCount = matrixTraveler.travelData.GetExitVisitCount(edgeData.startPort);
                if (isDestinationEdge)
                    visitsCount -= 1;

                edgeUI.SetEdgeData(visitsCount);
            }
            else
                edgeUI.SetEdgeData(-999);
            edgeUI.PositionAtEdgeCenter(edge);
            edgeUI.DisappearInstantly();
            edgeUI.Appear();
        }
        void DeactivateUI()
        {
            edgeUI.deactivate();
        }
    }
}
