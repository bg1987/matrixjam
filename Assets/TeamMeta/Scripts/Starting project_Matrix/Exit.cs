using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam
{
    public class Exit : Portal
    {
        public void EndLevel()
        {
            LevelHolder.Level.ExitLevel(this);
        }
    }
}
