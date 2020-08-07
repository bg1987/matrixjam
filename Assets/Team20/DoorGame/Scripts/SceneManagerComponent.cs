using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team20
{
    public class SceneManagerComponent : MonoBehaviour
    {
        public string NextScene;

        // Start is called before the first frame update
        void Start()
        {
            
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public static void RestartScene()
        {
            var loadedLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevel.buildIndex);
        }
    }
}
