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
        //public LevelConnects[] all_connects;
        //public Object[] play_scenes;
        public Object startScene;
        public Object endScene;
        private int num_entrence;
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
        public int Numentrence
        {
            get
            {
                return num_entrence;
            }
        }
        public void LoadScene(int num_sce, int num_port)
        {
            //load the scene from given level number and level entry.
            //this does start the gameplay in that scene.
            // if num_sce = -1 will load the start scene.
            // if num_sce = -2 will load the end scene.
            switch (num_sce)
            {
                case -1:
                {
                    LoadSceneFromName("Start");
                    break;
                }
                case -2:
                {
                    LoadSceneFromName(endScene.name);
                    break;
                }
                case -3:
                {
                    //Signals the game to start from its default entrance
                    num_entrence = -1;

                    break;
                }
                default:
                {
                    num_entrence = num_port;

                    break;
                }
            }
            if (num_sce >= 0 && num_sce < matrixGraphData.nodes.Count)
            {
                PlayerData.Data.current_level = num_sce;
                LoadSceneFromNumber(num_sce);
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
            MatrixPortData destinationPort = matrixGraphData.portUsedForEntry;

            num_entrence = destinationPort.id;
            PlayerData.Data.current_level = destinationNode.index;

            LoadSceneFromName(destinationNode.scenePath);
        }
        void LoadSceneFromNumber(int num_scn)
        {
            //load the scene of given number. 
            //This do not start the gameplay in the scene!
            LoadSceneFromName(matrixGraphData.nodes[num_scn].scenePath);
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
            string scenePath = matrixGraphData.nodes[start_sce].scenePath;

            //-1 means default entrance
            matrixGraphData.WrapTo(start_sce, -1);
            num_entrence = matrixGraphData.portUsedForEntry.id;

            LoadSceneFromName(scenePath);
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
    }
}

