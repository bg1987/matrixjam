using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeSelector : MonoBehaviour
    {
        EdgeSelectable hoveredEdge;

        [SerializeField] EdgeUI edgeUI;
        [SerializeField] EdgesUIs edgeUis;

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
            if (edgeUI)
            {
                edgeUI.DisappearInstantly();
                edgeUI = null;
            }
            edgeUis.Deactivate();
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
                edgeUI.Disappear();
                edgeUI = null;
            }
        }
        void ActivateUI()
        {
            Edge edge = hoveredEdge.GetEdge();
            if (edgeUI)
                edgeUI.Disappear();
            edgeUI = edgeUis.uis[edge.index];
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
            //edgeUI.DisappearInstantly();
            edgeUI.SetLineStartColor(edge.Material.color);
            edgeUI.Appear();
        }
        void DeactivateUI()
        {
            edgeUI.Deactivate();
        }
    }
}
