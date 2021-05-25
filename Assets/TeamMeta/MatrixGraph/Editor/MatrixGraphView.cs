using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphView : GraphView
    {
        Color backgroundColor = Color.gray;
        Vector2 defaultNodeSize = new Vector2(10, 10);
        public MatrixTravelHistoryView matrixTravelHistoryView { get; private set; }

        public MatrixGraphView()
        {
            ColorUtility.TryParseHtmlString("#3C3C4D", out backgroundColor);
            style.backgroundColor = backgroundColor;

            this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

        }
        private Port GeneratePort(MatrixNode node, Direction direction, Port.Capacity capacity)
        {
            Port port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));

            return port;
        }
        public void ClearGraph()
        {
            var edgesList = edges.ToList();
            var nodesList = nodes.ToList();

            foreach (var edge in edgesList)
            {
                this.RemoveElement(edge);
            }
            nodesList.ForEach(node => this.RemoveElement(node));
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach((Port port) =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }
        public MatrixNode CreateMatrixNode(MatrixNodeData nodeData , Vector2 position)
        {
            var node = new MatrixNode();

            node.index = nodeData.index;
            node.levelName = nodeData.name;
            node.scenePath = nodeData.scenePath;
            node.colorHdr1 = nodeData.colorHdr1;
            node.colorHdr2 = nodeData.colorHdr2;

            node.teamMembersJson = ConvertTeamMembersDataToJson(nodeData);

            node.title = nodeData.index+"";
            node.titleContainer.style.unityTextAlign = TextAnchor.MiddleCenter;
            node.name = nodeData.name;

            //var scenePathLabel = new Label("Scene Path");
            ////scenePathLabelStyle = scenePathLabel.style
            //scenePathLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            //scenePathLabel.style.backgroundColor = Color.black;
            //node.mainContainer.Add(scenePathLabel);
            node.GenerateFields();

            node.SetPosition(new Rect(position, defaultNodeSize));

            foreach (var portData in nodeData.inputPorts)
            {
                Port port = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
                port.portName = portData.id + "";
                node.inputContainer.Add(port);
            }
            foreach (var portData in nodeData.outputPorts)
            {
                Port port = GeneratePort(node, Direction.Output, Port.Capacity.Single);
                port.portName = portData.id + "";
                node.outputContainer.Add(port);
            }

            this.AddElement(node);

            node.RefreshExpandedState();
            node.RefreshPorts();

            return node;
        }
        
        public void ColorAsActive(MatrixNode node)
        {
            ColorUtility.TryParseHtmlString("#FF5E13", out var bgColor);
            bgColor.a = 0.5f;
            node.style.backgroundColor = bgColor;
        }
        public List<Port> GetInputPortsFromNode(Node node)
        {
            return node.inputContainer.Query<Port>().ToList();
        }
        public List<Port> GetOutputPortsFromNode(Node node)
        {
            return node.outputContainer.Query<Port>().ToList();
        }
        public void SyncWithPlayMode(string path)
        {
            MatrixTraveler matrixTraveler = Object.FindObjectOfType<MatrixTraveler>();
            if (!matrixTraveler)
            {
                Debug.Log("No MatrixTraveler to sync with");
                return;
            }
            var runtimeGraphPath = UnityEditor.AssetDatabase.GetAssetPath(matrixTraveler.MatrixGraphAsset);
            if (path != runtimeGraphPath)
            {
                Debug.Log(path + " is incompatible with Matrix Traveler's " + runtimeGraphPath + ". Must be of same path");
                return;
            }
            Debug.Log("Syncing with MatrixTraveler");
            //if (!Application.isPlaying)
            //{
            //    Debug.Log("Can't sync outside of play mode");
            //    return;
            //}

            List<MatrixNode> matrixNodes = nodes.ToList().ConvertAll(node => node as MatrixNode);
            matrixNodes.Sort((nodeA, nodeB) => nodeA.index.CompareTo(nodeB.index));

            matrixTravelHistoryView = new MatrixTravelHistoryView();
            matrixTravelHistoryView.GenerateNodesHistoryProperties(matrixNodes);

            matrixTravelHistoryView.SetRuntimeHistory(matrixTraveler.travelData.GetHistory());
            return;
        }
        private string ConvertTeamMembersDataToJson(MatrixNodeData nodeData)
        {
            var teamMembersDict = new Dictionary<string, string[]>();
            for (int i = 0; i < nodeData.teamMembers.Count; i++)
            {
                var teamMemberData = nodeData.teamMembers[i];

                teamMembersDict.Add(teamMemberData.name, teamMemberData.roles);
            }
            var teamMembersDictJson = SerializeWithJsonConvert(teamMembersDict);
            return teamMembersDictJson;
        }
        string SerializeWithJsonConvert(object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
