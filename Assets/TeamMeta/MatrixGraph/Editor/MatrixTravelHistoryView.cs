using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixTravelHistoryView
    {
        IReadOnlyList<MatrixEdgeData> runtimeHistory;
        List<MatrixEdgeData> history = new List<MatrixEdgeData>();
        List<MatrixNode> matrixNodes;
        MatrixNode activeMatrixNode;
        public void GenerateNodesHistoryProperties(List<MatrixNode> matrixNodes)
        {
            this.matrixNodes = matrixNodes;
            var bgColor = Color.black;
            bgColor.a = 0.2f;

            foreach (var node in matrixNodes)
            {
                GenerateInputPortsProperties(bgColor, node);

                GenerateOutputPortsProperties(bgColor, node);
            }
        }
        public void SetRuntimeHistory(IReadOnlyList<MatrixEdgeData> runtimeHistory)
        {
            this.runtimeHistory = runtimeHistory;
        }
        public void SyncWithRuntimeHistory()
        {
            if (runtimeHistory == null)
                return;
            if (history.Count == runtimeHistory.Count)
                return;
            Color visitedColorBG = Color.blue;
            visitedColorBG.a = 0.27f;
            Color completedColorBG = Color.green;
            completedColorBG.a = 0.27f;

            while (history.Count < runtimeHistory.Count)
            {
                MatrixEdgeData edge = runtimeHistory[history.Count];
                int startNodeIndex = edge.startPort.nodeIndex;

                Port startPort=null;

                if (startNodeIndex != -1)
                {

                    matrixNodes[startNodeIndex].style.backgroundColor = completedColorBG;
                    if (edge.startPort.id != -1)
                    {

                        startPort = matrixNodes[startNodeIndex].outputContainer.Query<Port>().Where(p => int.Parse(p.portName) == edge.startPort.id);
                        startPort.style.backgroundColor = completedColorBG;

                        Label startPortVisitsCountText = startPort.Query<VisualElement>().Where(element => element.name == "visitsCountText").First() as Label;

                        int startPortVisitsCount = int.Parse(startPortVisitsCountText.text);
                        startPortVisitsCount++;
                        startPortVisitsCountText.text = startPortVisitsCount.ToString();
                    }

                }
                int endNodeIndex = edge.endPort.nodeIndex;
                if(matrixNodes[endNodeIndex].style.backgroundColor!= completedColorBG)
                    matrixNodes[endNodeIndex].style.backgroundColor = visitedColorBG;          

                Port endPort = matrixNodes[endNodeIndex].inputContainer.Query<Port>().Where(p => int.Parse(p.portName) == edge.endPort.id);
                Label endPortVisitsCountText = endPort.Query<VisualElement>().Where(element => element.name == "visitsCountText").First() as Label;

                int visitsCount = int.Parse(endPortVisitsCountText.text);
                visitsCount++;
                endPortVisitsCountText.text = visitsCount.ToString();

                endPort.style.backgroundColor = completedColorBG;

                if(startPort!=null && endPort!=null)
                {
                    var usedEdgeColor = completedColorBG;
                    usedEdgeColor.a = 1;
                    startPort.portColor = usedEdgeColor;
                    endPort.portColor = usedEdgeColor;

                    var enumarator = startPort.connections.GetEnumerator();
                    enumarator.MoveNext();
                    enumarator.Current.UpdateEdgeControl();
                }

                history.Add(edge);
            }

            UpdateActiveNode();
        }
        private void UpdateActiveNode()
        {
            //Deactivate prev active node
            if (activeMatrixNode != null)
            {
                activeMatrixNode.style.borderBottomColor = Color.black;
                activeMatrixNode.style.borderLeftColor = Color.black;
                activeMatrixNode.style.borderRightColor = Color.black;
                activeMatrixNode.style.borderTopColor = Color.black;

                activeMatrixNode.style.borderBottomWidth = 0;
                activeMatrixNode.style.borderLeftWidth = 0;
                activeMatrixNode.style.borderRightWidth = 0;
                activeMatrixNode.style.borderTopWidth = 0;
            }

            //Find and set current Active node
            var edge = history[history.Count - 1];

            int endNodeIndex = edge.endPort.nodeIndex;
            activeMatrixNode = matrixNodes[endNodeIndex];

            ColorUtility.TryParseHtmlString("#FF6600", out var activeColor);

            activeColor.a = 0.5f;
            activeMatrixNode.style.borderBottomColor = activeColor;
            activeMatrixNode.style.borderLeftColor = activeColor;
            activeMatrixNode.style.borderRightColor = activeColor;
            activeMatrixNode.style.borderTopColor = activeColor;

            activeMatrixNode.style.borderBottomWidth = 2f;
            activeMatrixNode.style.borderLeftWidth = 2f;
            activeMatrixNode.style.borderRightWidth = 2f;
            activeMatrixNode.style.borderTopWidth = 2f;
        }
        private void GenerateInputPortsProperties(Color bgColor, MatrixNode node)
        {
            var ports = node.inputContainer.Query<Port>().ToList();

            VisualElement portsHeader = GeneratePortsHeader(bgColor);

            Label visitsCountLabel = GenerateVisitsCountLabel();
            visitsCountLabel.style.marginLeft = 4f;
            visitsCountLabel.style.borderLeftColor = Color.black;
            visitsCountLabel.style.borderLeftWidth = 0.1f;

            Label portIdLabel = GeneratePortIdLabel();
            portIdLabel.style.right = -1.5f;
            portIdLabel.style.unityTextAlign = TextAnchor.MiddleRight;

            portsHeader.Add(portIdLabel);
            portsHeader.Add(visitsCountLabel);

            node.inputContainer.Insert(0, portsHeader);

            foreach (var port in ports)
            {
                Label visitsCountText = GenerateVisitsCountText(bgColor);

                visitsCountText.style.marginLeft = 4f;

                port.Add(visitsCountText);
            }
        }
        private void GenerateOutputPortsProperties(Color bgColor, MatrixNode node)
        {
            var ports = node.outputContainer.Query<Port>().ToList();

            VisualElement portsHeader = GeneratePortsHeader(bgColor);
            Label visitsCountLabel = GenerateVisitsCountLabel();
            visitsCountLabel.style.marginRight = 4f;
            visitsCountLabel.style.borderRightColor = Color.black;
            visitsCountLabel.style.borderRightWidth = 0.1f;

            Label portIdLabel = GeneratePortIdLabel();
            portIdLabel.style.right = 1.5f;

            portsHeader.Add(visitsCountLabel);
            portsHeader.Add(portIdLabel);

            node.outputContainer.Insert(0, portsHeader);

            foreach (var port in ports)
            {
                Label visitsCountText = GenerateVisitsCountText(bgColor);

                visitsCountText.style.marginRight = 4f;

                port.Add(visitsCountText);
            }
        }
        private VisualElement GeneratePortsHeader(Color bgColor)
        {
            VisualElement portsHeader = new VisualElement();
            portsHeader.style.flexDirection = FlexDirection.Row;
            portsHeader.name = "header";
            portsHeader.style.bottom = 3.6f;
            portsHeader.style.backgroundColor = bgColor;
            portsHeader.style.borderBottomColor = Color.black;
            portsHeader.style.borderBottomWidth = 0.1f;
            return portsHeader;
        }
        private Label GenerateVisitsCountText(Color bgColor)
        {
            Label visitsCountText = new Label("0");
            visitsCountText.name = "visitsCountText";
            visitsCountText.style.unityTextAlign = TextAnchor.MiddleCenter;

            visitsCountText.style.flexGrow = 1;

            visitsCountText.style.backgroundColor = bgColor;
            visitsCountText.style.borderLeftColor = Color.black;
            visitsCountText.style.borderRightColor = Color.black;
            visitsCountText.style.borderLeftWidth = 0.1f;
            visitsCountText.style.borderRightWidth = 0.1f;
            return visitsCountText;
        }
        private Label GeneratePortIdLabel()
        {
            Label portIdLabel = new Label("#");
            portIdLabel.style.minWidth = 32f; // Width of portName
            return portIdLabel;
        }
        private Label GenerateVisitsCountLabel()
        {
            Label visitsCountLabel = new Label("Used");
            visitsCountLabel.style.flexGrow = 1;
            visitsCountLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            
            visitsCountLabel.style.paddingLeft = 4f;
            visitsCountLabel.style.paddingRight = 4f;
            return visitsCountLabel;
        }
    }
}
