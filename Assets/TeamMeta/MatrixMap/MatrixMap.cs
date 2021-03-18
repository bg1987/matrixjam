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
            CalculateNodesPositions();
            CreateNodes();
            //UpdateNodesScale();
            //UpadteNodesRadius();

            //DebugVisualizeFirstLastNodes();
        }
        private List<Vector3> CalculateNodesPositions()
        {
            nodesPositions.Clear();

            float rotateOffset = -Mathf.PI / 2f; //90 degrees. For visual symmetery

            for (int i = 0; i < nodesCount; i++)
            {
                float t = i / (float)nodesCount;
                float rotateBy = t * TAU + rotateOffset;
                nodesPositions.Add(new Vector3(Mathf.Cos(rotateBy), Mathf.Sin(rotateBy), transform.position.z) * radius);
            }
            return nodesPositions;
        }
        void CreateNodes()
        {
            for (int i = 0; i < nodesCount; i++)
            {
                var node = Instantiate(nodePrefab, transform);
                node.transform.position = nodesPositions[i];
                node.name = "Node"+i;
                node.transform.localScale = Vector3.one * nodesSize;
                nodes.Add(node);
            }

        }
    }
}
