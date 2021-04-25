using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class EdgeSelector : MonoBehaviour
    {
        EdgeSelectable hoveredEdge;

        // Start is called before the first frame update
        void Start()
        {
            
        }
        public void Reset()
        {
            if (hoveredEdge)
                hoveredEdge.HoverExit();   
        }
        public void HandleHoverEnter(EdgeSelectable target)
        {
            target.HoverEnter();
            hoveredEdge = target;
        }
        public void HandleHoverExit(EdgeSelectable target)
        {
            target.HoverExit();
            if (hoveredEdge == target)
                hoveredEdge = null;
        }
    }
}
