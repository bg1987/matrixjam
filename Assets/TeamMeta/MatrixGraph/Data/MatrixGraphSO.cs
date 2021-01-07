using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [CreateAssetMenu(fileName = "MatrixGraphData", menuName = "Matrix/MatrixGraphData", order = 1)]
    public class MatrixGraphSO : ScriptableObject
    {
        public List<MatrixNodeData> nodes;
        public List<MatrixEdgeData> edges;
    }
}
