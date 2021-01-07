using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    [System.Serializable]
    public struct MatrixNodeData
    {
        public int index;
        public string name;
        public List<MatrixPortData> inputPorts;
        public List<MatrixPortData> outputPorts;

        public MatrixNodeData(int index, string name) : this()
        {
            this.index = index;
            this.name = name;

            inputPorts = new List<MatrixPortData>();
            outputPorts = new List<MatrixPortData>();
        }
        public void AddInputPort(int portId)
        {
            inputPorts.Add(new MatrixPortData(portId, index));
        }
        public void AddOutputPort(int portId)
        {
            outputPorts.Add(new MatrixPortData(portId, index));
        }
    }
}
