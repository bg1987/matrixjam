using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphGenerator : EditorWindow
    {
        static string lastUsedPath ="";
        [MenuItem("Matrix/Matrix Graph Generator")]
        public static void OpenMatrixGraphBuilderhWindow()
        {
            var window = GetWindow<MatrixGraphGenerator>();
            window.titleContent = new GUIContent("Matrix Graph Generator");
        }
        private void OnEnable()
        {
            if (lastUsedPath == "")
                lastUsedPath = Application.dataPath;

            Button button = new Button();
            button.text = "Create a Matrix Graph File";
            button.clicked += CreateMatrixGraphFile;
            this.rootVisualElement.Add(button);
        }
        
        private void CreateMatrixGraphFile()
        {
            string path = EditorUtility.SaveFilePanel("Create a new Matrix Graph", lastUsedPath, "NewMatrixGraph", "matrixgraph");
            if (path.Length != 0)
            {
                MatrixGraphSO graphData = CreateDummyScriptableObject();
                string graphDataJson = JsonUtility.ToJson(graphData);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(graphDataJson);
                    }
                }
                AssetDatabase.Refresh();
                lastUsedPath = Path.GetDirectoryName(path);
            }
            return;
        }
        private MatrixGraphSO CreateDummyScriptableObject()
        {
            MatrixGraphSO matrixGraphSO = CreateInstance<MatrixGraphSO>();

            var nodes = new List<MatrixNodeData>();

            var node1 = new MatrixNodeData(0, "Level 1");
            node1 .AddInputPort(0);
            node1 .AddOutputPort(0);
            node1.AddOutputPort(3);
            nodes.Add(node1);

            var node2 = new MatrixNodeData(1, "Level 2");
            node2.AddInputPort(0);
            node2.AddInputPort(1);
            node2.AddOutputPort(0);
            node2.AddOutputPort(1);
            node2.AddOutputPort(2);
            nodes.Add(node2);

            var node3 = new MatrixNodeData(2, "Level 3");
            node3.AddInputPort(0);
            node3.AddInputPort(1);
            node3.AddOutputPort(4);
            node3.AddOutputPort(7);
            nodes.Add(node3);

            var node4 = new MatrixNodeData(3, "Level 4");
            node4.AddInputPort(0);
            node4.AddOutputPort(0);
            nodes.Add(node4);

            var edges = new List<MatrixEdgeData>();

            matrixGraphSO.nodes = nodes;
            matrixGraphSO.edges = edges;

            return matrixGraphSO;
        }
    }
}
