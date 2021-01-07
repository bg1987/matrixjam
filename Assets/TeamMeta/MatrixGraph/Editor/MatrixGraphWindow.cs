using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphWindow : EditorWindow
    {
        MatrixGraphView graphView;
        MatrixGraphSaver graphSaver;
        public string graphFilePath;

        public void OpenMatrixGraphWindow(string graphFilePath)
        {
            var window = GetWindow<MatrixGraphWindow>();
            window.Show();
            window.titleContent = new GUIContent("Matrix Graph");

            this.graphFilePath = graphFilePath;
            //graphSaver = new MatrixGraphSaver(graphView, graphFilePath);
            graphSaver = new MatrixGraphSaver(graphView, graphFilePath);

        }
        private void OnEnable()
        {
            ConstructGraphView();
            if (graphSaver == null)
            {
                //The double initialization in OnEnable and OpenWindow takes into account
                //the cases of recompilation and reopening of the window
                graphSaver = new MatrixGraphSaver(graphView, graphFilePath);
            }

            PopulateWithDummyData();

            GenerateToolBar();
        }
        private void PopulateWithDummyData()
        {
            //Dummy initial data for testing
            var matrixNodeData1 = new MatrixNodeData(0, "Level 1");
            matrixNodeData1.AddInputPort(0);
            matrixNodeData1.AddOutputPort(0);
            matrixNodeData1.AddOutputPort(3);

            var matrixNodeData2 = new MatrixNodeData(1, "Level 2");
            matrixNodeData2.AddInputPort(0);
            matrixNodeData2.AddInputPort(1);
            matrixNodeData2.AddOutputPort(0);
            matrixNodeData2.AddOutputPort(1);
            matrixNodeData2.AddOutputPort(2);

            var matrixNodeData3 = new MatrixNodeData(2, "Level 3");
            matrixNodeData3.AddInputPort(0);
            matrixNodeData3.AddInputPort(1);
            matrixNodeData3.AddOutputPort(4);
            matrixNodeData3.AddOutputPort(7);

            graphView.CreateMatrixNode(matrixNodeData1, Vector2.zero);
            graphView.CreateMatrixNode(matrixNodeData2, Vector2.one * 100);
            graphView.CreateMatrixNode(matrixNodeData3, Vector2.one * 200);
        }
        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
        private void ConstructGraphView()
        {
            graphView = new MatrixGraphView();
            graphView.name = "Matrix Graph";
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        void GenerateToolBar()
        {
            var toolBar = new Toolbar();
            rootVisualElement.Add(toolBar);

            var saveButton = new Button();
            saveButton.clicked += () => graphSaver.Save(); //ToDo something on click
            saveButton.name = "SaveButton";
            saveButton.text = "Save";
            toolBar.Add(saveButton);

        }
    }
}
