using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixStarter : StartHelper
    {
        [SerializeField] MatrixTraveler sceneManager;
        [Header("-1 => Select Random Scene")]
        [SerializeField] int startingSceneIndex = 0;
        // Start is called before the first frame update
        void Start()
        {
            if (startingSceneIndex == -1)
                sceneManager.WrapToRandomGame();
            else
            {
                //ToDo Change -3 into something that makes sense
                sceneManager.WrapTo(startingSceneIndex,-1);
            }

        }
    }
}
