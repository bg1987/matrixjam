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

        public MatrixEdgeData FindEdgeWithStartPort(MatrixPortData startPort)
        {
            return FindEdgeWithStartPort(startPort.nodeIndex, startPort.id);
        }
        public MatrixEdgeData FindEdgeWithStartPort(int nodeIndex, int portId)
        {
            MatrixEdgeData edgeData = edges.Find(edge => edge.startPort.nodeIndex == nodeIndex &&
                                                         edge.startPort.id == portId);
            return edgeData;
        }
    }
}
