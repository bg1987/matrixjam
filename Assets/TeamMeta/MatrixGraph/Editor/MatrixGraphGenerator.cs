using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.IO;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MatrixJam.TeamMeta
{
    public class MatrixGraphGenerator : EditorWindow
    {
        public List<UnityEngine.Object> scenes;
        private SerializedObject serializedObject;
        ListView listView;
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

            scenes = new List<UnityEngine.Object>();
            serializedObject = new SerializedObject(this);

            GenerateScenesLabel();
            GenerateScenesList();
            GenerateCreateMatrixButton();
        }

        private Button GenerateCreateMatrixButton()
        {
            Button button = new Button();
            button.text = "Create a Matrix Graph File";
            button.clicked += CreateMatrixGraphFile;
            rootVisualElement.Add(button);

            return button;
        }

        private ListView GenerateScenesList()
        {
            listView = new ListView();
            listView.bindingPath = nameof(scenes);
            listView.itemHeight = 20;

            var listProperty = this.serializedObject.FindProperty(listView.bindingPath);
            var listPropertySize = this.serializedObject.FindProperty(listView.bindingPath + ".Array.size");

            listPropertySize.intValue = 1;
            serializedObject.ApplyModifiedProperties();
            listView.style.flexGrow = 1;

            rootVisualElement.Add(listView);
            listView.Bind(serializedObject);
            rootVisualElement.Bind(serializedObject);

            return listView;
        }
        private VisualElement GenerateScenesLabel()
        {
            var scenesListLabel = new Label("Scenes For Matrix Creation");
            var scenesLabelStyle = scenesListLabel.style;
            scenesLabelStyle.unityTextAlign = TextAnchor.MiddleCenter;
            ColorUtility.TryParseHtmlString("#B0B8C2", out var labelColorBG);
            scenesLabelStyle.backgroundColor = labelColorBG;

            rootVisualElement.Add(scenesListLabel);

            return scenesListLabel;
        }
        private void CreateMatrixGraphFile()
        {
            string path = EditorUtility.SaveFilePanel("Create a new Matrix Graph", lastUsedPath, "NewMatrixGraph", "matrixgraph");
            if (path.Length != 0)
            {
                MatrixGraphSO graphData = CreateScriptableObjectOutOfScenes();
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
        private MatrixGraphSO CreateScriptableObjectOutOfScenes()
        {
            MatrixGraphSO matrixGraphSO = CreateInstance<MatrixGraphSO>();

            var initialScene = EditorSceneManager.GetActiveScene();
            string initialScenePath = initialScene.path;
            var nodes = new List<MatrixNodeData>();
            for (int i = 0; i < scenes.Count; i++)
            {
                UnityEngine.Object scene = scenes[i];
                if (scene == null)
                {
                    Debug.Log("Scene at index " + i + " is null");
                    continue;
                }

                string scenePath = AssetDatabase.GetAssetPath(scene);
                Scene activeScene = EditorSceneManager.OpenScene(scenePath);

                var node = new MatrixNodeData(i, scene.name);

                foreach (var rootGameObject in activeScene.GetRootGameObjects())
                {
                    Entrance[] entrances = rootGameObject.GetComponentsInChildren<Entrance>();
                    foreach (var entrance in entrances)
                    {
                        node.AddInputPort(entrance.num_portal);
                    }
                   
                    Exit[] exits = rootGameObject.GetComponentsInChildren<Exit>();
                    foreach (var exit in exits)
                    {
                        node.AddOutputPort(exit.num_portal);
                    }
                }
                nodes.Add(node);
            }
            EditorSceneManager.OpenScene(initialScenePath);

            matrixGraphSO.nodes = nodes;

            return matrixGraphSO;
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
