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
    }
}
