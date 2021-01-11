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
        public MatrixNodeData activeNode { get; private set; }
        public MatrixPortData activeNodeEntrancePort { get; private set; }

        public MatrixNodeData AdvanceTo(int portId)
        {
            MatrixEdgeData edge = FindEdgeWithStartPort(activeNode.index, portId);

            activeNodeEntrancePort = edge.endPort;
            activeNode = nodes[activeNodeEntrancePort.nodeIndex];

            return activeNode;
        }
        /// <param name="portId">
        /// -1 Means use default entrance
        /// </param>
        /// <returns></returns>
        public MatrixNodeData WrapTo(int nodeIndex,int portId)
        {
            activeNode = nodes[nodeIndex];
            if (portId == -1)
                activeNodeEntrancePort = new MatrixPortData(-1, nodeIndex);
            else
                activeNodeEntrancePort = activeNode.inputPorts.Find(port => port.id == portId);

            return activeNode;
        }
        public void SetEntrancePortIdInCaseOfDefault(int id)
        {
            if (activeNodeEntrancePort.id != -1)
            {
                Debug.Log("Can only set activeNodeEntrancePort id if the node was entered from its default entrance, aka -1");
                return;
            }
            var port = activeNodeEntrancePort;
            port.id = id;
            activeNodeEntrancePort = port;
        }
        private MatrixEdgeData FindEdgeWithStartPort(MatrixPortData startPort)
        {
            return FindEdgeWithStartPort(startPort.nodeIndex, startPort.id);
        }
        private MatrixEdgeData FindEdgeWithStartPort(int nodeIndex, int portId)
        {
            MatrixEdgeData edgeData = edges.Find(edge => edge.startPort.nodeIndex == nodeIndex &&
                                                         edge.startPort.id == portId);
            return edgeData;
        }
    }
}
