using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class MatrixMap : MonoBehaviour
    {
        [SerializeField] Node nodePrefab;
        [SerializeField] int nodesCount;

        [SerializeField] float nodesSize = 1;
        [SerializeField] float radius = 1;

        List<Node> nodes = new List<Node>();
        List<Node> visitedNodes = new List<Node>();
        List<Vector3> nodesPositions = new List<Vector3>();

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
            if(MatrixTraveler.Instance)
                CreateNodes(MatrixTraveler.Instance);
            else
                CreateNodes();

            CalculateNodesPositions();
            UpdateNodesPositions();
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
    }
}
