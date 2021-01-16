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
        public void GenerateNodesHistoryProperties(List<MatrixNode> matrixNodes)
        {
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
            if(runtimeHistory!=null)
            Debug.Log(runtimeHistory.Count);

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
