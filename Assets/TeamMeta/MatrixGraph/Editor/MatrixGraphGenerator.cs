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
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                }
                AssetDatabase.Refresh();
                lastUsedPath = Path.GetDirectoryName(path);
            }

            return;
        }
    }
}
