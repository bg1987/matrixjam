using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [System.Serializable]
    public struct MatrixPortData
    {
        public int id;
        public int nodeIndex;

        public MatrixPortData(int id, int nodeIndex)
        {
            this.id = id;
            this.nodeIndex = nodeIndex;
        }
        public static bool operator ==(MatrixPortData portData, MatrixPortData otherPortData)
        {
            return portData.id == otherPortData.id && portData.nodeIndex == otherPortData.nodeIndex;
        }
        public static bool operator !=(MatrixPortData portData, MatrixPortData otherPortData)
        {
            return !(portData == otherPortData);
        }
    }
}
