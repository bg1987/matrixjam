using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [System.Serializable]
    public struct MatrixEdgeData
    {
        public MatrixPortData startPort;
        public MatrixPortData endPort;

        public MatrixEdgeData(MatrixPortData startPort, MatrixPortData endPort)
        {
            this.startPort = startPort;
            this.endPort = endPort;
        }
        public static bool operator ==(MatrixEdgeData edgeData, MatrixEdgeData otherEdgeData)
        {
            return edgeData.startPort == otherEdgeData.startPort && edgeData.endPort == otherEdgeData.endPort;
        }
        public static bool operator !=(MatrixEdgeData edgeData, MatrixEdgeData otherEdgeData)
        {
            return !(edgeData == otherEdgeData);
        }
    }
}
