using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class SceneManagerComponent : MonoBehaviour
    {
        static int startLevel = 0;
        public GameObject[] Levels = new GameObject[2];

        public static SceneManagerComponent instance;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            var level = Levels[startLevel];
            level.SetActive(true);
            /*var playerTransform = Object.FindObjectOfType<PlayerComponent>().transform;
            var startPos = level.GetComponent<StartLevelComponent>().playerStartPos.position;
            playerTransform.position = new Vector3(startPos.x, startPos.y, playerTransform.position.z);*/
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
            ConnectionColorManager.Reset();
            var loadedLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevel.buildIndex);
        }
    }
}
