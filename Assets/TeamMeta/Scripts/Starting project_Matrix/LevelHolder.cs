using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatrixJam;
using System;
using MatrixJam.TeamMeta;
    
namespace MatrixJam
{
    public class LevelHolder : MonoBehaviour
    {
        Entrance[] entries;
        Exit[] exits;
        int num_lvel;
        int ent_num;
        public int def_ent;
        static LevelHolder level;
        public static LevelHolder Level
        {
            get
            {
                if (level == null)
                {

                    level = MonoBehaviour.FindObjectOfType<LevelHolder>();
                }
                return level;

            }
        }

        private void Start()
        {
            StartGame();
        }
        void StartGame()
        {
            Organize();
            if (SceneManager.SceneMang != null)
            {
                if (SceneManager.SceneMang.Numentrence >= 0)
                {
                    EnterLevel(PlayerData.Data.current_level, SceneManager.SceneMang.Numentrence);
                    return;
                }
            }

            if (PlayerData.Data != null)
            {
                EnterDefault(PlayerData.Data.current_level);
            }
            else
            {
                EnterDefault(GameJamData.TeamNumber);
            }
        }


        public int Current_Level
        {
            get
            {
                return num_lvel;
            }
        }
        public int Entrnce_Used
        {
            get
            {
                return ent_num;
            }
        }
        public void ExitLevel(Exit exit_to)
        {
            if (SceneManager.SceneMang != null)
            {
                if (PlayerData.Data.AddLevel(num_lvel, ent_num, exit_to.Num))
                {
                    SceneManager.SceneMang.MatrixOver();
                }
                else
                {
                    SceneManager.SceneMang.LoadSceneFromExit(num_lvel, exit_to.Num);
                }
            }
            else
            {
                Debug.Log("Game over at exit: " + exit_to.Num);

            }
        }
        public void Organize()
        {
            OrganizeEnters();
            OrganizeExits();
        }

        public void EnterLevel(int lvl_num, int num_ent)
        {
            num_lvel = lvl_num;
            ent_num = num_ent;
            Debug.Log(num_ent);
            entries[num_ent].Enter();
        }

        public void EnterDefault(int lvl)
        {
            if (def_ent <= 0 || def_ent > entries.Length - 1)
            {
                EnterRandom(lvl);
            }
            else
            {
                EnterLevel(lvl, def_ent);
            }
        }
        public void EnterRandom(int lvl)
        {
            System.Random rand = new System.Random();
            EnterLevel(lvl, rand.Next(0, entries.Length));
                
        }
        void OrganizeEnters()
        {
            Entrance[] all_enter = GameObject.FindObjectsOfType<Entrance>();
            entries = new Entrance[all_enter.Length];
            foreach (Entrance runner in all_enter)
            {
                entries[runner.Num] = runner;
            }
        }
        void OrganizeExits()
        {
            Exit[] all_exit = GameObject.FindObjectsOfType<Exit>();
            exits = new Exit[all_exit.Length];
            foreach (Exit runner in all_exit)
            {
                exits[runner.Num] = runner;
            }
        }
        public void Restart()
        {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                entries[ent_num].Enter();
        }

        public int PastPlayed()
        {
            if (PlayerData.Data == null)
            {
                return 0;
            }
            return PlayerData.Data.PastBeen(num_lvel);
        }
    }
}
