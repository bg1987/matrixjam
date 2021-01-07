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

            graphView.CreateNode("Matrix node 1", 1, 2, Vector2.zero);
            graphView.CreateNode("Matrix node 2", 2, 3, Vector2.one * 100);
            graphView.CreateNode("Matrix node 3", 2, 2, Vector2.one * 200);

            GenerateToolBar();
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
