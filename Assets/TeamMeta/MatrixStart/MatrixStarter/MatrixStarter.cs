using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixStarter : StartHelper
    {
        [SerializeField] SceneManager sceneManager;
        [Header("-1 means select randomly")]
        [SerializeField] int startingSceneIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            if (startingSceneIndex == -1)
                sceneManager.LoadFirstSceneRandomly();
            else
            {
                //ToDo Change -3 into something that makes sense
                sceneManager.LoadScene(startingSceneIndex, -3);
            }

        }
    }
}