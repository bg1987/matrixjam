using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class TimeUpdater : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Shader.SetGlobalFloat("_MatrixMapTime", Time.time);
        }
    }
}
