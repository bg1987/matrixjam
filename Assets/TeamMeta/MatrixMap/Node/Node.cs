using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class Node : MonoBehaviour
    {
        [SerializeField] GameObject model;

        List<Edge> startPortEdges = new List<Edge>();
        List<Edge> endPortEdges = new List<Edge>();

        //Edge Creation
        private List<int> EdgesNormalSign = new List<int>(); //1 positive, -1 negative

        // Start is called before the first frame update
        void Start()
        {
            
        }
        public void AddToStartPortEdges(Edge edge)
        {
            endPortEdges.Add(edge);
        }
        public void AddToEndPortEdges(Edge edge)
        {
            endPortEdges.Add(edge);
        }
    }
}
