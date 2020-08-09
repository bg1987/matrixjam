using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MatrixJam
{
    public class SceneManager : MonoBehaviour
    {

        public LevelConnects[] all_connects;
        public Object[] play_scenes;
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
        public int Numentrence
        {
            get
            {
                return num_entrence;
            }
        }
        public void LoadScene(int num_sce, int num_port)
        {

            switch (num_sce)
            {
                case -1:
                    {
                        LoadSceneFromName("Start");
                        break;
                    }
                case -2:
                    {
                        LoadSceneFromName("End");
                        break;
                    }
                default:
                    {
                        if (num_sce >= 0)
                        {
                            num_entrence = num_port;
                            PlayerData.Data.current_level = num_sce;
                            LoadSceneFromNumber(num_sce);
                        }
                        break;
                    }
            }

        }
        public void LoadSceneFromConnectionMem(Connection con)
        {
            LoadScene(con.scene_from, con.portal_from);
        }
        public void LoadSceneFromExit(int num_sce, int int_exit)
        {
            Connection ent_point = FindConnectTo(num_sce, int_exit);
            LoadScene(ent_point.scene_to, ent_point.portal_to);
        }
        public void LoadSceneFromNumber(int num_scn)
        {
            LoadSceneFromName(play_scenes[num_scn - 1].name);
        }

        public void LoadSceneFromName(string name)
        {
           //only load the scene from memory. This do not start the gameplay in the scene!
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }
        public Connection FindConnectTo(int level, int exit)
        {
            return all_connects[level].FindConnect(exit, true);
        }
        public void LoadRandomScene()
        {
            int start_sce = 1 + Random.Range(0, PlayerData.Data.NumGames);
            LoadScene(start_sce, 0);
        }
        public void ResetLevelScene()
        {
            if (LevelHolder.Level != null)
            {
                LevelHolder.Level.Restart();
            }
        }
        public void MatrixOver()
        {
            Debug.Log("MatrixOver!");
            LoadScene(-2, 0);
        }
    }
}

