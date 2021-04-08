using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class FrameRate : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }

    }
}
