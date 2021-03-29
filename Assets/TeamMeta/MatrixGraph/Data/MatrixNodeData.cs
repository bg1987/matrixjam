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
        public string scenePath;
        public ColorHdr colorHdr1;
        public ColorHdr colorHdr2;

        public List<MatrixPortData> inputPorts;
        public List<MatrixPortData> outputPorts;

        public MatrixNodeData(int index, string name, string scenePath) : this()
        {
            this.index = index;
            this.name = name;
            this.scenePath = scenePath;
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
        public bool FindInputPortById(int id, out MatrixPortData matrixPortData)
        {
            foreach (var port in inputPorts)
            {
                if (port.id == id)
                {
                    matrixPortData = port;
                    return true;
                }
            }
            matrixPortData = new MatrixPortData(-1, index);
            return false;
        }
        public bool FindOutputPortById(int id,out MatrixPortData matrixPortData)
        {
            foreach (var port in outputPorts)
            {
                if (port.id == id)
                {
                    matrixPortData = port;
                    return true;
                }
            }
            matrixPortData = new MatrixPortData(-1, index);
            return false;
        }
    }
}
