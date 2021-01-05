using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphWindow : EditorWindow
    {
        MatrixGraphView graphView;

        public void OpenMatrixGraphWindow()
        {
            var window = GetWindow<MatrixGraphWindow>();
            window.Show();
            window.titleContent = new GUIContent("Matrix Graph");
        }
        private void OnEnable()
        {
            ConstructGraphView();
            graphView.CreateNode("Matrix node 1", 1, 2, Vector2.zero);
            graphView.CreateNode("Matrix node 2", 2, 3, Vector2.one * 100);
            graphView.CreateNode("Matrix node 3", 2, 2, Vector2.one * 200);
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
    }
}
