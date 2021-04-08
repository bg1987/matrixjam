using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [RequireComponent(typeof(MatrixMap.Edge))]
    public class EdgeTest : MonoBehaviour
    {
        MatrixMap.Edge edge;
        // Start is called before the first frame update
        void Awake()
        {
            edge = GetComponent<MatrixMap.Edge>();
        }
        private void Start()
        {
            edge.Init(Vector3.zero, Vector3.right * 8 + Vector3.up, Vector3.right * 16);
        }
        // Update is called once per frame
        void Update()
        {
            edge.UpdateMesh();
        }
    }
}
