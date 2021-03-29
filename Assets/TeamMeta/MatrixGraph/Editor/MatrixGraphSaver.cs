using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphSaver
    {
        static string lastSaveAsPath = "";

        MatrixGraphView graphView;
        string path;
        List<MatrixNode> nodes;
        List<Edge> edges;
        MatrixGraphConverter matrixGraphConverter = new MatrixGraphConverter();
        public MatrixGraphSaver(MatrixGraphView graphView, string path)
        {
            this.graphView = graphView;
            this.path = path;

            nodes = new List<MatrixNode>();
            edges = new List<Edge>();
        }
        public void SetPath(string path)
        {
            this.path = path;
        }
        public void Save()
        {
            RefreshGraphViewElements();

            var graphData = CreateScriptableObjectFromGraph();

            string graphDataJson = matrixGraphConverter.ToJson(graphData);

            var nodePositionsData = new List<Vector2>();
            nodes.ForEach(node => nodePositionsData.Add(node.GetPosition().position));
            string nodePositionsDataJson = SerializeWithDataContractJsonSerializer(nodePositionsData);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                    writer.WriteLine(graphDataJson);
                    writer.WriteLine(nodePositionsDataJson);
            }
            AssetDatabase.Refresh();
            Debug.Log("Saved the following json to file\n" + graphDataJson+"\n"+ nodePositionsDataJson);
        }
        public void SaveAsScriptableObject()
        {
            RefreshGraphViewElements();

            if (lastSaveAsPath == "")
                lastSaveAsPath = Application.dataPath;
            string path = EditorUtility.SaveFilePanel("Create a new Matrix Graph Scriptable Object", lastSaveAsPath, "MatrixGraphData", "asset");
            if (path.Length != 0)
            {
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                MatrixGraphSO matrixGraphData = CreateScriptableObjectFromGraph();
                AssetDatabase.CreateAsset(matrixGraphData, path);
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = matrixGraphData;
                EditorGUIUtility.PingObject(Selection.activeObject);

                lastSaveAsPath = Path.GetDirectoryName(path);
            }
        }
        public void Load()
        {
            if (!File.Exists(path))
                return;

            string graphDataJson;
            string nodePositionsJson;

            using (StreamReader reader = new StreamReader(path))
            {
                graphDataJson = reader.ReadLine()+"";
                nodePositionsJson = reader.ReadLine() + "";
            }
            MatrixGraphSO matrixGraphSO = matrixGraphConverter.ToScriptableObject(graphDataJson);

            List<Vector2> nodePositions;
            nodePositions = DeserializeWithDataContractJsonSerializer<List<Vector2>>(nodePositionsJson);

            graphView.ClearGraph();
            List<MatrixNode> matrixNodes = ConstructGraphNodesFromData(matrixGraphSO);
            ConstructGraphEdgesFromData(matrixGraphSO, matrixNodes);

            if(nodePositions!=null)
                SetNodePositions(nodePositions, matrixNodes);
            else
            {
                ArrangeNodesInCircle(matrixNodes);
                EventPropagation eventPropagation = graphView.FrameOrigin();
            }
        }

        private void ArrangeNodesInCircle(List<MatrixNode> matrixNodes)
        {
            int count = matrixNodes.Count;
            int radius = 300;
            Vector2 centerPoint = graphView.contentRect.size;
            centerPoint /= 2f;
            Vector2 nodeOffset = new Vector2(-85f, -50); //Trying to determine node's size only returns NaN

            float tau = Mathf.PI*2f;
            for (int i = 0; i < count; i++)
            {
                float t = ((float)i) / count;

                Rect nodeRect = matrixNodes[i].GetPosition();
                Vector2 nodePosition = centerPoint+ nodeOffset;
                nodePosition.x += Mathf.Cos(t* tau) * radius; // i = 0 cos = 1
                nodePosition.y += Mathf.Sin(t* tau) * radius;
                nodeRect.position = nodePosition;

                matrixNodes[i].SetPosition(nodeRect);
            }
        }

        private void SetNodePositions(List<Vector2> nodePositions, List<MatrixNode> matrixNodes)
        {
            for (int i = 0; i < nodePositions.Count; i++)
            {
                Rect nodeRect = matrixNodes[i].GetPosition();
                nodeRect.position = nodePositions[i];
                matrixNodes[i].SetPosition(nodeRect);
            }
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
                var nodeData = new MatrixNodeData(node.index, node.name,node.scenePath);
                nodeData.colorHdr1 = node.colorHdr1;
                nodeData.colorHdr2 = node.colorHdr2;
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
        List<MatrixNode> ConstructGraphNodesFromData(MatrixGraphSO matrixGraphSO)
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
        void ConstructGraphEdgesFromData(MatrixGraphSO matrixGraphSO, List<MatrixNode> matrixNodes)
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
        string SerializeWithDataContractJsonSerializer<T>(T obj)
        {
            string serializedObject;
            var jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                jsonSerializer.WriteObject(memoryStream, obj);
                serializedObject = System.Text.Encoding.ASCII.GetString(memoryStream.ToArray());
            }
            return serializedObject;
        }
        T DeserializeWithDataContractJsonSerializer<T>(string jsonObject)
        {
            T DeserializedObject;
            if (jsonObject.Length == 0)
                return default(T);
            var jsonSerializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonObject)))
            {
                DeserializedObject =(T)jsonSerializer.ReadObject(memoryStream);
            }
            return DeserializedObject;
        }
    }
}
