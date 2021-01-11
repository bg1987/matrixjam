using MatrixJam.TeamMeta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MatrixJam
{
    public class MatrixTraveler : MonoBehaviour
    {
        [SerializeField] TextAsset matrixGraphAsset;
        public MatrixGraphSO matrixGraphData { get; private set; }
        public Object startScene;
        public Object endScene;
        public static MatrixTraveler Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;

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
            SceneManager.LoadScene(startScene.name);
        }
        public void TravelFromExit(int exitId)
        {
            //load scene & entry that connects to given exit.

            MatrixNodeData destinationNode = matrixGraphData.AdvanceTo(exitId);
            MatrixPortData destinationPort = matrixGraphData.activeNodeEntrancePort;

            PlayerData.Data.current_level = destinationNode.index;

            SceneManager.LoadScene(name);
        }

        public void WrapToRandomGame()
        {
            Debug.Log("Starting first scene at random");
            Debug.Log($"PlayerData.Data.NumGames {PlayerData.Data.NumGames}");
            //choose a random scene and load it.
            //this also start the gameplay in that scene.
            int start_sce = Random.Range(0, matrixGraphData.nodes.Count);
            WrapTo(start_sce, -1);
        }
        public void ReTravelToCurrentGame()
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
            SceneManager.LoadScene(endScene.name);
        }
        public void WrapTo(int nodeIndex,int entranceId)
        {
            MatrixNodeData destinationNode = matrixGraphData.WrapTo(nodeIndex, entranceId);
            SceneManager.LoadScene(destinationNode.scenePath);
        }
    }
}

