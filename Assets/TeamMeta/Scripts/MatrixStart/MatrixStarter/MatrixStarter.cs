using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class MatrixStarter : StartHelper
    {
        [SerializeField] SceneManager sceneManager;
        // Start is called before the first frame update
        void Start()
        {
            sceneManager.LoadRandomScene();
        }
    }
}
