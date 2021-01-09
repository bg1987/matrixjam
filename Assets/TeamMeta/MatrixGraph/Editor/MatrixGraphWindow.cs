using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphWindow : EditorWindow
    {
        MatrixGraphView graphView;
        MatrixGraphSaver graphSaver;
        public string graphFilePath="";

        public void OpenMatrixGraphWindow(string graphFilePath)
        {
            var window = GetWindow<MatrixGraphWindow>();
            window.Show();
            window.titleContent = new GUIContent("Matrix Graph");

            this.graphFilePath = graphFilePath;

            graphSaver.SetPath(graphFilePath);
            LoadAfterGraphViewFinishInitialization();
        }
        private void OnEnable()
        {
            ConstructGraphView();
            graphSaver = new MatrixGraphSaver(graphView, graphFilePath);
            GenerateToolBar();

            if (graphFilePath.Length>0)
            {
                LoadAfterGraphViewFinishInitialization();
            }
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
            saveButton.clicked += () => graphSaver.Save();
            saveButton.name = "SaveButton";
            saveButton.text = "Save";
            toolBar.Add(saveButton);

            var loadButton = new Button();
            loadButton.clicked += () => graphSaver.Load();
            loadButton.name = "loadButton";
            loadButton.text = "Load";
            toolBar.Add(loadButton);

            var saveAsButton = new Button();
            saveAsButton.clicked += () => graphSaver.SaveAsScriptableObject();
            saveAsButton.name = "saveAsButton";
            saveAsButton.text = "Save As Scriptable Object";
            toolBar.Add(saveAsButton);

        }
        async void LoadAfterGraphViewFinishInitialization()
        {
            //Hack to wait for graphView to finish resolving its width & height
            //This function might be better inside graphSaver
            //ToDo Test out element.RegisterCallback<GeometryChangedEvent>(myCallback);

            var timeoutCount = 0;
            var timeStep = 10;
            while (float.IsNaN(graphView.contentRect.width) && timeoutCount< 5000f)
            {
                timeoutCount += timeStep;

                await Task.Delay(timeStep);
            }
            graphSaver.Load();
        }
    }
}
