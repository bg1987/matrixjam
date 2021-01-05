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
        Vector2 defaultNodeSize = new Vector2(100, 150);

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
        public MatrixNode CreateNode(string nodeName, int inputPorts, int outputPorts, Vector2 position)
        {
            var node = new MatrixNode();
            node.title = nodeName;
            node.GUID = System.Guid.NewGuid().ToString();
            node.SetPosition(new Rect(position, defaultNodeSize));

            for (int i = 0; i < inputPorts; i++)
            {
                Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
                inputPort.portName = i + "";
                node.inputContainer.Add(inputPort);
            }
            for (int i = 0; i < outputPorts; i++)
            {
                Port outputPort = GeneratePort(node, Direction.Output, Port.Capacity.Single);
                outputPort.portName = i + "";
                node.outputContainer.Add(outputPort);
            }



            this.AddElement(node);

            //node.RefreshExpandedState();
            //node.RefreshPorts();

            return node;
        }
    }
}
