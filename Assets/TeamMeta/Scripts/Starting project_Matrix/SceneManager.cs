using MatrixJam.TeamMeta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MatrixJam
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] TextAsset matrixGraphAsset;
        public MatrixGraphSO matrixGraphData { get; private set; }
        public Object startScene;
        public Object endScene;
        private static SceneManager scenemg;
        public static SceneManager SceneMang
        {
            get
            {
                if (scenemg == null)
                {
                    scenemg = GameObject.FindObjectOfType<SceneManager>();
                }
                return scenemg;
            }
        }
        private void Awake()
        {
            MatrixGraphConverter matrixGraphConverter = new MatrixGraphConverter();
            matrixGraphData = matrixGraphConverter.ToScriptableObject(matrixGraphAsset.text);
        }
        public int entranceId
        {
            get
            {
                return matrixGraphData.activeNodeEntrancePort.id;
            }
        }

        public void LoadStartScene()
        {
            LoadSceneFromName(startScene.name);
        }
        public void LoadSceneFromExit(int num_sce, int int_exit)
        {
            //load scene & entry that connects to given exit.

            MatrixNodeData destinationNode = matrixGraphData.AdvanceTo(int_exit);
            MatrixPortData destinationPort = matrixGraphData.activeNodeEntrancePort;

            PlayerData.Data.current_level = destinationNode.index;

            LoadSceneFromName(destinationNode.scenePath);
        }
        void LoadSceneFromName(string name)
        {
            //the scene from memory.
            //This do not start the gameplay in the scene!
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }
        public void LoadRandomScene()
        {
            Debug.Log("Starting first scene at random");
            Debug.Log($"PlayerData.Data.NumGames {PlayerData.Data.NumGames}");
            //choose a random scene and load it.
            //this also start the gameplay in that scene.
            int start_sce = Random.Range(0, matrixGraphData.nodes.Count);
            WrapTo(start_sce, -1);
        }
        public void ResetLevelScene()
        {
            //restart the level from the start
            //this start the gameplay in the scene.
            if (LevelHolder.Level != null)
            {
                LevelHolder.Level.Restart();
            }
        }
        public void MatrixOver()
        {
            //end the matrix and start the end scene
            Debug.Log("MatrixOver!");
            LoadSceneFromName(endScene.name);

        }
        public void WrapTo(int nodeIndex,int entranceId)
        {
            MatrixNodeData destinationNode= matrixGraphData.WrapTo(nodeIndex, entranceId);
            LoadSceneFromName(destinationNode.scenePath);
        }
    }
}

