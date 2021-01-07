using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphSaver
    {
        MatrixGraphView graphView;
        string path;
        List<MatrixNode> nodes;
        List<Edge> edges;
        public MatrixGraphSaver(MatrixGraphView graphView, string path)
        {
            this.graphView = graphView;
            this.path = path;

            nodes = new List<MatrixNode>();
            edges = new List<Edge>();
        }
        public void Save()
        {
            RefreshGraphViewElements();

            var graphData = CreateScriptableObjectFromGraph();

            var graphDataJson = JsonUtility.ToJson(graphData);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                    writer.WriteLine(graphDataJson);
            }
            Debug.Log("Saved the following json to file\n" + graphDataJson);
        }
        public void Load()
        {
            if (!File.Exists(path))
                return;

            string graphDataJson;
            using (StreamReader reader = new StreamReader(path))
            {
                graphDataJson = reader.ReadLine();
            }

            var matrixGraphSO = ScriptableObject.CreateInstance<MatrixGraphSO>();
            JsonUtility.FromJsonOverwrite(graphDataJson, matrixGraphSO);

            graphView.ClearGraph();
            List<MatrixNode> matrixNodes = ConstructGraphNodesFromData(matrixGraphSO);
            ConstructGraphEdgesFromData(matrixGraphSO, matrixNodes);
        }
        void RefreshGraphViewElements()
        {
            nodes.Clear();
            graphView.nodes.ToList().ForEach(node => nodes.Add(node as MatrixNode));
            nodes.Sort((nodeA, nodeB) => nodeA.index.CompareTo(nodeB.index));
            edges = graphView.edges.ToList();
        }
        MatrixGraphSO CreateScriptableObjectFromGraph()
        {
            MatrixGraphSO matrixGraphData = ScriptableObject.CreateInstance<MatrixGraphSO>();

            List<MatrixNodeData> nodesData = CreateNodesData();
            List<MatrixEdgeData> edgesData = CreateEdgesData(nodesData);

            matrixGraphData.nodes = nodesData;
            matrixGraphData.edges = edgesData;

            return matrixGraphData;
        }
        List<MatrixNodeData> CreateNodesData()
        {
            var nodesData = new List<MatrixNodeData>();
            foreach (MatrixNode node in nodes)
            {
                var nodeData = new MatrixNodeData(node.index, node.name);

                //Handle input ports
                List<Port> inputPorts = graphView.GetInputPortsFromNode(node);
                foreach (var port in inputPorts)
                {
                    var success = int.TryParse(port.portName, out int portId);
                    if (!success)
                        Debug.LogError("Port Name has to be an a number");

                    nodeData.AddInputPort(portId);
                }
                //Handle output ports
                List<Port> outputPorts = graphView.GetOutputPortsFromNode(node);
                foreach (var port in outputPorts)
                {
                    var success = int.TryParse(port.portName, out int portId);
                    if (!success)
                        Debug.LogError("Port Name has to be an a number");

                    nodeData.AddOutputPort(portId);
                }

                nodesData.Add(nodeData);
            }
            return nodesData;
        }
        List<MatrixEdgeData> CreateEdgesData(List<MatrixNodeData> nodesData)
        {
            var edgesData = new List<MatrixEdgeData>();

            foreach (var edge in edges)
            {
                var edgeData = new MatrixEdgeData();

                //start port
                MatrixPortData startPort;

                int startNodeIndex = (edge.output.node as MatrixNode).index;
                MatrixNodeData startNode = nodesData[startNodeIndex];

                int.TryParse(edge.output.portName, out int startPortId);
                startPort = startNode.outputPorts.Find(port => port.id == startPortId);

                edgeData.startPort = startPort;

                //end port
                MatrixPortData endPort;

                int endNodeIndex = (edge.input.node as MatrixNode).index;
                MatrixNodeData endNode = nodesData[endNodeIndex];

                int.TryParse(edge.input.portName, out int endPortId);
                endPort = endNode.inputPorts.Find(port => port.id == endPortId);

                edgeData.endPort = endPort;

                edgesData.Add(edgeData);
            }

            return edgesData;
        }
        private List<MatrixNode> ConstructGraphNodesFromData(MatrixGraphSO matrixGraphSO)
        {
            List<MatrixNode> matrixNodes = new List<MatrixNode>();
            foreach (var nodeData in matrixGraphSO.nodes)
            {
                //ToDo give a real position to node
                var node = graphView.CreateMatrixNode(nodeData, nodeData.index*100*Vector2.one); 
                matrixNodes.Add(node);
            }

            return matrixNodes;
        }
        private void ConstructGraphEdgesFromData(MatrixGraphSO matrixGraphSO, List<MatrixNode> matrixNodes)
        {
            foreach (var edgeData in matrixGraphSO.edges)
            {
                var edge = new Edge();

                //Edge output
                Port startPort;

                var startNode = matrixNodes[edgeData.startPort.nodeIndex];
                var startNodeOutputPorts = graphView.GetOutputPortsFromNode(startNode);
                startPort = startNodeOutputPorts.Find(port => int.Parse(port.portName) == edgeData.startPort.id);

                edge.output = startPort;

                //Edge input
                Port endPort;

                var endNode = matrixNodes[edgeData.endPort.nodeIndex];
                var endNodeInputPorts = graphView.GetInputPortsFromNode(endNode);
                endPort = endNodeInputPorts.Find(port => int.Parse(port.portName) == edgeData.endPort.id);

                edge.input = endPort;

                //Construct edge
                edge.input.Connect(edge);
                edge.output.Connect(edge);

                graphView.AddElement(edge);
            }
        }
    }
}
