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
        public MatrixTravelData matrixTravelData { get; private set; }

        public MatrixNodeData currentGame { get=> matrixTravelData.currentGame;}
        public MatrixPortData enteredAt { get => matrixTravelData.enteredAt;}

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
            //ToDo Refactor out
            get
            {
                return enteredAt.id;
            }
        }

        public void LoadStartScene()
        {
            SceneManager.LoadScene(startScene.name);
        }
        /// <summary> Port Id = -1 means use default entrance</summary>
        public void TravelFromExit(int exitId)
        {
            MatrixPortData startPort = currentGame.FindOutputPortById(exitId);
            MatrixEdgeData edge = matrixGraphData.FindEdgeWithStartPort(startPort);

            MatrixPortData destinationPort = edge.endPort;

            matrixTravelData.CountExit(startPort);

            WrapTo(destinationPort);
            //ToDo Refactor PlayerData 
            //PlayerData.Data.current_level = destinationNode.index;
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

        /// <summary> Port Id = -1 means use default entrance</summary>
        public void WrapTo(int nodeIndex,int entranceId)
        {
            MatrixNodeData destinationNode = matrixGraphData.nodes[nodeIndex];
            MatrixPortData destinationPort = destinationNode.FindInputPortById(entranceId);

            matrixTravelData.currentGame = matrixGraphData.nodes[nodeIndex];
            matrixTravelData.enteredAt = destinationPort;

            matrixTravelData.CountEntrance(destinationPort);
            matrixTravelData.CountGame(destinationNode);

            SceneManager.LoadScene(destinationNode.scenePath);
        }
        /// <summary> Port Id = -1 means use default entrance</summary>
        public void WrapTo(MatrixPortData portData)
        {
            WrapTo(portData.nodeIndex, portData.id);
        }
        public void SetEntranceUsedInCaseOfDefault(int id)
        {
            if (this.enteredAt.id != -1)
            {
                Debug.Log("Can only set activeNodeEntrancePort id if the node was entered from its default entrance, aka -1");
                return;
            }
            var port = enteredAt;
            port.id = id;
            matrixTravelData.enteredAt = port;
        }
    }
}

