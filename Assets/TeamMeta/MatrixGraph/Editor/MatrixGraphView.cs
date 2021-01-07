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

            node.title = nodeData.name;
            node.name = nodeData.name;
            
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
        public List<Port> GetInputPortsFromNode(Node node)
        {
            return node.inputContainer.Query<Port>().ToList();
        }
        public List<Port> GetOutputPortsFromNode(Node node)
        {
            return node.outputContainer.Query<Port>().ToList();
        }
    }
}
