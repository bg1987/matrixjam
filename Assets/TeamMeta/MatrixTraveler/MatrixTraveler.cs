using MatrixJam.TeamMeta;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MatrixJam
{
    public class MatrixTraveler : MonoBehaviour
    {
        [SerializeField] TextAsset matrixGraphAsset;
        public TextAsset MatrixGraphAsset { get=> matrixGraphAsset; }
        public MatrixGraphSO matrixGraphData { get; private set; }
        public MatrixTravelHistory travelData { get; private set; }

        public string startScene;
        public string endScene;
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

            travelData = new MatrixTravelHistory();
        }

        public void LoadFromDisk()
        {
            travelData.LoadFromDisk();
            ReTravelToCurrentGame();

        }
        public int entranceId
        {
            //ToDo Refactor out
            get
            {
                return GetLastUsedEntrance().id;
            }
        }
        public MatrixNodeData GetCurrentGame()
        {
            bool success = travelData.TryGetLastTravel(out MatrixEdgeData matrixEdgeData);
            if (success)
                return matrixGraphData.nodes[matrixEdgeData.endPort.nodeIndex];
            else
            {
                Debug.Log("MatrixTraveler has not traveled to any game yet");
                return new MatrixNodeData(-1,"Undefined Game","");
            }
        }
        public MatrixPortData GetLastUsedEntrance()
        {
            bool success = travelData.TryGetLastTravel(out MatrixEdgeData matrixEdgeData);
            if (success)
                return matrixEdgeData.endPort;
            else
            {
                Debug.Log("MatrixTraveler has not traveled to any game yet");
                return new MatrixPortData(-1,-1);
            }
        }
        public void LoadStartScene()
        {
            SceneManager.LoadScene(startScene);
        }

        public void TravelFromExit(int exitId)
        {
            bool success = GetCurrentGame().FindOutputPortById(exitId,out MatrixPortData startPort);
            if(!success)
            {
                Debug.Log("Exit "+exitId+" of game "+GetCurrentGame().index+" doesn't exist");
                return;
            }
            MatrixEdgeData edge = matrixGraphData.FindEdgeWithStartPort(startPort);

            MatrixPortData destinationPort = edge.endPort;
            MatrixNodeData destinationGame = matrixGraphData.nodes[destinationPort.nodeIndex];

            travelData.AddTravel(startPort, destinationPort);
            //ToDo Refactor PlayerData 
            //PlayerData.Data.current_level = destinationGame.index;
            if (travelData.GetCompletedGamesCount() == matrixGraphData.nodes.Count)
            {
                MatrixOver();
                return;
            }
            SceneManager.LoadScene(destinationGame.scenePath);
        }

        public void WarpToRandomGame()
        {
            Debug.Log("Starting first scene at random");
            Debug.Log($"PlayerData.Data.NumGames {PlayerData.Data.NumGames}");
            //choose a random scene and load it.
            //this also start the gameplay in that scene.
            int start_sce = UnityEngine.Random.Range(0, matrixGraphData.nodes.Count);
            WarpTo(start_sce, -1);
        }
        public void ReTravelToCurrentGame()
        {
            //restart the level from the start
            //this start the gameplay in the scene.
            //if (LevelHolder.Level != null)
            //{
            //    LevelHolder.Level.Restart();
            //}
            MatrixNodeData game = GetCurrentGame();
            if (game.index >= 0)
                SceneManager.LoadScene(game.scenePath);
            else
                Debug.Log("Can't retravel to current game as current game's index is -1");
        }
        public void MatrixOver()
        {
            //end the matrix and start the end scene
            Debug.Log("MatrixOver!");
            SceneManager.LoadScene(endScene);
        }

        /// <summary> Entrance Id = -1 means use default entrance</summary>
        public void WarpTo(int nodeIndex,int entranceId)
        {
            if(nodeIndex>=matrixGraphData.nodes.Count || nodeIndex<0)
            {
                Debug.Log("There is no game with index " + nodeIndex);
                return;
            }
            MatrixNodeData destinationNode = matrixGraphData.nodes[nodeIndex];
            if (entranceId <-1)
            {
                Debug.Log("There is no entrance with id " + nodeIndex + "in game "+nodeIndex);
                return;
            }
            MatrixPortData destinationPort;
            if (entranceId == -1)
            {
                //Use default entrance
                destinationPort = new MatrixPortData(entranceId, nodeIndex);
            }
            else
            {
                var success = destinationNode.FindInputPortById(entranceId, out destinationPort);
                if(!success)
                {
                    Debug.Log("There is no entrance with id " + nodeIndex + "in game " + nodeIndex);
                    return;
                }

            }

            MatrixNodeData destinationGame = destinationNode;

            travelData.AddTravel(new MatrixPortData(-1,-1), destinationPort);

            SceneManager.LoadScene(destinationNode.scenePath);
        }
        /// <summary> Entrance Id = -1 means use default entrance</summary>
        public void WarpTo(MatrixPortData portData)
        {
            WarpTo(portData.nodeIndex, portData.id);
        }
        public void SetEntranceUsedInCaseOfDefault(int id)
        {
            MatrixPortData lastUsedEntrance = GetLastUsedEntrance();
            if (lastUsedEntrance.id != -1)
            {
                Debug.Log("Can only set last used entrance id if the game was entered from its default entrance, aka -1");
                return;
            }
            travelData.AmendLastTravelDestinationPortId(id);
        }
    }
}

