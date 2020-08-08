using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class SceneManagerComponent : MonoBehaviour
    {
        public int startLevel = 0;
        public GameObject[] Levels = new GameObject[2];
        GameObject currentLevel;

        public static SceneManagerComponent instance;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            var level = Levels[startLevel];
            currentLevel = Object.Instantiate(level);
            currentLevel.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                RestartScene();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                GoToNextLevel();
            }
        }

        public void GoToNextLevel()
        {
            startLevel++;
            RestartScene();
        }

        public void RestartScene()
        {
            Object.Destroy(currentLevel);
            currentLevel = null;
            var level = Levels[startLevel];
            currentLevel = Object.Instantiate(level);
            currentLevel.SetActive(true);
        }
    }
}
